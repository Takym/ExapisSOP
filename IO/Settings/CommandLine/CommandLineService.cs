/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExapisSOP.Core;
using ExapisSOP.Properties;

namespace ExapisSOP.IO.Settings.CommandLine
{
	internal class CommandLineService : ICommandLineService
	{
		private readonly CommandLineServiceOptions             _options;
		private readonly Func<CommandLineServiceOptions, Task> _optionsCallBack;
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
				var conv = new CommandLineConverter();
				for (int i = 0; i < _options.ResultTypes.Count; ++i) {
					conv.ResultTypes.Add(_options.ResultTypes[i]);
				}
				var dict = _options.GetConverters();
				if (dict != null) {
					foreach (var pair in dict) {
						if (conv.Converters.ContainsKey(pair.Key)) {
							conv.Converters[pair.Key] = pair.Value;
						} else {
							conv.Converters.Add(pair.Key, pair.Value);
						}
					}
				}
				conv.CaseSensitive = _options.CaseSensitive;
				_hasError = !conv.TryConvert(switches, out _converterResult);
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
	}
}
