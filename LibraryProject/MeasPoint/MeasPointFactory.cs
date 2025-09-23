namespace LibraryProject.MeasPoint
{
	using System;
	using System.Collections.Generic;
	using LibraryProject.Helpers;
	using LibraryProject.Logger;

	public sealed class MeasPointFactory
        : Container<MeasPointEntry>
    {
        private readonly List<object> _measPointsToSet = new List<object>();

        public MeasPointFactory(
            string measPointScript,
            ILogger logger) : base(logger)
        {
            MeasPointScript = measPointScript;
        }

        public string MeasPointScript { get; }

        public override void InitEntities(
            ElementConfig element)
        {
            // log
            Logger.Info(
                $"[MEASUREMENT POINT] ({element.Config.Name}) #Interfaces = {element.Interfaces.Count}");

            foreach (var @interface in element.Interfaces)
            {
                try
                {
                    // measurement point creation
                    var measPoint = MiscExt.CreateEntity<MeasPointEntry>(
                        element,
                        @interface);

                    measPoint.Id = HighestId.ToString();
                    measPoint.Script = $"{MeasPointScript};#;PARAMETER:1:{element.Config.DmsElementId.Value};#;OPTIONS:0;#;DEFER:False;#;CHECKSETS:True";

                    // log
                    Logger.Info(
                        $"[MEASUREMENT POINT] ({element.Config.Name}) ID: {measPoint.Id} | Name: {measPoint.Name} | Spectrum Script: {MeasPointScript}");

                    Entities.Add(measPoint);

                    _measPointsToSet.Add(
                        (string[])measPoint);
                }
                catch (Exception e)
                {
                    Logger.Error(
                        $"({element.Config.Name}) Unable to create measurement point for interface '{@interface}'\n\t{e}");
                }
            }
        }

        public override void Create(
            IReadOnlyCollection<ElementConfig> elements)
        {
            foreach (var element in elements)
            {
                InitEntities(element);

                element.Config.SpectrumAnalyzer.SetMeasurementPoints(
                    false,
                    _measPointsToSet.ToArray());

                _measPointsToSet.Clear();
            }
        }
    }
}