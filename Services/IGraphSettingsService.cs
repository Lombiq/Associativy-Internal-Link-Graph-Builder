using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lombiq.Associativy.InternalLinkGraphBuilder.Models;
using Orchard;

namespace Lombiq.Associativy.InternalLinkGraphBuilder.Services
{
    public interface IGraphSettingsService : IDependency
    {
        IGraphSettings Get(string graphName);
        void Set(string graphName, IGraphSettings settings);
    }
}
