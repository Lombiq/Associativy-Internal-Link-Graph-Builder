using Associativy.Administration;
using Associativy.InternalLinkGraphBuilder.Models.Pages.Admin;
using Orchard.ContentManagement.Handlers;
using Piedone.HelpfulLibraries.Contents.DynamicPages;

namespace Associativy.InternalLinkGraphBuilder.Handler
{
    public class AdminPageHandler : ContentHandler
    {
        protected override void Initializing(InitializingContentContext context)
        {
            var pageContext = context.PageContext();

            if (pageContext.Group != AdministrationPageConfigs.Group) return;

            if (pageContext.Page.IsPage("ManageGraph", pageContext.Group))
            {
                pageContext.Page.Weld(new AssociativyInternalLinkGraphManageGraphPart());
            }
        }
    }
}