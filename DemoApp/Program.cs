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
using ExapisSOP.IO.Settings;
using ExapisSOP.Utils;

namespace ExapisSOP.DemoApp
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
			Console.WriteLine("Hello, World!!");
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
				config => config
					.AddFileSystem(async (options) => {
						options.SetDataPath(DefaultPath.Application);
						options.CreateLockFile = false;//true;
						await Task.CompletedTask;
					})
					.AddSettingsSystem(async (options) => {
						options.CreateNewSettings = () => new CustomSettings(new EnvironmentSettings() {
							OutputReadableXML = true,
							Locale = "ja",
							DataStore = new DataStore() {
								["aaa"] = "string value",
								[12345] = new OptimizedSettings(),
								["dat"] = new DataStore() {
									[0] = 0,
									[1] = 1,
									["null"] = null,
									["empty"] = string.Empty,
									["object"] = new DataStore() {
										["default"] = new DefaultSettings(),
										["env"] = new EnvironmentSettings(),
										["custom"] = new CustomSettings() {
											Default = new EnvironmentSettings()
										}
									}
								}
							}
						});
						await Task.CompletedTask;
					})
					.AddAppWorker<Program>()
			).Build().RunAsync();
		}
	}
}
