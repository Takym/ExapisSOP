/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Threading.Tasks;
using ExapisSOP.Core;

namespace ExapisSOP.IO.Settings
{
	internal sealed class SettingsSystemService : AppWorker, ISettingsSystemService
	{
		private readonly Func<SettingsSystemServiceOptions, Task> _optionsCallBack;
		private          SettingsSystemServiceOptions             _options;
		private          IFileSystemService?                      _fss;
		private          FileStream?                              _ver;
		private          StreamReader?                            _ver_r;
		private          StreamWriter?                            _ver_w;
		private          FileStream?                              _env;
		private          bool                                     _abort;

		internal SettingsSystemService(Func<SettingsSystemServiceOptions, Task> callBackFunc)
		{
			_optionsCallBack = callBackFunc;
			_options         = new SettingsSystemServiceOptions();
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
			this.EnsureStreams();

			// Saved version
			string? libversion  = await (_ver_r?.ReadLineAsync() ?? Task.FromResult<string?>("?.?.?.?"));
			string? libcodename = await (_ver_r?.ReadLineAsync() ?? Task.FromResult<string?>("no lib ver"));
			string? appversion  = await (_ver_r?.ReadLineAsync() ?? Task.FromResult<string?>("?.?.?.?"));
			string? appcodename = await (_ver_r?.ReadLineAsync() ?? Task.FromResult<string?>("no app ver"));
			if (libversion == null) {
				await this.WriteSavedVersion();
			} else if (!_options.HasCompatibleWith(appversion, appcodename) ||
				libversion  != VersionInfo.VersionString ||
				libcodename != VersionInfo.CodeName) {
				_abort = true;
				return;
			}
		}

		private void EnsureStreams()
		{
#if false
			if (_ver == null) {
				_ver = _fss!.OpenSettingFile("last_saved_version.txt");
				_ver_r = new StreamReader(_ver, true);
				_ver_w = new StreamWriter(_ver, _ver_r.CurrentEncoding);
			}
			if (_env == null) {
				_env = _fss!.OpenSettingFile("envconfig.xml");
			}
			_ver.Seek(0, SeekOrigin.Begin);
			_env.Seek(0, SeekOrigin.Begin);
#else
			if (_ver == null) {
				_ver = _fss?.OpenSettingFile("last_saved_version.txt");
				if (_ver != null) {
					_ver_r = new StreamReader(_ver, true);
					_ver_w = new StreamWriter(_ver, _ver_r.CurrentEncoding);
				}
			}
			if (_env == null) {
				_env = _fss?.OpenSettingFile("envconfig.xml");
			}
			_ver?.Seek(0, SeekOrigin.Begin);
			_env?.Seek(0, SeekOrigin.Begin);
#endif
		}

		protected override void OnStartup(ContextEventArgs e)
		{
			base.OnStartup(e);
			if (_abort) {
				throw new TerminationException();
			}
		}

		public override async Task FinalizeAsync(IContext context)
		{
			await base.FinalizeAsync(context);
			this.EnsureStreams();

			// Saved version
			if (!_abort) {
				_ver?.SetLength(0);
				await this.WriteSavedVersion();
			}

			await this.DisposeStreams();
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
#if false
			await _fss!.CloseStreamAsync(_ver!);
			await _fss!.CloseStreamAsync(_env!);
#else
			if (_ver != null) {
				await (_fss?.CloseStreamAsync(_ver) ?? Task.FromResult(false));
			}
			if (_env != null) {
				await (_fss?.CloseStreamAsync(_env) ?? Task.FromResult(false));
			}
#endif
		}
	}
}
