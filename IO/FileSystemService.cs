/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ExapisSOP.Core;

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
			var opt = new FileSystemServiceOptions();
			await _options(opt);
			_paths = new Paths(opt.DataPath);
			if (context is InitFinalContext initContext && initContext.IsInitializationPhase()) {
				initContext.Paths = _paths;
			}
			if (opt.CreateLockFile) {
				_lockfile = _paths.DataRoot + ".lock";
				if (_lockfile.Exists) {
					_abort = true;
				} else {
					lock (_streams) {
						_streams.Add(new FileStream(_lockfile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None));
					}
				}
			}
		}

		protected override void OnStartup(ContextEventArgs e)
		{
			base.OnStartup(e);
			if (_abort) {
				throw new TerminationException();
			}
		}

		public FileStream OpenDataFile(string name)
		{
			try {
				var fs = new FileStream(_paths.DataRoot + name, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
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
				if (_streams.Contains(s)) {
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
				if (_streams.Contains(s)) {
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
