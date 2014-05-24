
namespace Associativy.InternalLinkGraphBuilder.Models
{
    // Using a class for this for future-proofness: other settings may come.
    public class GraphSettings : IGraphSettings
    {
        public bool ProcessInternalLinks { get; set; }


        public GraphSettings()
        {
        }

        public GraphSettings(IGraphSettings otherSettings)
        {
            ProcessInternalLinks = otherSettings.ProcessInternalLinks;
        }
    }
}