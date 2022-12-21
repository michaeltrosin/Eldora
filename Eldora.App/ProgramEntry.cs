using System;
using System.Windows.Forms;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Eldora.App;

internal static class ProgramEntry
{
	/// <summary>
	/// The main entry point for the application.
	/// </summary>
	[STAThread]
	private static void Main()
	{
		InternalPaths.CreateFolderStructure();
		InitiateLogging();
		EldoraApp.Initalize();

		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new MainWindow());

		EldoraApp.SaveSettings();
	}

	private static void InitiateLogging()
	{
		const string layout = "[${longdate} | ${level:uppercase=true}] (${logger}) > ${message:withexception=true}";

		var config = new LoggingConfiguration();

		var logfile = new FileTarget("log_file")
		{
			FileName = $@"{InternalPaths.LogPath}\log.log",
			Layout = layout,
			ArchiveOldFileOnStartup = true
		};
		var consoleTarget = new ConsoleTarget("log_console")
		{
			Layout = layout
		};

		var debugLogFile = new FileTarget("debug_log_file")
		{
			FileName = $@"{InternalPaths.LogPath}\debug.log",
			Layout = layout,
			ArchiveOldFileOnStartup = true
		};

		config.AddRule(LogLevel.Info, LogLevel.Fatal, consoleTarget);
		config.AddRule(LogLevel.Info, LogLevel.Fatal, logfile);
		config.AddRule(LogLevel.Debug, LogLevel.Fatal, debugLogFile);

		LogManager.Configuration = config;
	}
}