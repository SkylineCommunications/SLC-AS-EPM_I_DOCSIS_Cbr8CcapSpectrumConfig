namespace LibraryProject.Helpers
{
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Skyline.DataMiner.Core.DataMinerSystem.Common;

	public sealed class ElementConfig
    {
        public ElementConfig(
            IDmsElement element)
        {
            Config = element;

            Interval = MiscExt.GetValue<int?>(
                element,
                (int)Cbr8CcapParams.FreeRunDuration) + 10; // + safe delay

            var rawInterfaces = MiscExt.GetValue<string>(
                element,
                (int)Cbr8CcapParams.InterfacesList)
                ?.Split(';');

            if (rawInterfaces != null)
            {
                foreach (var item in rawInterfaces)
                    Interfaces.Add(item);
            }
        }

        [JsonIgnore]
        public IDmsElement Config { get; }

        public string Name => Config?.Name;

        public HashSet<string> Interfaces { get; } = new HashSet<string>();

        public int? Interval { get; }
    }
}