using Associativy.InternalLinkGraphBuilder.Models;
using Orchard;

namespace Associativy.InternalLinkGraphBuilder.Services
{
    public interface IGraphSettingsService : IDependency
    {
        IGraphSettings Get(string graphName);
        void Set(string graphName, IGraphSettings settings);
    }
}
