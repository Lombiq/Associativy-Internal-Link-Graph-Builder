using Orchard.ContentManagement;

namespace Associativy.InternalLinkGraphBuilder.Models.Pages.Admin
{
    public class AssociativyInternalLinkGraphManageGraphPart : ContentPart
    {
        public GraphSettings GraphSettings { get; set; }


        public AssociativyInternalLinkGraphManageGraphPart()
        {
            GraphSettings = new GraphSettings();
        }
    }
}