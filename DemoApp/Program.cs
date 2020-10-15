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

			var e0 = new AggregateException(
				new NullReferenceException(),
				new NotImplementedException(),
				new ObjectDisposedException("abcd"),
				new TerminationException(),
				new ArgumentOutOfRangeException(
					"引数", "値", "メッセージ"
				),
				new CultureNotFoundException(
					"引数", "iv", "メッセージ"
				),
				new Exception(
					"内部例外を持つ例外1",
					new ApplicationException(
						"アプリエラー",
						new Win32Exception(123, "めっせーじ")
					)
				),
				new Exception(
					"内部例外を持つ例外2",
					new FileNotFoundException(
						"メッセージ", "ファイル名",
						new FileLoadException(
							"メッセージ", "ファイル名",
							new InvalidPathFormatException(
								"ファイルパス",
								new TerminationException(
									new TerminationException(
										"メッセージ",
										TerminationReason.NoCompatible,
										CancellationToken.None
									)
								)
							)
						)
					)
				),
				new XmlException("xmlerror", new SerializationException(), 12, 34),
				new DivideByZeroException(),
				new StackOverflowException(),
				new PlatformNotSupportedException(
					"テスト",
					new InvalidOperationException(
						"メッセージ",
						new InvalidCastException()
					)
				),
				new InvalidOperationException()
			);
			var e1 = new Exception("エラーメッセージ", e0);

			try {
				throw e1;
			} catch (Exception e2) {
				e.Context.GetLoggingSystem()?.SaveErrorReport(e.Context, e2, new HResultDetailProvider());
			}

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
