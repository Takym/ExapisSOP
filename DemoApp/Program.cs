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

namespace ExapisSOP.DemoApp
{
	internal class Program : AppWorker
	{
		public override async Task InitializeAsync(IContext context)
		{
			this.Startup  += this.Program_Startup;
			this.Update   += this.Program_Update;
			this.Shutdown += this.Program_Shutdown;
			await base.InitializeAsync(context);
			Console.WriteLine("init");
		}

		public override async Task FinalizeAsync(IContext context)
		{
			await base.FinalizeAsync(context);
			Console.WriteLine("final");
			Console.ReadKey(true);
		}

		private void Program_Startup(object? sender, ContextEventArgs e)
		{
			Console.WriteLine(VersionInfo.Caption);
			Console.WriteLine(VersionInfo.Copyright);
			//Console.WriteLine("<your tool name> [v?.?.?.?, cn:unknown]");
			//Console.WriteLine("Copyright (C) <year> <your name>.");
			Console.WriteLine();
			e.Context.SetMessage("Hello, World!!");
			Console.WriteLine("startup");
			Console.ReadKey(true);
		}

		private void Program_Update(object? sender, ContextEventArgs e)
		{
			Console.WriteLine(e.Context.GetMessage());
			Console.WriteLine("update");
			Console.ReadKey(true);
			throw new TerminationException();
		}

		private void Program_Shutdown(object? sender, ContextEventArgs e)
		{
			Console.WriteLine("shutdown");
			Console.ReadKey(true);
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
					.AddAppWorker<Program>()
			).RunAsync();
		}
	}
}
