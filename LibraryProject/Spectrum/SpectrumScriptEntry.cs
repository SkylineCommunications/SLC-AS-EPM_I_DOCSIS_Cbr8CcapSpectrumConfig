namespace LibraryProject.SpectrumScript
{
	using LibraryProject.Helpers;
	using Newtonsoft.Json;

	public class SpectrumScriptEntry
        : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public string DmsIdentifier { get; set; }

        public static implicit operator SpectrumScriptEntry(string[] data)
        {
            return new SpectrumScriptEntry
            {
                Id = data[0],
                Name = data[2],
                Description = data[3],
            };
        }

        public void Init(
            ElementConfig element,
            string @interface)
        {
            DmsIdentifier = $"{element.Config.AgentId}{element.Config.Id}";
        }
    }
}
