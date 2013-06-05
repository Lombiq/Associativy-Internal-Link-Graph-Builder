using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lombiq.Associativy.InternalLinkGraphBuilder.Models;
using Piedone.HelpfulLibraries.KeyValueStore;

namespace Lombiq.Associativy.InternalLinkGraphBuilder.Services
{
    public class GraphSettingsService : IGraphSettingsService
    {
        private const string SettingsKey = "Lombiq.Associativy.InternalLinkGraphBuilder.Settings";

        private readonly IKeyValueStore _keyValueStore;


        public GraphSettingsService(IKeyValueStore keyValueStore)
        {
            _keyValueStore = keyValueStore;
        }


        public IGraphSettings Get(string graphName)
        {
            var graphSettings = GetSettings().GraphSettings;
            if (graphSettings.ContainsKey(graphName)) return graphSettings[graphName];
            return new GraphSettings();
        }

        public void Set(string graphName, IGraphSettings settings)
        {
            var s = GetSettings();
            s.GraphSettings[graphName] = settings;
            _keyValueStore.Set(SettingsKey, s);
        }


        private InternalLinkGraphSettings GetSettings()
        {
            if (_keyValueStore.Exists(SettingsKey)) return _keyValueStore.Get<InternalLinkGraphSettings>(SettingsKey);

            var settings = new InternalLinkGraphSettings();
            _keyValueStore.Set(SettingsKey, settings);
            return settings;
        }


        // Using a class for this for future-proofness: global settings may come.
        public class InternalLinkGraphSettings
        {
            public Dictionary<string, IGraphSettings> GraphSettings { get; set; }


            public InternalLinkGraphSettings()
            {
                GraphSettings = new Dictionary<string, IGraphSettings>();
            }
        }
    }
}