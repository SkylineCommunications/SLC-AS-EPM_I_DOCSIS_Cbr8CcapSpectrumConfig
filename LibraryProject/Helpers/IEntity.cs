namespace LibraryProject.Helpers
{
    public interface IEntity
    {
        string Id { get; set; }

        string DmsIdentifier { get; }

        void Init(
            ElementConfig element,
            string @interface);
    }
}
