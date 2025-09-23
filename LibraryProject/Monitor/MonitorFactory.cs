namespace LibraryProject.Monitor
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using LibraryProject.Helpers;
	using LibraryProject.Logger;
	using LibraryProject.MeasPoint;
	using LibraryProject.SpectrumScript;

    public sealed class MonitorFactory
        : Container<MonitorEntry>
    {
        private readonly Container<MeasPointEntry> _measPoints;
        private readonly SpectrumScriptEntry _spectScript;

        public MonitorFactory(
            IReadOnlyCollection<ElementConfig> elements,
            Container<MeasPointEntry> measPoints,
            Container<SpectrumScriptEntry> spectScripts,
            ILogger logger) : base(logger)
        {
            _measPoints = measPoints;

            _spectScript = spectScripts
                .Entities
                .FirstOrDefault();

            if (spectScripts.Count > 1)
            {
                Logger.Warning(
                    $"More than one Spectrum Script available. Picking '{_spectScript?.Name}' (ID: {_spectScript?.Id})");
            }

            foreach (var element in elements)
                InitEntities(element);
        }

        public override void Create(
            IReadOnlyCollection<ElementConfig> elements)
        {
            Logger.Info(
                $"#Monitors = {Entities.Count}");

            foreach (var element in elements)
            {
                foreach (var @interface in element.Interfaces)
                {
                    // monitor creation
                    var newMonitor = MiscExt.CreateEntity<MonitorEntry>(
                        element,
                        @interface);

                    var existingMonitor = Find(
                        x => x.Name,
                        newMonitor.Name); // ccapName_port

                    // find corresponding measurement point ID
                    var measPointId = _measPoints
                            .FindAll(
                                x => x.DmsIdentifier,
                                element.Config.DmsElementId.Value)
                            .FirstOrDefault(
                                x => x.Name == newMonitor.Name)?.Id;

                    // check if monitor already exists
                    if (!Equals(existingMonitor, default))
                    {
                        UpdateMonitor(
                            existingMonitor,
                            element,
                            measPointId,
                            @interface);
                        continue;
                    }

                    CreateMonitor(
                        newMonitor,
                        element,
                        measPointId,
                        @interface);
                }
            }
        }

        public override void InitEntities(
            ElementConfig element)
        {
            var rawMonitors = element
                .Config
                .SpectrumAnalyzer
                .Monitors
                .GetMonitors() as object[];

            if (rawMonitors == null || rawMonitors.Length == 0)
                return;

            foreach (var rawMonitor in rawMonitors)
            {
                var monitor = rawMonitor as string[];
                if (monitor == null || monitor.Length == 0)
                    continue;

                Add(
                    (MonitorEntry)monitor);
            }
        }

        public void RemoveDeprecatedMonitors(
            IReadOnlyCollection<ElementConfig> elements)
        {
            foreach (var element in elements)
            {
                var validNames = new HashSet<string>(
                    element.Interfaces.Select(n => $"{element.Config.Name}_{n}"));

                // find monitors for this element that are not in the validNames set
                var deprecatedMonitors = Entities
                    .Where(
                        m =>
                        m.DmsIdentifier == element.Config.DmsElementId.Value &&
                        !validNames.Contains(m.Name))
                    .ToList();

                foreach (var monitor in deprecatedMonitors)
                {
                    try
                    {
                        Logger.Info(
                            $"[REMOVING Deprecated Monitor] ({element.Config.Name}) Name: {monitor.Name} | ID: {monitor.Id}");

                        element
                            .Config
                            .SpectrumAnalyzer
                            .Monitors
                            .DeleteMonitor(Convert.ToInt32(monitor.Id));

                        Entities.Remove(monitor);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(
                            $"Unable to remove deprecated monitor '{monitor.Name}'\n\t{e}");
                    }
                }
            }
        }

        private void UpdateMonitor(
            MonitorEntry existingMonitor,
            ElementConfig element,
            string measPointId,
            string @interface)
        {
            try
            {
                // assign default measurement point, spectrum script ID, interval
                existingMonitor.MeasPointIds = measPointId;
                existingMonitor.SpectrumScriptId = _spectScript.Id;
                existingMonitor.Interval = element.Interval?.ToString();

                // log
                Logger.Warning(
                    $"[UPDATING Existing Monitor] ({element.Name}) Name: {existingMonitor.Name} | ID: {existingMonitor.Id}");

                // update monitor
                element
                    .Config
                    .SpectrumAnalyzer
                    .Monitors
                    .UpdateMonitor(
                        Convert.ToInt32(existingMonitor.Id),
                        (string[])existingMonitor);
            }
            catch (Exception e)
            {
                Logger.Error(
                    $"Unable to update the monitor '{element.Name}_{@interface}'\n\t{e}");
            }
        }

        private void CreateMonitor(
            MonitorEntry newMonitor,
            ElementConfig element,
            string measPointId,
            string @interface)
        {
            try
            {
                // assign default measurement point, spectrum script ID
                newMonitor.MeasPointIds = measPointId;
                newMonitor.SpectrumScriptId = _spectScript.Id;

                // log
                Logger.Info(
                    $"[NEW Monitor] ({element.Name}) Name: {newMonitor.Name} | MeasPointIds: {newMonitor.MeasPointIds} | Spectrum Script ID: {newMonitor.SpectrumScriptId}");

                // add monitor
                element
                    .Config
                    .SpectrumAnalyzer
                    .Monitors
                    .AddMonitor((string[])newMonitor);
            }
            catch (Exception e)
            {
                Logger.Error(
                    $"Unable to create the monitor '{element.Name}_{@interface}'\n\t{e}");
            }
        }
    }
}