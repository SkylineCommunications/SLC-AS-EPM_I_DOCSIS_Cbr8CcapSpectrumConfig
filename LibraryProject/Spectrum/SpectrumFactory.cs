namespace LibraryProject.Spectrum
{
	using System.Collections.Generic;
	using LibraryProject.Helpers;
	using LibraryProject.Logger;
	using LibraryProject.SpectrumScript;

	public sealed class SpectrumFactory
        : Container<SpectrumScriptEntry>
    {
        public SpectrumFactory(
            IReadOnlyCollection<ElementConfig> ciscoElements,
            ILogger logger) : base(logger)
        {
            foreach (var element in ciscoElements)
                InitEntities(element);
        }

        public override void Create(
            IReadOnlyCollection<ElementConfig> elements)
        {
            // nothing to do
        }

        public override void InitEntities(ElementConfig element)
        {
            var rawScripts = element
                   .Config
                   .SpectrumAnalyzer
                   .Scripts
                   .GetAllScripts() as object[];

            if (rawScripts == null)
                return;

            foreach (var rawScript in rawScripts)
            {
                var script = rawScript as string[];
                if (script == null)
                    continue;

                var entry = (SpectrumScriptEntry)script;

                entry.Init(
                    element,
                    null);

                Add(entry);
            }
        }
    }
}