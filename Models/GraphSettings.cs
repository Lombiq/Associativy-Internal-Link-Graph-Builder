using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.Associativy.InternalLinkGraphBuilder.Models
{
    // Using a class for this for future-proofness: other settings may come.
    public class GraphSettings : IGraphSettings
    {
        public bool ProcessInternalLinks { get; set; }
    }
}