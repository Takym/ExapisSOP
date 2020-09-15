/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Threading.Tasks;
using ExapisSOP.Core;

namespace ExapisSOP.DemoApp
{
	internal class Program : AppWorker
	{
		public override async Task InitializeAsync(IContext context)
		{
			this.Startup += this.Program_Startup;
			this.Update  += this.Program_Update;
			await base.InitializeAsync(context);
		}

		private void Program_Startup(object? sender, ContextEventArgs e)
		{
			Console.WriteLine(VersionInfo.Caption);
			Console.WriteLine(VersionInfo.Copyright);
			//Console.WriteLine("<your tool name> [v?.?.?.?, cn:unknown]");
			//Console.WriteLine("Copyright (C) <year> <your name>.");
			Console.WriteLine();
			e.Context.SetMessage("Hello, World!!");
		}

		private void Program_Update(object? sender, ContextEventArgs e)
		{
			Console.WriteLine(e.Context.GetMessage());
			Console.ReadKey(true);
			throw new TerminationException();
		}

		[STAThread()]
		private static async Task<int> Main(string[] args)
		{
			return await HostRunner.Create(args).Configure(
				config => config.AddAppWorker<Program>()
			).RunAsync();
		}
	}
}
