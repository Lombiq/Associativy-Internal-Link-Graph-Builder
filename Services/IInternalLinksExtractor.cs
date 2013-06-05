using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;
using Orchard.ContentManagement;
using Piedone.HelpfulLibraries.Tasks.Jobs;

namespace Lombiq.Associativy.InternalLinkGraphBuilder.Services
{
    public interface IInternalLinksExtractor : IDependency
    {
        void ProcessHtml(ContentItem item, Func<ContentItem, string> htmlFactory);
    }
}
