// Ignore Spelling: Cbr Ccap

namespace SLCASEPMIDOCSISCbr8CcapSpectrumConfig_1
{
	using System;
	using System.Linq;
	using LibraryProject.Helpers;
	using LibraryProject.Logger;
	using LibraryProject.MeasPoint;
	using LibraryProject.Monitor;
	using LibraryProject.Spectrum;
	using Skyline.DataMiner.Automation;

    /// <summary>
    /// Represents a DataMiner Automation script.
    /// </summary>
	public class Script
    {
        /// <summary>
        /// The script entry point.
        /// </summary>
        /// <param name="engine">Link with SLAutomation process.</param>
        public static void Run(IEngine engine)
        {
            try
            {
                engine.Timeout = TimeSpan.FromMinutes(30);
                RunSafely(engine);
            }
            catch (ScriptAbortException)
            {
                // Catch normal abort exceptions (engine.ExitFail or engine.ExitSuccess)
                throw; // Comment if it should be treated as a normal exit of the script.
            }
            catch (ScriptForceAbortException)
            {
                // Catch forced abort exceptions, caused via external maintenance messages.
                throw;
            }
            catch (ScriptTimeoutException)
            {
                // Catch timeout exceptions for when a script has been running for too long.
                throw;
            }
            catch (InteractiveUserDetachedException)
            {
                // Catch a user detaching from the interactive script by closing the window.
                // Only applicable for interactive scripts, can be removed for non-interactive scripts.
                throw;
            }
            catch (Exception e)
            {
                engine.ExitFail(
                    "Run|Something went wrong: " + e);
            }
        }

        private static void RunSafely(IEngine engine)
        {
            var logger = new EngineLogger(engine);

            var elements = engine
                .GetCiscoElements()
                .ToArray();

            if (elements.Length == 0)
            {
                logger.Error(
                    "No active Cisco UTSC elements are set with the production version.");
                return;
            }

            // log
            logger.Info(
                $"Active Cisco UTSC elements",
                elements);

            // it's only possible to create a monitor in DataMiner if at least one Spectrum Script is available
            var spectScripts = new SpectrumFactory(
                elements,
                logger);

            if (spectScripts.Count == 0)
            {
                logger.Error(
                    "No Spectrum scripts available for Monitor creation. Please create at least one Spectrum Script.");
                return;
            }

            // log
            logger.Info(
                $"Spectrum Scripts available",
                spectScripts.Entities);

            // measurement points creation
            var spectSettingsScript = engine
                .GetScriptParam(0)
                .Value;

            if (string.IsNullOrWhiteSpace(spectSettingsScript))
            {
                logger.Error(
                    "Measurement Point Script not defined!");
                return;
            }

            var measPoints = new MeasPointFactory(
                spectSettingsScript,
                logger);

            measPoints
                .Create(elements);

            // monitors creation
            var monitors = new MonitorFactory(
                elements,
                measPoints,
                spectScripts,
                logger);

            monitors
                .Create(elements);
        }
    }
}