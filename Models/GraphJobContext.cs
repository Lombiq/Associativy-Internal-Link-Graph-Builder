using System.Collections.Generic;

namespace Lombiq.Associativy.InternalLinkGraphBuilder.Models
{
    public class GraphJobContext
    {
        public int ItemId { get; set; }
        public string ItemType { get; set; }
        public IEnumerable<string> LinkedUrls { get; set; }
    }
}