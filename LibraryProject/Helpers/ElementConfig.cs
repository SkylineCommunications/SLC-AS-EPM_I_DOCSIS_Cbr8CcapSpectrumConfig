namespace LibraryProject.Helpers
{
    using System;
    using System.Collections.Generic;
    using LibraryProject.Extensions;
    using Newtonsoft.Json;
    using Skyline.DataMiner.Core.DataMinerSystem.Common;

    public sealed class ElementConfig
    {
        public ElementConfig(
            IDmsElement element,
            int monitorInterval)
        {
            Config = element;

            if (monitorInterval == -1)
            {
                Interval = MiscExt.GetValue<int?>(
                    element,
                    (int)Cbr8CcapParams.FreeRunDuration) + 10; // + safe delay
            }
            else
            {
                Interval = monitorInterval;
            }

            var rawInterfaces = MiscExt.GetValue<string>(
                element,
                (int)Cbr8CcapParams.InterfacesList);

            Interfaces = ParseInterfaces(rawInterfaces);
        }

        [JsonIgnore]
        public IDmsElement Config { get; }

        public string Name => Config?.Name;

        public IReadOnlyCollection<string> Interfaces { get; }

        public int? Interval { get; }

        private static IReadOnlyCollection<string> ParseInterfaces(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return Array.Empty<string>();

            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var s in raw.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (s.Length > 0)
                    set.Add(s);
            }

            return set;
        }
    }
}