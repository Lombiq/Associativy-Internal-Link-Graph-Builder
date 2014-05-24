using Associativy.Administration;
using Associativy.InternalLinkGraphBuilder.Models.Pages.Admin;
using Piedone.HelpfulLibraries.Contents.DynamicPages;

namespace Associativy.InternalLinkGraphBuilder
{
    public class AdminPageEventHandler : IPageEventHandler
    {
        public void OnPageInitializing(PageContext pageContext)
        {
            if (pageContext.Group != AdministrationPageConfigs.Group) return;

            var page = pageContext.Page;

            if (page.IsPage("ManageGraph", pageContext.Group))
            {
                page.ContentItem.Weld(new AssociativyInternalLinkGraphManageGraphPart());
            }
        }

        public void OnPageInitialized(PageContext pageContext)
        {
        }

        public void OnPageBuilt(PageContext pageContext)
        {
        }

        public void OnAuthorization(PageAutorizationContext authorizationContext)
        {
        }
    }
}