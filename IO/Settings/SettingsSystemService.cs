/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ExapisSOP.Core;
using ExapisSOP.Properties;

namespace ExapisSOP.IO.Settings
{
	internal sealed class SettingsSystemService : AppWorker, ISettingsSystemService
	{
		private readonly static Encoding                                 _enc = Encoding.Unicode;
		private readonly        Func<SettingsSystemServiceOptions, Task> _optionsCallBack;
		private                 SettingsSystemServiceOptions             _options;
		private                 IFileSystemService?                      _fss;
		private                 FileStream?                              _ver;
		private                 StreamReader?                            _ver_r;
		private                 StreamWriter?                            _ver_w;
		private                 FileStream?                              _env;
		private                 XmlReader?                               _env_r;
		private                 XmlWriter?                               _env_w;
		private                 FileStream?                              _envr;
		private                 XmlWriter?                               _envr_w;
		private                 EnvironmentSettings?                     _settings;
		private                 XmlSerializer                            _xs;
		private                 XmlSerializer                            _xsr;
		private                 bool                                     _abort;

		internal SettingsSystemService(Func<SettingsSystemServiceOptions, Task> callBackFunc)
		{
			_optionsCallBack = callBackFunc;
			_options         = new SettingsSystemServiceOptions();
			_xs              = new XmlSerializer(typeof(EnvironmentSettings));
			_xsr             = new XmlSerializer(typeof(EnvironmentSettings), EnvironmentSettings.LoadSchema().TargetNamespace);
		}

		public override async Task InitializeAsync(IContext context)
		{
			await base.InitializeAsync(context);

			// Load the options
			await _optionsCallBack(_options);

			// Load the file system service
			_fss = context.GetFileSystem();
			if (_fss == null) {
				_abort = true;
				return;
			}

			// Load the settings
			await this.Reload(context);

			// Apply the language configuration
			var culture = _settings?.GetCulture() ?? CultureInfo.InstalledUICulture;
			CultureInfo.DefaultThreadCurrentCulture   = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;
			CultureInfo.CurrentCulture                = culture;
			CultureInfo.CurrentUICulture              = culture;
			culture.ClearCachedData();
		}

		protected override void OnStartup(ContextEventArgs e)
		{
			base.OnStartup(e);
			if (_abort) {
				throw new TerminationException(
					Resources.SettingsSystemService_TerminationException1,
					TerminationReason.NoCompatible,
					CancellationToken.None);
			}
		}

		protected override void OnUpdate(ContextEventArgs e)
		{
			base.OnUpdate(e);
			if (_abort) {
				throw new TerminationException(
					Resources.SettingsSystemService_TerminationException2,
					TerminationReason.NoCompatible,
					CancellationToken.None);
			}
		}

		public override async Task FinalizeAsync(IContext context)
		{
			await base.FinalizeAsync(context);
			await this.Save();
			await this.DisposeStreams();
		}

		public async Task Reload(IContext context)
		{
			bool firstBoot = false;
			this.EnsureStreams();

			// Get the saved version
			string? libversion  = await (_ver_r?.ReadLineAsync() ?? Task.FromResult<string?>("?.?.?.?"));
			string? libcodename = await (_ver_r?.ReadLineAsync() ?? Task.FromResult<string?>("no lib ver"));
			string? appversion  = await (_ver_r?.ReadLineAsync() ?? Task.FromResult<string?>("?.?.?.?"));
			string? appcodename = await (_ver_r?.ReadLineAsync() ?? Task.FromResult<string?>("no app ver"));
			if (libversion == null) {
				firstBoot = true;
				await this.WriteSavedVersion();
			} else if (!_options.HasCompatibleWith(appversion, appcodename) ||
				libversion  != VersionInfo.VersionString ||
				libcodename != VersionInfo.CodeName) {
				_abort = true;
				return;
			}

			// Load or create a settings
			if (firstBoot) {
				_settings = _options.CreateNewSettings();
				_settings.FirstBoot = firstBoot;
			} else {
				_settings = _xs.Deserialize(_env_r) as EnvironmentSettings
					?? _options.CreateNewSettings();
			}

			// Set the settings to the context
			if (context is InitFinalContext initContext && initContext.IsInitializationPhase()) {
				initContext.Settings = _settings;
			} else if (context.Settings != null) {
				context.Settings.CopyFrom(_settings);
				_settings = context.Settings;
			}
		}

		public async Task Save()
		{
			if (!_abort) {
				this.EnsureStreams();

				// Save the current version
				_ver?.SetLength(0);
				await this.WriteSavedVersion();

				// Save the current settings
				_env?.SetLength(0);
				var settings = _settings ?? _options.CreateNewSettings();
				_xs.Serialize(_env_w, settings);
				if (settings.OutputReadableXML) {
					_envr?.SetLength(0);
					_xsr.Serialize(_envr_w, settings);
					byte[] buf = XmlSettings.ReadableEncoding.GetBytes(Environment.NewLine);
					_envr?.Write(buf, 0, buf.Length);
				}
			}
		}

		private void EnsureStreams()
		{
			bool saveReadable = _settings?.OutputReadableXML ?? false;
			if (_ver == null) {
				_ver = _fss?.OpenSettingFile("last_saved_version.txt");
			}
			if (_env == null) {
				_env = _fss?.OpenSettingFile(EnvironmentSettings.RootElementName + ".xml");
			}
			if (_envr == null && saveReadable) {
				_envr = _fss?.OpenSettingFile(EnvironmentSettings.RootElementName + ".readable.xml");
			}
			_ver ?.Seek(0, SeekOrigin.Begin);
			_env ?.Seek(0, SeekOrigin.Begin);
			_envr?.Seek(0, SeekOrigin.Begin);
			if (_ver != null) {
				_ver_r ??= new StreamReader(_ver, _enc, true);
				_ver_w ??= new StreamWriter(_ver, _ver_r.CurrentEncoding);
			}
			if (_env != null) {
				_env_r ??= XmlReader.Create(new StreamReader(_env, XmlSettings.Encoding, true), XmlSettings.Reader);
				_env_w ??= XmlWriter.Create(new StreamWriter(_env, XmlSettings.Encoding),       XmlSettings.Writer);
			}
			if (_envr != null && saveReadable) {
				_envr_w ??= XmlWriter.Create(new StreamWriter(_envr, XmlSettings.ReadableEncoding), XmlSettings.ReadableWriter);
			}
		}

		private async Task WriteSavedVersion()
		{
			(string appversion, string appcodename) = _options.GetCurrentVersion();
			await (_ver_w?.WriteLineAsync(VersionInfo.VersionString) ?? Task.CompletedTask);
			await (_ver_w?.WriteLineAsync(VersionInfo.CodeName     ) ?? Task.CompletedTask);
			await (_ver_w?.WriteLineAsync(appversion               ) ?? Task.CompletedTask);
			await (_ver_w?.WriteLineAsync(appcodename              ) ?? Task.CompletedTask);
			await (_ver_w?.FlushAsync()                              ?? Task.CompletedTask);
		}

		private async Task DisposeStreams()
		{
			if (_ver != null) {
				await (_fss?.CloseStreamAsync(_ver) ?? Task.FromResult(false));
			}
			if (_env != null) {
				await (_fss?.CloseStreamAsync(_env) ?? Task.FromResult(false));
			}
		}
	}
}
