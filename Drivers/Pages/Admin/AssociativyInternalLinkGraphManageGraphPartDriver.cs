using Associativy.Administration.Models.Pages.Admin;
using Lombiq.Associativy.InternalLinkGraphBuilder.Models.Pages.Admin;
using Lombiq.Associativy.InternalLinkGraphBuilder.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Lombiq.Associativy.InternalLinkGraphBuilder.Drivers.Pages.Admin
{
    public class AssociativyInternalLinkGraphManageGraphPartDriver : ContentPartDriver<AssociativyInternalLinkGraphManageGraphPart>
    {
        private readonly IGraphSettingsService _settingsService;

        protected override string Prefix { get { return "Lombiq.Associativy.InternalLinkGraphBuilder.AssociativyInternalLinkGraphManageGraphPart"; } }


        public AssociativyInternalLinkGraphManageGraphPartDriver(IGraphSettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        

        protected override DriverResult Display(AssociativyInternalLinkGraphManageGraphPart part, string displayType, dynamic shapeHelper)
        {
            return Editor(part, shapeHelper);
        }

        protected override DriverResult Editor(AssociativyInternalLinkGraphManageGraphPart part, dynamic shapeHelper)
        {
            return ContentShape("Pages_AssociativyInternalLinkGraphManageGraph",
                () =>
                {
                    var settings = _settingsService.Get(GraphName(part));
                    part.GraphSettings.ProcessInternalLinks = settings.ProcessInternalLinks;

                    return shapeHelper.DisplayTemplate(
                                    TemplateName: "Pages/Admin/InternalLinkGraphManageGraph",
                                    Model: part,
                                    Prefix: Prefix);
                });
        }

        protected override DriverResult Editor(AssociativyInternalLinkGraphManageGraphPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
                _settingsService.Set(GraphName(part), part.GraphSettings);
            }

            return Editor(part, shapeHelper);
        }


        private static string GraphName(AssociativyInternalLinkGraphManageGraphPart part)
        {
            return part.As<AssociativyManageGraphPart>().GraphDescriptor.Name;
        }
    }
}