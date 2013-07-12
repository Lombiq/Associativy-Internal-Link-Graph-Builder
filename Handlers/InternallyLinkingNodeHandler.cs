using System;
using System.Collections.Generic;
using System.Linq;
using Lombiq.Associativy.InternalLinkGraphBuilder.Services;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Common.Models;
using Orchard.Core.Common.Settings;
using Orchard.Environment;
using Orchard.Services;

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

                            return htmlFiltersWork.Value.Aggregate(part.Text, (text, filter) => filter.ProcessContent(text, flavor));
                        });
                });
        }
    }
}