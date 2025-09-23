namespace LibraryProject.Logger
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Skyline.DataMiner.Automation;

    public class EngineLogger : ILogger
    {
        private readonly IEngine _engine;

        public EngineLogger(IEngine engine)
        {
            _engine = engine;
        }

        public void Info<T>(
            string extraInfo,
            IEnumerable<T> messages)
        {
            _engine.Log(
                $"\n[INFO] -{extraInfo}-\n{JsonConvert.SerializeObject(messages, Formatting.Indented)}");
        }

        public void Info(string message)
        {
            _engine.Log(
                $"\n[INFO] {message}");
        }

        public void Warning(string message)
        {
            _engine.Log(
                $"\n[WARN] {message}");
        }

        public void Error(string message)
        {
            _engine.Log(
                $"\n[ERR] {message}");
        }
    }
}
