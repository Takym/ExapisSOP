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

namespace ExapisSOP.IO
{
	internal sealed class FileSystemService : IFileSystemService
	{
		private readonly List<Stream>                         _streams;
		private readonly Func<FileSystemServiceOptions, Task> _options;

		internal FileSystemService(Func<FileSystemServiceOptions, Task> callBackFunc)
		{
			_streams = new List<Stream>();
			_options = callBackFunc;
		}

		public async Task InitializeAsync(IContext context)
		{
			var opt = new FileSystemServiceOptions();
			await _options(opt);
		}

		public async Task FinalizeAsync(IContext context)
		{

		}
	}
}
