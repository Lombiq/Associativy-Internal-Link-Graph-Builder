using System;
using Orchard;
using Orchard.ContentManagement;

namespace Associativy.InternalLinkGraphBuilder.Services
{
    public interface IInternalLinksExtractor : IDependency
    {
        void ProcessHtml(ContentItem item, Func<ContentItem, string> htmlFactory);
    }
}
