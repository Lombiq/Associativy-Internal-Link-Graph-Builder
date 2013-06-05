using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace Lombiq.Associativy.InternalLinkGraphBuilder.Models.Pages.Admin
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