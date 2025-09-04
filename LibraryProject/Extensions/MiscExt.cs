namespace LibraryProject.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Common;

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

            foreach (var ciscoElement in ciscoElements)
            {
                yield return new ElementConfig(
                    ciscoElement);
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
    }
}