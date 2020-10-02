/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ExapisSOP.Core;
using ExapisSOP.Properties;

namespace ExapisSOP.IO
{
	internal sealed class FileSystemService : AppWorker, IFileSystemService
	{
		private readonly List<Stream>                         _streams;
		private readonly Func<FileSystemServiceOptions, Task> _options;
		private          Paths                                _paths;
		private          PathString?                          _lockfile;
		private          bool                                 _abort;

		internal FileSystemService(Func<FileSystemServiceOptions, Task> callBackFunc)
		{
			_streams = new List<Stream>();
			_options = callBackFunc;
			_paths   = null!;
		}

		public override async Task InitializeAsync(IContext context)
		{
			await base.InitializeAsync(context);

			// Load the options
			var opt = new FileSystemServiceOptions();
			await _options(opt);
			_paths = new Paths(opt.DataPath);

			// Set the path list to the context
			if (context is InitFinalContext initContext && initContext.IsInitializationPhase()) {
				initContext.Paths = _paths;
			}

			// Create and check the lock file
			if (opt.CreateLockFile) {
				_lockfile = _paths.DataRoot + ".lock";
				if (_lockfile.Exists) {
					_abort = true;
					_paths.ExistsLockFile = true;
				} else {
					lock (_streams) {
						_streams.Add(new FileStream(_lockfile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None));
					}
					_paths.ExistsLockFile = false;
				}
			}
		}

		protected override void OnStartup(ContextEventArgs e)
		{
			base.OnStartup(e);
			if (_abort) {
				throw new TerminationException(
					string.Format(Resources.FileSystemService_TerminationException, _lockfile),
					TerminationReason.ProcessLocked,
					CancellationToken.None);
			}
		}

		public  FileStream OpenDataFile   (string name) => this.OpenFileStreamPrivate(_paths.DataRoot  + name);
		public  FileStream OpenLogFile    (string name) => this.OpenFileStreamPrivate(_paths.Logs      + name);
		public  FileStream OpenTempFile   (string name) => this.OpenFileStreamPrivate(_paths.Temporary + name);
		public  FileStream OpenSettingFile(string name) => this.OpenFileStreamPrivate(_paths.Settings  + name);
		private FileStream OpenFileStreamPrivate(PathString path)
		{
			try {
				var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
				lock (_streams) {
					_streams.Add(fs);
				}
				return fs;
			} catch (Exception e) {
				throw new IOException(e.Message, e);
			}
		}

		public MemoryStream OpenMemory(params byte[] bin)
		{
			try {
				var ms = new MemoryStream();
				ms.Write(bin, 0, bin.Length);
				ms.Seek(0, SeekOrigin.Begin);
				lock (_streams) {
					_streams.Add(ms);
				}
				return ms;
			} catch (Exception e) {
				throw new IOException(e.Message, e);
			}
		}

		public CachedStream OpenCachedMemory()
		{
			try {
				var cs = new CachedStream(_paths.Caches + Path.GetRandomFileName(), new byte[0]);
				lock (_streams) {
					_streams.Add(cs);
				}
				return cs;
			} catch (Exception e) {
				throw new IOException(e.Message, e);
			}
		}

		public BufferedStream AddBufferingLayer(Stream s)
		{
			try {
				var bs = new BufferedStream(s);
				lock (_streams) {
					_streams.Add(bs);
				}
				return bs;
			} catch (Exception e) {
				throw new IOException(e.Message, e);
			}
		}

		public bool CloseStream(Stream s)
		{
			try {
				bool contains;
				lock (_streams) {
					contains = _streams.Contains(s);
				}
				if (contains) {
					s.Close();
					lock (_streams) {
						return _streams.Remove(s);
					}
				} else {
					return false;
				}
			} catch (Exception e) {
				throw new IOException(e.Message, e);
			}
		}

		public async Task<bool> CloseStreamAsync(Stream s)
		{
			try {
#if NET48
				return await Task.FromResult(this.CloseStream(s));
#elif NETCOREAPP3_1
				bool contains;
				lock (_streams) {
					contains = _streams.Contains(s);
				}
				if (contains) {
					await s.DisposeAsync();
					lock (_streams) {
						return _streams.Remove(s);
					}
				} else {
					return false;
				}
#endif
			} catch (Exception e) {
				throw new IOException(e.Message, e);
			}
		}

		public override async Task FinalizeAsync(IContext context)
		{
			await base.FinalizeAsync(context);

#if NETCOREAPP3_1
			var tasks = new List<Task>();
#endif
			lock (_streams) {
				for (int i = 0; i < _streams.Count; ++i) {
#if NET48
					_streams[i].Close();
#elif NETCOREAPP3_1
					tasks.Add(_streams[i].DisposeAsync().AsTask());
#endif
				}
			}
#if NETCOREAPP3_1
			await Task.WhenAll(tasks);
#endif

			if (!(_lockfile is null || _abort)) {
				File.Delete(_lockfile);
			}
		}
	}
}
