/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExapisSOP.Resources.Utils;
using ExapisSOP.Text;

namespace ExapisSOP.Binary
{
	/// <summary>
	///  オブジェクトとバイナリデータの変換を行います。
	/// </summary>
	public class DataValue
	{
		/// <summary>
		///  データ値を取得または設定します。
		/// </summary>
		public object? Value { get; set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.Binary.DataValue"/>'の新しいインスタンスを生成します。
		/// </summary>
		public DataValue() { }

		/// <summary>
		///  型'<see cref="ExapisSOP.Binary.DataValue"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="obj">新しいデータ値に設定するオブジェクトです。</param>
		public DataValue(object obj)
		{
			this.Value = obj;
		}

		/// <summary>
		///  データ値を指定されたリーダーから読み取ります。
		/// </summary>
		/// <param name="br">データ値が格納されたリーダーです。</param>
		/// <exception cref="System.NotSupportedException" />
		public virtual void Read(BinaryReader br)
		{
			byte type;
			do {
				type = br.ReadByte();
			} while (type == 0);
			this.Value = type switch
			{
				0xFF => null,
				0x01 => br.ReadChar(),
				0x02 => br.ReadString(),
				0x03 => br.ReadBoolean(),
				0x04 => br.ReadByte(),
				0x05 => br.ReadUInt16(),
				0x06 => br.ReadUInt32(),
				0x07 => br.ReadUInt64(),
				0x08 => br.ReadSByte(),
				0x09 => br.ReadInt16(),
				0x0A => br.ReadInt32(),
				0x0B => br.ReadInt64(),
				0x0C => br.ReadSingle(),
				0x0D => br.ReadDouble(),
				0x0E => br.ReadDecimal(),
				0x0F => br.ReadBytes(br.ReadInt32()),
				0x10 => this.ReadList(br),
				0x11 => this.ReadDict(br),
				0x12 => new SimpleString(br.ReadBytes(br.ReadInt32())),
				0x13 => new Guid(br.ReadBytes(16)),
				0x14 => new DateTime(br.ReadInt64()),
				_ => throw new NotSupportedException(string.Format(StringRes.DataValue_NotSupportedException, type))
			};
		}

		private object ReadList(BinaryReader br)
		{
			int count  = br.ReadInt32();
			var result = new List<DataValue>(count);
			for (int i = 0; i < count; ++i) {
				var value = new DataValue();
				value.Read(br);
				result.Add(value);
			}
			return result;
		}

		private object ReadDict(BinaryReader br)
		{
			int count  = br.ReadInt32();
			var result = new Dictionary<string, DataValue>(count);
			for (int i = 0; i < count; ++i) {
				var key   = br.ReadString();
				var value = new DataValue();
				value.Read(br);
				result.Add(key, value);
			}
			return result;
		}

		/// <summary>
		///  データ値を指定されたリーダーへ書き込みます。
		/// </summary>
		/// <param name="bw">データ値の格納先のライターです。</param>
		/// <exception cref="System.NotSupportedException" />
		public virtual void Write(BinaryWriter bw)
		{
			switch (this.Value) {
			case null:
				bw.Write((byte)(0xFF));
				break;
			case char c:
				bw.Write((byte)(0x01));
				bw.Write(c);
				break;
			case string s:
				bw.Write((byte)(0x02));
				bw.Write(s);
				break;
			case bool b:
				bw.Write((byte)(0x03));
				bw.Write(b);
				break;
			case byte u8:
				bw.Write((byte)(0x04));
				bw.Write(u8);
				break;
			case ushort u16:
				bw.Write((byte)(0x05));
				bw.Write(u16);
				break;
			case uint u32:
				bw.Write((byte)(0x06));
				bw.Write(u32);
				break;
			case ulong u64:
				bw.Write((byte)(0x07));
				bw.Write(u64);
				break;
			case sbyte s8:
				bw.Write((byte)(0x08));
				bw.Write(s8);
				break;
			case short s16:
				bw.Write((byte)(0x09));
				bw.Write(s16);
				break;
			case int s32:
				bw.Write((byte)(0x0A));
				bw.Write(s32);
				break;
			case long s64:
				bw.Write((byte)(0x0B));
				bw.Write(s64);
				break;
			case float sgl:
				bw.Write((byte)(0x0C));
				bw.Write(sgl);
				break;
			case double dbl:
				bw.Write((byte)(0x0D));
				bw.Write(dbl);
				break;
			case decimal dec:
				bw.Write((byte)(0x0E));
				bw.Write(dec);
				break;
			case byte[] binary:
				bw.Write((byte)(0x0F));
				bw.Write(binary.Length);
				bw.Write(binary);
				break;
			case IList<DataValue> list:
				bw.Write((byte)(0x10));
				bw.Write(list.Count);
				for (int i = 0; i < list.Count; ++i) {
					list[i].Write(bw);
				}
				break;
			case IDictionary<string, DataValue> dict:
				bw.Write((byte)(0x11));
				bw.Write(dict.Count);
				foreach (var pair in dict) {
					bw.Write(pair.Key);
					pair.Value.Write(bw);
				}
				break;
			case SimpleString ys:
				bw.Write((byte)(0x12));
				bw.Write(ys.Value.Length);
				bw.Write(ys.Value);
				break;
			case Guid guid:
				bw.Write((byte)(0x13));
				bw.Write(guid.ToByteArray());
				break;
			case DateTime dt:
				bw.Write((byte)(0x14));
				bw.Write(dt.Ticks);
				break;
			case DataValue value:
				value.Write(bw);
				break;
			default:
				throw new NotSupportedException(string.Format(StringRes.DataValue_NotSupportedException, this.Value.GetType().FullName));
			}
		}

		/// <summary>
		///  このデータ値を純粋なオブジェクトへ変換します。
		/// </summary>
		/// <returns>変換後の<see cref="ExapisSOP.Binary.DataValue"/>を含まない純粋なオブジェクトです。</returns>
		public virtual object? ToObject()
		{
			if (this.Value is IList<DataValue> list) {
				var result = new List<object?>();
				for (int i = 0; i < list.Count; ++i) {
					result.Add(list[i].ToObject());
				}
				return result;
			} else if (this.Value is IDictionary<string, DataValue> dict) {
				var result = new Dictionary<string, object?>();
				foreach (var pair in dict) {
					result.Add(pair.Key, pair.Value.ToObject());
				}
				return result;
			} else if (this.Value is DataValue value) {
				return value.ToObject();
			} else if (this.Value is ICloneable cloneable) {
				return cloneable.Clone();
			} else {
				return this.Value;
			}
		}

		/// <summary>
		///  純粋なオブジェクトからデータ値へ変換します。
		/// </summary>
		/// <param name="obj">変換前の<see cref="ExapisSOP.Binary.DataValue"/>を含まない純粋なオブジェクトです。</param>
		public virtual void FromObject(object? obj)
		{
			if (obj is IList<object?> list) {
				var value = new List<DataValue>();
				for (int i = 0; i < list.Count; ++i) {
					var cval = new DataValue();
					cval.FromObject(list[i]);
					value.Add(cval);
				}
				this.Value = value;
			} else if (obj is IDictionary<string, object?> dict) {
				var value = new Dictionary<string, DataValue>();
				foreach (var pair in dict) {
					var cval = new DataValue();
					cval.FromObject(pair.Value);
					value.Add(pair.Key, cval);
				}
				this.Value = value;
			} else if (obj is ICloneable cloneable) {
				this.Value = cloneable.Clone();
			} else {
				this.Value = obj;
			}
		}

		/// <summary>
		///  このデータ値を可読な文字列へ変換します。
		/// </summary>
		/// <returns>変換後の可読なオブジェクトです。</returns>
		public override string ToString()
		{
			switch (this.Value) {
			case null:
				return "datavalue null";
			case char c:
				return $"datavalue char: \'{c}\'";
			case string s:
				return $"datavalue string: \"{s}\"";
			case bool b:
				return $"datavalue bool: {b}";
			case byte u8:
				return $"datavalue unsigned integer: 0x{u8:X02} ({u8})";
			case ushort u16:
				return $"datavalue unsigned integer: 0x{u16:X04} ({u16})";
			case uint u32:
				return $"datavalue unsigned integer: 0x{u32:X08} ({u32})";
			case ulong u64:
				return $"datavalue unsigned integer: 0x{u64:X16} ({u64})";
			case sbyte s8:
				return $"datavalue signed integer: 0x{s8:X02} ({s8})";
			case short s16:
				return $"datavalue signed integer: 0x{s16:X04} ({s16})";
			case int s32:
				return $"datavalue signed integer: 0x{s32:X08} ({s32})";
			case long s64:
				return $"datavalue signed integer: 0x{s64:X16} ({s64})";
			case float sgl:
				return $"datavalue single float: {sgl}";
			case double dbl:
				return $"datavalue double float: {dbl}";
			case decimal dec:
				return $"datavalue decimal: {dec}";
			case byte[] binary:
				var sb1 = new StringBuilder();
				sb1.Append("datavalue binary: ");
				for (int i = 0; i < binary.Length; ++i) {
					sb1.Append(binary[i].ToString("X02")).Append(" ");
				}
				return sb1.ToString().Trim();
			case IList<DataValue> list:
				var sb2 = new StringBuilder();
				sb2.Append("datavalue list: count = ").Append(list.Count).AppendLine();
				for (int i = 0; i < list.Count; ++i) {
					sb2.Append($"list value[{i}]:").AppendLine();
					string[] lines = list[i].ToString().Replace('\r', '\n').Split('\n', StringSplitOptions.RemoveEmptyEntries);
					for (int j = 0; j < lines.Length; ++j) {
						sb2.Append("    " + lines[j]).AppendLine();
					}
				}
				return sb2.ToString().Trim();
			case IDictionary<string, DataValue> dict:
				var sb3 = new StringBuilder();
				sb3.Append("datavalue dictionary: count = ").Append(dict.Count).AppendLine();
				foreach (var pair in dict) {
					sb3.Append($"dictionary value[{pair.Key}]:").AppendLine();
					string[] lines = pair.Value.ToString().Replace('\r', '\n').Split('\n', StringSplitOptions.RemoveEmptyEntries);
					for (int j = 0; j < lines.Length; ++j) {
						sb3.Append("    " + lines[j]).AppendLine();
					}
				}
				return sb3.ToString().Trim();
			case SimpleString ys:
				return $"datavalue simple string: \"{ys.ToStringWithoutNull().Replace("\r", "[[CR]]").Replace("\n", "[[LF]]")}\"";
			case Guid guid:
				return $"datavalue guid: {{{guid}}}";
			case DateTime dt:
				return $"datavalue date/time: {dt:yyyy/MM/dd HH:mm:ss.fffffff}";
			case DataValue value:
				return value.ToString();
			default:
				return $"!warning! detected unsupported object. type: {this.Value.GetType().FullName}, value: {this.Value}";
				//throw new NotSupportedException(string.Format(StringRes.DataValue_NotSupportedException, this.Value.GetType().FullName));
			}
		}

		/// <summary>
		///  指定したオブジェクトと現在のオブジェクトが等価かどうか判定します。
		/// </summary>
		/// <param name="obj">判定するオブジェクトです。</param>
		/// <returns>等しい場合は<see langword="true"/>、等しくない場合は<see langword="false"/>を返します。</returns>
		public override bool Equals(object? obj)
		{
			if (obj is DataValue val) {
				return this.Value?.Equals(val) ?? false;
			}
			return this.Value?.Equals(obj) ?? false;
		}

		/// <summary>
		///  現在のオブジェクトのハッシュ値を取得します。
		/// </summary>
		/// <returns>現在のオブジェクトが格納しているオブジェクトのハッシュ値です。</returns>
		public override int GetHashCode()
		{
			return this.Value?.GetHashCode() ?? base.GetHashCode();
		}
	}
}
