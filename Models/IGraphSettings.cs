using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lombiq.Associativy.InternalLinkGraphBuilder.Models
{
    public interface IGraphSettings
    {
        bool ProcessInternalLinks { get; }
    }
}
