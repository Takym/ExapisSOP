/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Text;
using ExapisSOP.Properties;

namespace ExapisSOP.IO.Logging
{
	internal class BinaryLogFile : LogFile
	{
		private const    ulong        _sig     = 0x666C627800FF7F80;
		private const    ushort       _sig_ixt = 0x7869;
		private const    uint         _ver     = 0x00000000;
		private readonly Stream       _stream;
		private readonly BinaryReader _br;
		private readonly BinaryWriter _bw;
		private          uint         _pos_ixt;

		public override ulong Count => 0;

		internal BinaryLogFile(Stream stream)
		{
			if (stream == null) {
				throw new ArgumentNullException(nameof(stream));
			}
			_stream  = Stream.Synchronized(stream);
			_br      = new BinaryReader(stream, Encoding.UTF8);
			_bw      = new BinaryWriter(stream, Encoding.UTF8);
			this.Init();
		}

		private void Init()
		{
			_stream.Position = 0;
			if (_stream.Length == 0) {
				_bw.Write(_sig);
				_bw.Write(_ver);
				_bw.Write(unchecked((uint)(_stream.Position)) + 4);
			} else if (_stream.Length >= 16) {
				ulong fsig = _br.ReadUInt64();
				uint  fver = _br.ReadUInt32();
				if (fsig != _sig || fver != _ver) {
					throw new InvalidDataException(Resources.BinaryLogFile_InvalidDataException);
				}
			} else {
				throw new InvalidDataException(Resources.BinaryLogFile_InvalidDataException);
			}
			this.ResetIndexTable();
		}

		private void NextIndexTable()
		{
			if (_pos_ixt == 0) {
				_stream.Position = 12;
				_pos_ixt = _br.ReadUInt32();
			} else {
				_stream.Position = _pos_ixt + 2;
				uint offset = _br.ReadUInt32() * 4;
				_stream.Position += offset;
				_pos_ixt = _br.ReadUInt32();
			}
			_stream.Position = _pos_ixt;
			if (_br.ReadUInt16() != _sig_ixt) {
				throw new InvalidDataException(Resources.BinaryLogFile_InvalidDataException);
			}
		}

		private void ResetIndexTable()
		{
			_pos_ixt = 0;
			this.NextIndexTable();
		}

		protected override void AddLogCore(LogData data)
		{
		}

		protected override LogData GetLogCore(ulong index)
		{
			return null!;
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.IsDisposed) {
				if (disposing) {
					_stream.Flush();
					_stream.Dispose();
				}
				base.Dispose(disposing);
			}
		}
	}
}
