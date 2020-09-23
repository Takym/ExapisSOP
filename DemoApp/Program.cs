/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Security;
using System.Threading.Tasks;
using ExapisSOP.Core;
using ExapisSOP.IO;
using ExapisSOP.NativeWrapper;
using ExapisSOP.Utils;

namespace ExapisSOP.DemoApp
{
	internal sealed class Program : AppWorker
	{
		private readonly SecureString _correct_password = new SecureString();

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
			ConsoleUtils.WriteHorizontalRule();
			_correct_password.AppendChar('e');
			_correct_password.AppendChar('x');
			_correct_password.AppendChar('i');
			_correct_password.AppendChar('t');
			_correct_password.MakeReadOnly();
		}

		private void Program_Update(object? sender, ContextEventArgs e)
		{
			using (var pass = ConsoleUtils.ReadPassword()) {
				if (pass.IsEqualWith(_correct_password) && ConsoleUtils.ReadYesNo("Close?")) {
					ConsoleUtils.WriteHorizontalRule('-');
					throw new TerminationException();
				}
			}
		}

		private void Program_Shutdown(object? sender, ContextEventArgs e)
		{
			_correct_password.Dispose();
			ConsoleUtils.Pause();
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
