/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System.IO;
using ExapisSOP.Properties;

namespace ExapisSOP.IO.Logging
{
	partial class BinaryLogFile
	{
		// _a: アドレス, _v: 値
		private const long   _a_hdr            = 0x00000000;
		//private const long   _a_hdr_sig        = 0x00000000;
		//private const long   _a_hdr_ver        = 0x00000008;
		private const long   _a_hdr_ixt        = 0x0000000C;
		private const ulong  _v_hdr_sig        = 0x464C42587F0080EF; // EF 80 00 7F "XBLF"
		private const uint   _v_hdr_ver        = 0x00000000;         // 0
		private const ushort _v_ixt_sig        = 0x7869;             // "ix"
		private const uint   _default_ixt_size = 0x00000100;
		private       long   _pos_ixt;
		private       uint   _size_ixt;
		private       uint   _count_ixt;

		private void Init()
		{
			if (_stream.Length == 0) {
				this.CreateHeader();
			} else if (_stream.Length >= 16) {
				this.CheckHeader();
			} else {
				throw new InvalidDataException(Resources.BinaryLogFile_InvalidDataException);
			}
			this.ResetIndexTable();
		}

		private void CreateHeader()
		{
			_stream.Position = _a_hdr;
			_bw.Write(_v_hdr_sig);
			_bw.Write(_v_hdr_ver);
			_bw.Write((long)(0));
			this.CreateIndexTable();
		}

		private void CheckHeader()
		{
			_stream.Position = _a_hdr;
			ulong fsig = _br.ReadUInt64();
			uint  fver = _br.ReadUInt32();
			if (fsig != _v_hdr_sig || fver != _v_hdr_ver) {
				throw new InvalidDataException(Resources.BinaryLogFile_InvalidDataException);
			}
		}

		private void ResetIndexTable()
		{
			_pos_ixt = 0;
		}

		private void NextIndexTable()
		{
			if (_pos_ixt == 0) {
				_stream.Position = _a_hdr_ixt;
				_pos_ixt         = _br.ReadInt64();
			} else {
				_stream.Position = _pos_ixt + 2;
				uint offset = _br.ReadUInt32() * 8 + 4;
				_stream.Position += offset;
				_pos_ixt = _br.ReadInt64();
			}
			_stream.Position = _pos_ixt;
			if (_br.ReadUInt16() != _v_ixt_sig) {
				throw new InvalidDataException(Resources.BinaryLogFile_InvalidDataException);
			}
			_size_ixt  = _br.ReadUInt32();
			_count_ixt = _br.ReadUInt32();
		}

		private void CreateIndexTable(uint size = _default_ixt_size)
		{
			if (_pos_ixt == 0) {
				_stream.Position = _a_hdr_ixt;
				_pos_ixt         = _br.ReadInt64();
			}
			while (_pos_ixt != 0) {
				this.NextIndexTable();
			}
			_pos_ixt   = _stream.Length;
			_size_ixt  = size;
			_count_ixt = 0;
			_stream.Position -= 8;
			_bw.Write(_pos_ixt);
			_stream.Position = _pos_ixt;
			_bw.Write(_v_ixt_sig);
			_bw.Write(_size_ixt);
			_bw.Write(_count_ixt);
			for (uint i = 0; i < _size_ixt; ++i) {
				_bw.Write((long)(0));
			}
			_bw.Write((long)(0));
		}

		private ulong CountLogs()
		{
			ulong result = 0;
			this.ResetIndexTable();
			do {
				this.NextIndexTable();
				if (_pos_ixt == 0) {
					break;
				} else {
					result += _count_ixt;
				}
			} while (true);
			return result;
		}

		// private interface IXblfFormat { }
	}
}
