namespace LibraryProject.Monitor
{
    using LibraryProject.Helpers;

    public class MonitorEntry
        : IEntity
    {
        public string DmsIdentifier
            => $"{DmaId}/{ElementId}";

        public string Id { get; set; }

        public string DmaId { get; set; }

        public string ElementId { get; set; }

        public string Interval { get; set; }

        public string SpectrumScriptId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Enabled { get; set; } = "FALSE"; // enabled

        public string Unknown1 { get; set; } = "10:5";

        public string Unknown2 { get; set; } = "0";

        public string Unknown3 { get; set; } = "TRUE";

        public string Unknown4 { get; set; } = "0";

        public string CreateServiceFlag { get; set; } = "FALSE"; // disabled

        public string CreateServiceId { get; set; } = string.Empty;

        public string MeasPointIds { get; set; } = string.Empty;

        public static explicit operator string[](MonitorEntry data)
        {
            return new[]
            {
                data.SpectrumScriptId,
                data.DmaId,
                data.ElementId,
                data.Interval,
                data.Name,
                data.Description,
                data.Enabled,
                data.Unknown1,
                data.MeasPointIds,
                data.Unknown3,
                data.Unknown4,
                data.Unknown2,
                data.CreateServiceFlag,
                data.CreateServiceId,
                string.Empty,
            };
        }

        public static implicit operator MonitorEntry(string[] data)
        {
            return new MonitorEntry
            {
                Id = data[0],
                DmaId = data[1],
                ElementId = data[2],
                Interval = data[3],
                SpectrumScriptId = data[4],
                Name = data[5],
                Description = data[6],
                Enabled = data[7],
                Unknown1 = data[8],
                Unknown2 = data[9],
                Unknown3 = data[10],
                Unknown4 = data[11],
                CreateServiceFlag = data[12],
                CreateServiceId = data[13],
                MeasPointIds = data[14],
            };
        }

        public void Init(
            ElementConfig element,
            string @interface)
        {
            DmaId = element.Config.AgentId.ToString();
            ElementId = element.Config.Id.ToString();
            Interval = element.Interval?.ToString();
            Name = $"{element.Config.Name}_{@interface}";
        }
    }
}
