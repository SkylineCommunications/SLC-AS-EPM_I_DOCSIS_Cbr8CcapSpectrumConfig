namespace LibraryProject.MeasPoint
{
	using LibraryProject.Helpers;

	public class MeasPointEntry
        : IEntity
    {
        public string DmsIdentifier
            => $"{DmaId}/{ElementId}";

        public string Id { get; set; }

        public string DmaId { get; set; } = string.Empty;

        public string ElementId { get; set; } = string.Empty;

        public string WrParamIds { get; set; } = string.Empty;

        public string Name { get; set; }

        public string ParamFlags { get; set; } = string.Empty;

        public string SetParams { get; set; } = string.Empty;

        public string DelayAfterSet { get; set; } = "0";

        public string GetParams { get; set; } = string.Empty;

        public string ParamIdxs { get; set; } = string.Empty;

        public string Unknown1 { get; set; } = string.Empty;

        public string FreqOffset { get; set; } = "0";

        public string InvFreq { get; set; } = "false";

        public string Script { get; set; }

        public string Unknown2 { get; set; } = string.Empty;

        public string AmplCorr { get; set; } = string.Empty;

        public static explicit operator string[](MeasPointEntry data)
        {
            return new[]
            {
                data.Id,
                data.DmaId,
                data.ElementId,
                data.WrParamIds,
                data.Name,
                data.ParamFlags,
                data.SetParams,
                data.DelayAfterSet,
                data.GetParams,
                data.ParamIdxs,
                data.FreqOffset,
                data.InvFreq,
                data.Script,
                data.AmplCorr,
            };
        }

        public static implicit operator MeasPointEntry(string[] data)
        {
            return new MeasPointEntry
            {
                Id = data[0],
                Name = data[4],
                Script = data[13],
            };
        }

        public void Init(
            ElementConfig element,
            string @interface)
        {
            Name = $"{element.Config.Name}_{@interface}";
            DmaId = element.Config.AgentId.ToString();
            ElementId = element.Config.Id.ToString();
        }
    }
}