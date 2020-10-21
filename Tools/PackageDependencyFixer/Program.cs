/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Threading.Tasks;
using ExapisSOP.Core;
using ExapisSOP.IO;
using ExapisSOP.IO.Logging;
using ExapisSOP.IO.Settings;
using ExapisSOP.Utils;

namespace ExapisSOP.Tools.PackageDependencyFixer
{
	internal sealed class Program : AppWorker
	{
		public override async Task InitializeAsync(IContext context)
		{
			this.Startup  += this.Program_Startup;
			this.Update   += this.Program_Update;
			this.Shutdown += this.Program_Shutdown;
			await base.InitializeAsync(context);
		}

		private void Program_Startup(object? sender, ContextEventArgs e)
		{
			Console.WriteLine(VersionInfo.Caption);
			Console.WriteLine(VersionInfo.Copyright);
			ConsoleUtil.WriteHorizontalRule();
		}

		private void Program_Update(object? sender, ContextEventArgs e)
		{
			var logger = e.Context.LogFile?.GetConsoleLogger();
			logger?.Info("This tool fixes the NuGet package dependency of ExapisSOP.");
			Console.WriteLine();
			PDFixerCore.Fix(logger, e.Context);
			throw new TerminationException();
		}

		private void Program_Shutdown(object? sender, ContextEventArgs e)
		{
			ConsoleUtil.Pause();
		}

		[STAThread()]
		private static async Task<int> Main(string[] args)
		{
			return await HostRunner.Create(args).Configure(
				config => {
					config
						.AddSystemServices(
							fileSystemOptions: async (options) => {
								options.SetDataPath(DefaultPath.Application);
								options.CreateLockFile = false;
								await Task.CompletedTask;
							},
							settingsSystemOptions: async (options) => {
								options.CreateNewSettings = ()
									=> new CustomSettings(new EnvironmentSettings() {
										OutputReadableXML = false,
										Locale            = "en",
										EnableLogging     = true
									});
								await Task.CompletedTask;
							},
							commandLineOptions: async (options) => {
								options.AllowOverrideSettings = false;
								await Task.CompletedTask;
							},
							loggingSystemOptions: async (options) => {
								options.FileFactory       = (stream) => new TextAndXmlLogFile(stream);
								options.CheckServiceState = true;
								options.LogOnUpdate       = true;
								options.UseLongName       = true;
								await Task.CompletedTask;
							}
						)
						.AddAppWorker<Program>();
					config.UnhandledError += Config_UnhandledError;
				}
			).Build().RunAsync();
		}

		private static void Config_UnhandledError(object? sender, UnhandledErrorEventArgs e)
		{
			var path = e.Context.GetLoggingSystem()?.SaveErrorReport(e.Context, e.Exception, new HResultDetailProvider());
			if (e.Context.Paths is IPathList paths) {
				ErrorReportBuilder.SaveERBC(paths);
			}
			var logger = e.Context.LogFile?.GetConsoleLogger();
			logger?.UnhandledException(e.Exception);
			logger?.Info("The error report file is saved to: " + path);
			e.Abort = true;
		}
	}
}
