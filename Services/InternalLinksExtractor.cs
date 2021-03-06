﻿using System;
using System.Collections.Generic;
using System.Linq;
using Associativy.GraphDiscovery;
using HtmlAgilityPack;
using Associativy.InternalLinkGraphBuilder.Models;
using Orchard.Alias;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Environment;
using Orchard.Services;
using Orchard.Settings;
using Orchard.Tasks.Scheduling;
using Piedone.HelpfulLibraries.Tasks;
using Piedone.HelpfulLibraries.Tasks.Jobs;
using Orchard.Logging;

namespace Associativy.InternalLinkGraphBuilder.Services
{
    public class InternalLinksExtractor : IInternalLinksExtractor, IScheduledTaskHandler, IOrchardShellEvents
    {
        private const string TaskType = "Associativy.InternalLinkGraphBuilder.InternalLinksGraphUpdater";
        private const string Industry = "Associativy.InternalLinkGraphBuilder.InternalLinksBuilding";
        private const int ItemBatch = 10;
        private const int RunPerMinutes = 5;

        private readonly IGraphManager _graphManager;
        private readonly IContentManager _contentManager;
        private readonly ISiteService _siteService;
        private readonly IJobManager _jobManager;
        private readonly IAliasService _aliasService;
        private readonly IScheduledTaskManager _scheduledTaskManager;
        private readonly IClock _clock;
        private readonly IGraphSettingsService _settingsService;

        public ILogger Logger { get; set; }


        public InternalLinksExtractor(
            IGraphManager graphManager,
            IContentManager contentManager,
            ISiteService siteService,
            IJobManager jobManager,
            IAliasService aliasService,
            IScheduledTaskManager scheduledTaskManager,
            IClock clock,
            IGraphSettingsService settingsService)
        {
            _graphManager = graphManager;
            _contentManager = contentManager;
            _siteService = siteService;
            _jobManager = jobManager;
            _aliasService = aliasService;
            _scheduledTaskManager = scheduledTaskManager;
            _clock = clock;
            _settingsService = settingsService;

            Logger = NullLogger.Instance;
        }


        public void ProcessHtml(ContentItem item, Func<ContentItem, string> htmlFactory)
        {
            var graphs = _graphManager
                .FindGraphs(new GraphContext { ContentTypes = new[] { item.ContentType } })
                .Where(graph => _settingsService.Get(graph.Name).ProcessInternalLinks);

            if (!graphs.Any()) return;

            var html = htmlFactory(item);
            if (string.IsNullOrEmpty(html)) return;

            // Since this can be called from a background task, we can't get the base url from the request.
            var siteUri = new Uri(_siteService.GetSiteSettings().BaseUrl);
            var urls = new HashSet<string>();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var links = doc.DocumentNode.SelectNodes("//a[@href]");
            if (links == null) return; // See: https://htmlagilitypack.codeplex.com/workitem/29175
            var aliasAspect = item.As<IAliasAspect>();
            if (aliasAspect == null) throw new ArgumentException("The content item should have a part that implements IAliasAspect.");
            var itemUri = new Uri(siteUri, aliasAspect.Path);
            foreach (var link in links)
            {
                var href = link.GetAttributeValue("href", null);
                if (href != null)
                {
                    try
                    {
                        if (Uri.IsWellFormedUriString(href, UriKind.Relative))
                        {
                            urls.Add(new Uri(itemUri, href).LocalPath);
                        }
                        else
                        {
                            var uri = new Uri(href);
                            if (uri.Host == siteUri.Host) urls.Add(uri.LocalPath);
                        }
                    }
                    catch (UriFormatException ex)
                    {
                        // Such errors shouldn't cause other processes to halt.
                        Logger.Warning(ex, $"Couldn't follow the internal URL {href} because it has an invalid format.");
                    }
                }
            }

            if (urls.Count == 0) return;

            // Queueing this defers execution. Thus if an item is linked that isn't published yet there's a chance it will be published
            // until the job runs.
            _jobManager.CreateJob(Industry, new GraphJobContext { ItemId = item.Id, ItemType = item.ContentType, LinkedUrls = urls }, 0);
        }

        public void Process(ScheduledTaskContext context)
        {
            if (context.Task.TaskType != TaskType) return;

            Renew(true);

            for (int i = 0; i < ItemBatch; i++)
            {
                var job = _jobManager.TakeJob(Industry);

                if (job == null) return;

                var jobContext = job.Context<GraphJobContext>();

                var graphs = _graphManager.FindGraphs(new GraphContext { ContentTypes = new[] { jobContext.ItemType } });

                foreach (var graph in graphs.Where(graph => _settingsService.Get(graph.Name).ProcessInternalLinks))
                {
                    foreach (var url in jobContext.LinkedUrls)
                    {
                        var route = _aliasService.Get(url.TrimStart('/'));
                        if (route != null && (route.ContainsKey("Id") || route.ContainsKey("threadId"))) // threadId is hack for NGM.Forum threads
                        {
                            var target = route.ContainsKey("Id") ? _contentManager.Get(Convert.ToInt32(route["Id"])) : _contentManager.Get(Convert.ToInt32(route["threadId"]));
                            if (target != null && graph.ContentTypes.Contains(target.ContentType))
                            {
                                graph.Services.ConnectionManager.Connect(jobContext.ItemId, target.Id);
                            }
                        }
                    } 
                }

                _jobManager.Done(job);
            }
        }

        public void Activated()
        {
            Renew(false);
        }

        public void Terminating()
        {
        }


        private void Renew(bool calledFromTaskProcess)
        {
            _scheduledTaskManager.CreateTaskIfNew(TaskType, _clock.UtcNow.AddMinutes(RunPerMinutes), null, calledFromTaskProcess);
        }
    }
}