using Associativy.GraphDiscovery;
using HtmlAgilityPack;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Common.Models;
using Orchard.Environment.Extensions;
using Orchard.Settings;
using Piedone.HelpfulLibraries.Tasks.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.Mvc;
using Orchard.Environment;
using Orchard.Services;
using Orchard.Core.Common.Settings;
using Lombiq.Associativy.InternalLinkGraphBuilder.Services;

namespace Lombiq.Associativy.InternalLinkGraphBuilder.Handlers
{
    public class InternallyLinkingNodeHandler : ContentHandler
    {
        public InternallyLinkingNodeHandler(
            Work<IInternalLinksExtractor> extractorWork,
            Work<IEnumerable<IHtmlFilter>> htmlFiltersWork)
        {
            OnPublished<BodyPart>((context, part) =>
                {
                    extractorWork.Value.ProcessHtml(part.ContentItem, item =>
                        {
                            // Sadly copied from BodyPartDriver
                            var typePartSettings = part.Settings.GetModel<BodyTypePartSettings>();
                            var flavor = (typePartSettings != null && !string.IsNullOrWhiteSpace(typePartSettings.Flavor))
                                       ? typePartSettings.Flavor
                                       : part.PartDefinition.Settings.GetModel<BodyPartSettings>().FlavorDefault;

                            return htmlFiltersWork.Value
                                .Where(x => x.GetType().Name.Equals(flavor + "filter", StringComparison.OrdinalIgnoreCase))
                                .Aggregate(part.Text, (text, filter) => filter.ProcessContent(text));
                        });
                });
        }
    }
}