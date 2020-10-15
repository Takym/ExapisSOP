/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using ExapisSOP.Core;
using ExapisSOP.IO;
using ExapisSOP.IO.Logging;
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
						options.CreateLockFile = true;
						await Task.CompletedTask;
					})
					.AddSettingsSystem()
					.AddCommandLine()
					.AddLoggingSystem()
					.AddUtility()
					.AddAppWorker<Program>()
			).Build().RunAsync();
		}
	}
}
