namespace LibraryProject.Logger
{
	using System.Collections.Generic;

	public interface ILogger
    {
        void Info(string message);

        void Info<T>(string extraInfo,IEnumerable<T> messages);

        void Warning(string message);

        void Error(string message);
    }
}
