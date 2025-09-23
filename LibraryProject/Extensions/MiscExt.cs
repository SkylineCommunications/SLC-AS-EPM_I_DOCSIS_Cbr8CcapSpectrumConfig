namespace LibraryProject.Helpers
{
    using Skyline.DataMiner.Automation;
    using Skyline.DataMiner.Core.DataMinerSystem.Automation;
    using Skyline.DataMiner.Core.DataMinerSystem.Common;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public static class MiscExt
    {
        public static T GetValue<T>(
            IDmsElement element,
            int readPid)
        {
            return element
                .GetStandaloneParameter<T>(readPid)
                .GetValue();
        }

        public static IEnumerable<ElementConfig> GetCiscoElements(
            this IEngine engine)
        {
            var ciscoElements = engine
                .GetDms()
                .GetElements()
                .Where(
                    x => x.State.Equals(ElementState.Active) &&
                    x.Protocol.Name.Equals(Constants.CiscoProtocol) &&
                    x.Protocol.Version.Equals("Production"));

            // monitor interval
            var rawMonitorInterval = engine
                .GetScriptParam(2)
                .Value;

            int inputMonitorInterval = ParseInputMonitorInterval(
                rawMonitorInterval);

            foreach (var ciscoElement in ciscoElements)
            {
                yield return new ElementConfig(
                    ciscoElement,
                    inputMonitorInterval);
            }
        }

        public static T CreateEntity<T>(
            ElementConfig element,
            string @interface) where T : IEntity, new()
        {
            T t = new T();
            t.Init(
                element,
                @interface);

            return t;
        }

        private static int ParseInputMonitorInterval(
            string raw,
            int defaultValue = -1)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return defaultValue;
            }

            if (!int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
            {
                throw new FormatException(
                    $"Invalid monitor interval: '{raw}'");
            }

            return value > 0 ?
                value :
                defaultValue;
        }
    }
}