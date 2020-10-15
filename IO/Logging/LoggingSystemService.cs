/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExapisSOP.Core;
using ExapisSOP.IO.Settings;
using ExapisSOP.IO.Settings.CommandLine;

namespace ExapisSOP.IO.Logging
{
	internal sealed class LoggingSystemService : AppWorker, ILoggingSystemService
	{
		#region 動的

		private  readonly LoggingSystemServiceOptions             _options;
		private  readonly Func<LoggingSystemServiceOptions, Task> _optionsCallBack;
		private           FileStream?                             _logStream;
		private           ILogFile                                _logFile;
		private           SystemLogger?                           _sysLogger;
		private  readonly DefaultErrorDetailProvider              _default_detailProvider;
		internal          bool                                    LogOnUpdate     => _options.LogOnUpdate;

		internal LoggingSystemService(Func<LoggingSystemServiceOptions, Task> callBackFunc)
		{
			_options                = new LoggingSystemServiceOptions();
			_optionsCallBack        = callBackFunc;
			_logFile                = EmptyLogFile.Instance;
			_default_detailProvider = new DefaultErrorDetailProvider();
		}

		public override async Task InitializeAsync(IContext context)
		{
			await base.InitializeAsync(context);

			// Load the options
			await _optionsCallBack(_options);

			// Set the file name mode
			UseLongName = _options.UseLongName;

			// Create a log file
			if (context?.Settings?.EnableLogging ?? true) {
				_logStream = context?.GetFileSystem()?.OpenLogFile(CreateFileName());
				if (_logStream != null) {
					switch (_options.FileType) {
					case LogFileType.Custom:
						_logFile = _options.FileFactory(_logStream);
						break;
					case LogFileType.Serialized:
						_logFile = LogFile.CreateSerializedFile(_logStream);
						break;
					case LogFileType.Text:
						_logFile = LogFile.CreateTextFile(_logStream);
						break;
					case LogFileType.Xml:
						_logFile = LogFile.CreateXmlFile(_logStream);
						break;
					}
				}
			}

			// Set the default log file to the context
			if (context is InitFinalContext initContext && initContext.IsInitializationPhase()) {
				initContext.LogFile = _logFile;
			}

			// Create the system logger
			_sysLogger = new SystemLogger(_logFile);
			_sysLogger.Info("The logging system service is initialized successfully.");

			// Create the injection logger
			InjectionLogger = new SystemLogger(_logFile, "injection");
		}

		protected override void OnStartup(ContextEventArgs e)
		{
			base.OnStartup(e);

			_sysLogger?.Info("The application is starting up...");
			if (_options.CheckServiceState && _logFile != EmptyLogFile.Instance) {
				_sysLogger?.Info("Checking system services status...");
				{
					var args = e.Context.GetHostRunner().Arguments;
					_sysLogger?.Info("The command-line arguments count: " + args.Count);
					for (int i = 0; i < args.Count; ++i) {
						_sysLogger?.Debug($"args[{i}] = {args[i]}");
					}
				}
				if (e.Context.GetFileSystem() is FileSystemService) {
					_sysLogger?.Info("The file system service is initialized successfully.");
					if (e.Context.IsMultipleBoot().HasValue) {
						if (e.Context.IsMultipleBoot()!.Value) {
							_sysLogger?.Warn("Is multiple boot? true");
						} else {
							_sysLogger?.Info("Is multiple boot? false");
						}
					} else {
						_sysLogger?.Warn("Is multiple boot? null");
					}
					_sysLogger?.Info($"Data directory: {e.Context.Paths?.DataRoot}");
				} else {
					_sysLogger?.Warn($"The file system service is not initialized or not an {nameof(ExapisSOP)} object.");
				}
				if (e.Context.GetSettingsSystem() is SettingsSystemService) {
					_sysLogger?.Info("The settings system service is initialized successfully.");
					if (e.Context.IsFirstBoot().HasValue) {
						_sysLogger?.Info($"Is first boot? {e.Context.IsFirstBoot()!.Value}");
					} else {
						_sysLogger?.Warn("Is first boot? null");
					}
					_sysLogger?.Info ($"{nameof(e.Context.Settings.OutputReadableXML)} = {e.Context.Settings?.OutputReadableXML}");
					_sysLogger?.Info ($"{nameof(e.Context.Settings.Locale)}            = {e.Context.Settings?.Locale}");
					_sysLogger?.Debug($"{nameof(e.Context.Settings.EnableLogging)}     = {e.Context.Settings?.EnableLogging}");
				} else {
					_sysLogger?.Warn($"The settings system service is not initialized or not an {nameof(ExapisSOP)} object.");
				}
				if (e.Context.GetCommandLine() is CommandLineService) {
					_sysLogger?.Info("The command-line service is initialized successfully.");
					if (e.Context.Arguments == null) {
						_sysLogger?.Warn("The arguments are null.");
					} else {
						var args = e.Context.Arguments;
						if (args.Length == 0) {
							_sysLogger?.Info($"Passed no argument.");
						} else {
							if (args.Length == 1) {
								_sysLogger?.Info($"Passed an argument below:");
							} else {
								_sysLogger?.Info($"Passed {args.Length} arguments below:");
							}
							for (int i = 0; i < args.Length; ++i) {
								_sysLogger?.Debug($"The argument [{i}] = {args[i].Name}");
								for (int j = 0; j < args[i].Options.Length; ++j) {
									_sysLogger?.Debug($"The argument [{i}].[{j}] = {args[i].Options[j].Name}");
									for (int k = 0; k < args[i].Options[j].Values.Length; ++k) {
										_sysLogger?.LongMessage(
											LogLevel.Debug,
											$"The argument [{i}].[{j}].[{k}] = {args[i].Options[j].Values[k].Source}",
											args[i].Options[j].Values[k].Text
										);
									}
								}
							}
						}
					}
				} else {
					_sysLogger?.Info($"The command-line service is not initialized or not an {nameof(ExapisSOP)} object.");
				}
				_sysLogger?.Info("Checked all system services");
			}
			_sysLogger?.Info("The application is started up");
		}

		protected override void OnUpdate(ContextEventArgs e)
		{
			base.OnUpdate(e);
			if (_options.LogOnUpdate) {
				_sysLogger?.Trace($"The host runner called {nameof(this.OnUpdate)} method.");
			}
		}

		protected override void OnShutdown(ContextEventArgs e)
		{
			base.OnShutdown(e);
			_sysLogger?.Info("The application is shutting down...");
		}

		protected override void OnTerminate(TerminationEventArgs e)
		{
			base.OnTerminate(e);
			_sysLogger?.Info("The termination exception was threwn. Reason: " + e.Reason);
			_sysLogger?.Exception(e.Exception);
		}

		protected override void OnUnhandledError(UnhandledErrorEventArgs e)
		{
			base.OnUnhandledError(e);
			_sysLogger?.UnhandledException(e.Exception);
			this.SaveErrorReport(e.Context, e.Exception);
		}

		public override async Task FinalizeAsync(IContext context)
		{
			_sysLogger?.Info("The application and the logging system service is in finalization phase...");

			await base.FinalizeAsync(context);

			if (_logFile is IDisposable disposable) {
				disposable.Dispose();
			}
			if (_logStream != null) {
				await (context?.GetFileSystem()?.CloseStreamAsync(_logStream) ?? Task.CompletedTask);
			}
		}

		public PathString? SaveErrorReport(IContext context, Exception exception, params ICustomErrorDetailProvider[] detailProviders)
		{
			if (context == null) {
				throw new ArgumentNullException(nameof(context));
			}
			if (exception == null) {
				throw new ArgumentNullException(nameof(exception));
			}
			if (detailProviders == null) {
				detailProviders = new ICustomErrorDetailProvider[] { _default_detailProvider };
			}
			if (!detailProviders.Any((provider) => provider.GetType() == typeof(DefaultErrorDetailProvider))) {
				var tmp = new ICustomErrorDetailProvider[detailProviders.Length + 1];
				Array.Copy(detailProviders, tmp, detailProviders.Length);
				tmp[detailProviders.Length] = _default_detailProvider;
				detailProviders = tmp;
			}
			var fss = context.GetFileSystem();
			if (fss == null) {
				return null;
			} else {
				FileStream? fs = null;
				try {
					fs = fss.OpenLogFile(CreateFileName(nameof(exception)));
					ErrorReportBuilder.Create(exception, detailProviders).Save(fs);
					_sysLogger?.Info($"Saved the error report to \"{fs.Name}\" about following exception:");
					_sysLogger?.Exception(exception);
					return new PathString(fs.Name);
				} finally {
					if (fs != null) {
						fss.CloseStream(fs);
					}
				}
			}
		}

		#endregion

		#region 静的

		internal static bool          UseLongName     { get; private set; }
		internal static SystemLogger? InjectionLogger { get; private set; }

		internal static string CreateFileName(string? tag = null)
		{
			return CreateFileName(DateTime.Now, Process.GetCurrentProcess(), tag);
		}

		internal static string CreateFileName(DateTime dt, string? tag = null)
		{
			return CreateFileName(dt, Process.GetCurrentProcess(), tag);
		}

		internal static string CreateFileName(Process proc, string? tag = null)
		{
			return CreateFileName(DateTime.Now, proc, tag);
		}

		internal static string CreateFileName(DateTime dt, Process proc, string? tag = null)
		{
			if (UseLongName) {
				if (string.IsNullOrEmpty(tag)) {
					return $"{dt:yyyy-MM-dd_HH-mm-ss+fffffff}.[{proc.Id}].log";
				} else {
					return $"{dt:yyyy-MM-dd_HH-mm-ss+fffffff}.[{proc.Id}].{tag}.log";
				}
			} else {
				if (string.IsNullOrEmpty(tag)) {
					return $"{dt:yyyyMMddHHmmssfffffff}.[{proc.Id}].log";
				} else {
					return $"{dt:yyyyMMddHHmmssfffffff}.[{proc.Id}].{tag}.log";
				}
			}
		}

		#endregion
	}
}
