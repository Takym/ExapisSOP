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
using ExapisSOP.IO.Settings.CommandLine;
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

			var conv = new CommandLineConverter();
			conv.ResultTypes.Add(typeof(EnvironmentSettings));
			//bool isValid = conv.TryConvert(conv.Convert("/Settings", "-output-readable-xml", "-lang", "zh-CN", "-logging:enabled"), out var result);
			//bool isValid = conv.TryConvert(conv.Convert("/S", "-x", "-l", "en", "-g"), out var result);
			//bool isValid = conv.TryConvert(conv.Convert("cmd", "-opt", "/S", "-x", "/hello"), out var result);
			bool isValid = conv.TryConvert(conv.Convert(e.Context.GetHostRunner().Arguments), out var result);
			if (isValid) {
				Console.WriteLine("Valid!");
			} else {
				Console.WriteLine("Invalid...");
			}
			var result2 = (result[typeof(EnvironmentSettings)] as EnvironmentSettings);
			Console.WriteLine($"OutputReadableXML: {result2?.OutputReadableXML}");
			Console.WriteLine($"Locale           : {result2?.Locale}");
			Console.WriteLine($"EnableLogging    : {result2?.EnableLogging}");
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
						options.CreateLockFile = true;
						await Task.CompletedTask;
					})
					.AddSettingsSystem()
					.AddUtility()
					.AddAppWorker<Program>()
			).Build().RunAsync();
		}
	}
}
