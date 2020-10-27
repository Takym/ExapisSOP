/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections;
using System.Collections.Generic;

namespace ExapisSOP.IO
{
	partial class PathString
	{
		private sealed class FileSystemEntryList : IEnumerable<PathString>
		{
			private readonly IEnumerable<string> _entries;

			internal FileSystemEntryList(IEnumerable<string> entries)
			{
				_entries = entries;
			}

			public IEnumerator<PathString> GetEnumerator()
			{
				return new FileSystemEntryEnumerator(_entries.GetEnumerator());
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			private sealed class FileSystemEntryEnumerator : IEnumerator<PathString>
			{
				private readonly IEnumerator<string> _entries;
				public           bool                IsDisposed { get; private set; }

				public PathString Current
				{
					get
					{
						if (this.IsDisposed) {
							throw new ObjectDisposedException(nameof(FileSystemEntryEnumerator));
						}
						return new PathString(_entries.Current);
					}
				}

				object? IEnumerator.Current => this.Current;

				internal FileSystemEntryEnumerator(IEnumerator<string> entries)
				{
					_entries = entries;
				}

				~FileSystemEntryEnumerator()
				{
					this.Dispose(false);
				}

				public bool MoveNext()
				{
					if (this.IsDisposed) {
						throw new ObjectDisposedException(nameof(FileSystemEntryEnumerator));
					}
					return _entries.MoveNext();
				}

				public void Reset()
				{
					if (this.IsDisposed) {
						throw new ObjectDisposedException(nameof(FileSystemEntryEnumerator));
					}
					_entries.Reset();
				}

				public void Dispose()
				{
					this.Dispose(true);
					GC.SuppressFinalize(this);
				}

				private void Dispose(bool disposing)
				{
					if (!this.IsDisposed) {
						if (disposing) {
							_entries.Dispose();
						}
						this.IsDisposed = true;
					}
				}
			}
		}
	}
}
