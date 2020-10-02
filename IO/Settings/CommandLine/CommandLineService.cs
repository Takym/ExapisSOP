/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using ExapisSOP.Core;
using ExapisSOP.Properties;
using ExapisSOP.Utils;

namespace ExapisSOP.IO.Settings.CommandLine
{
	internal class CommandLineService : ICommandLineService
	{
		private readonly CommandLineServiceOptions             _options;
		private readonly Func<CommandLineServiceOptions, Task> _optionsCallBack;
		private          CommandLineConverter?                 _conv;
		private          CLManualsCore?                        _man;
		private          IDictionary<Type, object>?            _converterResult;
		private          bool                                  _hasError;

		internal CommandLineService(Func<CommandLineServiceOptions, Task> callBackFunc)
		{
			_options         = new CommandLineServiceOptions();
			_optionsCallBack = callBackFunc;
		}

		public async Task InitializeAsync(IContext context)
		{
			await _optionsCallBack(_options);

			// Analyze the command line arguments
			var switches = CommandLineParser.Parse(context.GetHostRunner().Arguments);

			// Set the result
			if (context is InitFinalContext initContext && initContext.IsInitializationPhase()) {
				initContext.Arguments = switches;
			}

			// Convert switches to objects
			if (_options.ConvertToObject) {
				_conv = new CommandLineConverter();
				for (int i = 0; i < _options.ResultTypes.Count; ++i) {
					_conv.ResultTypes.Add(_options.ResultTypes[i]);
				}
				if (_options.AllowOverrideSettings) {
					_conv.ResultTypes.Add(typeof(EnvironmentSettings));
				}
				var dict = _options.GetConverters();
				if (dict != null) {
					foreach (var pair in dict) {
						if (_conv.Converters.ContainsKey(pair.Key)) {
							_conv.Converters[pair.Key] = pair.Value;
						} else {
							_conv.Converters.Add(pair.Key, pair.Value);
						}
					}
				}
				_conv.CaseSensitive = _options.CaseSensitive;
				_hasError = !_conv.TryConvert(switches, out _converterResult);

				// Apply settings
				if (_options.AllowOverrideSettings) {
					var envcfg = _converterResult.GetValue<EnvironmentSettings?>();
					if (envcfg != null) {
						context.GetSettingsSystem()?.Apply(envcfg);
					}
				}
			}
		}

		public Task FinalizeAsync(IContext context)
		{
			return Task.CompletedTask;
		}

		public void TerminateWhenError()
		{
			if (_hasError) {
				throw new TerminationException(
					Resources.CommandLineService_TerminationException,
					TerminationReason.InvalidCommandLine,
					CancellationToken.None);
			}
		}

		public IDictionary<Type, object>? GetValues()
		{
			return _converterResult;
		}

		public void PrintManuals(IContext context, string copyright)
		{
			if (_conv != null) {
				if (_conv.ResetCache || _man == null) {
					_man = new CLManualsCore(_conv, CultureInfo.CurrentCulture);
				}
				if (context.Arguments?.DoShowHelp() ?? false) {
					ConsoleUtil.WriteHorizontalRule('-');
					Console.WriteLine(Resources.CommandLineService_PrintManuals_Help);
					Console.WriteLine(_man.GetHelpText());
					ConsoleUtil.WriteHorizontalRule('-');
				}
				if (context.Arguments?.DoShowVersion() ?? false) {
					var lib = context.GetSettingsSystem()?.GetLibraryVersion();
					var app = context.GetSettingsSystem()?.GetApplicationVersion();
					ConsoleUtil.WriteHorizontalRule('-');
					Console.WriteLine(Resources.CommandLineService_PrintManuals_Version);
					Console.WriteLine(_man.GetVersionText(
						lib?.version ?? VersionInfo.VersionString, lib?.codename ?? VersionInfo.CodeName, VersionInfo.Copyright,
						app?.version ?? "?.?.?.?",                 app?.codename ?? "unknown",            copyright
					));
					ConsoleUtil.WriteHorizontalRule('-');
				}
			}
		}
	}
}
