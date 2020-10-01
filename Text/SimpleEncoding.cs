/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ExapisSOP.Text
{
	/// <summary>
	///  単純な文字符号化方式を表します。
	/// </summary>
	[Serializable()]
	public class SimpleEncoding : Encoding
	{
		private static readonly List               <char>       _remap_table;
		private static readonly Dictionary         <char, byte> _remap_table_rev;
		private static readonly IReadOnlyList      <char>       _rmt;
		private static readonly IReadOnlyDictionary<char, byte> _rmt_rev;
		private        readonly bool                            _remove_nullchar;

		static SimpleEncoding()
		{
			_remap_table = new List<char>() {
				'\0', '\r', '\n', '\t', ' ',  '.',  ',',  ':',  ';',  '!',  '?',  '\'', '\"', '`',  '\\', '_',
				'0',  '1',  '2',  '3',  '4',  '5',  '6',  '7',  '8',  '9',  'a',  'b',  'c',  'd',  'e',  'f',
				'g',  'h',  'i',  'j',  'k',  'l',  'm',  'n',  'o',  'p',  'q',  'r',  's',  't',  'u',  'v',
				'w',  'x',  'y',  'z',  'ā',  'ē',  'ī',  'ō',  'ū',  '(',  ')',  '[',  ']',  '+',  '-',  '^',
				//-------------------------------------------------------------------------------------------//
				'〇', '一', '二', '三', '四', '五', '六', '七', '八', '九', 'A',  'B',  'C',  'D',  'E',  'F',
				'G',  'H',  'I',  'J',  'K',  'L',  'M',  'N',  'O',  'P',  'Q',  'R',  'S',  'T',  'U',  'V',
				'W',  'X',  'Y',  'Z',  'Ā',  'Ē',  'Ī',  'Ō',  'Ū',  '<',  '>',  '{',  '}',  '*',  '/',  '~',
				'@',  '#',  '$',  '%',  '&',  '=',  '|',  'ｧ',  'ｨ',  'ｩ',  'ｪ',  'ｫ',  'ｬ',  'ｭ',  'ｮ',  'ｯ',
				//===========================================================================================//
				'ｱ',  'ｲ',  'ｳ',  'ｴ',  'ｵ',  'ｶ',  'ｷ',  'ｸ',  'ｹ',  'ｺ',  'ｻ',  'ｼ',  'ｽ',  'ｾ',  'ｿ',  'ﾀ',
				'ﾁ',  'ﾂ',  'ﾃ',  'ﾄ',  'ﾅ',  'ﾆ',  'ﾇ',  'ﾈ',  'ﾉ',  'ﾊ',  'ﾋ',  'ﾌ',  'ﾍ',  'ﾎ',  'ﾏ',  'ﾐ',
				'ﾑ',  'ﾒ',  'ﾓ',  'ﾔ',  'ﾕ',  'ﾖ',  'ﾗ',  'ﾘ',  'ﾙ',  'ﾚ',  'ﾛ',  'ﾜ',  'ｦ',  'ﾝ',  'ﾞ',  'ﾟ',
				'α', 'β', 'γ', 'δ', 'ε', 'ζ', 'η', 'θ', 'ι', 'κ', 'λ', 'μ', 'ν', 'ξ', 'ο', 'π',
				//-------------------------------------------------------------------------------------------//
				'ρ', 'σ', 'τ', 'υ', 'φ', 'χ', 'ψ', 'ω', 'Α', 'Β', 'Γ', 'Δ', 'Ε', 'Ζ', 'Η', 'Θ',
				'Ι', 'Κ', 'Λ', 'Μ', 'Ν', 'Ξ', 'Ο', 'Π', 'Ρ', 'Σ', 'Τ', 'Υ', 'Φ', 'Χ', 'Ψ', 'Ω',
			};
			_remap_table_rev = new Dictionary<char, byte>();
			for (byte i = 0; i < _remap_table.Count; ++i) {
				_remap_table_rev.Add(_remap_table[i], i);
			}
			_rmt = _remap_table.AsReadOnly();
			_rmt_rev = new ReadOnlyDictionary<char, byte>(_remap_table_rev);
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Text.SimpleEncoding"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="removeNullChar">
		///  NULL文字を削除する場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を設定します。
		///  既定値は<see langword="false"/>です。
		/// </param>
		public SimpleEncoding(bool removeNullChar = false)
		{
			_remove_nullchar = removeNullChar;
		}

		/// <summary>
		///  指定された文字列をバイト配列へ変換(エンコード)した時のバイト数を計算します。
		/// </summary>
		/// <param name="chars">計算に利用する文字配列です。</param>
		/// <param name="index">文字配列の開始位置(オフセット)です。</param>
		/// <param name="count">計算に利用する文字数です。</param>
		/// <returns>バイト配列に変換した後のバイト数を表します。</returns>
		public override int GetByteCount(char[] chars, int index, int count)
		{
			int byteCount = 0;
			for (int i = 0; i < count; ++i) {
				if (_rmt_rev.ContainsKey(chars[i + index])) {
					++byteCount;
				} else {
					byteCount += 3;
				}
			}
			return byteCount;
		}

		/// <summary>
		///  指定された文字列をバイト配列へ変換(エンコード)します。
		/// </summary>
		/// <param name="chars">変換する文字配列です。</param>
		/// <param name="charIndex">文字配列の開始位置(オフセット)です。</param>
		/// <param name="charCount">文字数です。</param>
		/// <param name="bytes">変換後のバイト配列の格納先です。</param>
		/// <param name="byteIndex">バイト配列の開始位置(オフセット)です。</param>
		/// <returns>バイト配列に変換した後のバイト数を表します。</returns>
		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			int bytePos = byteIndex;
			for (int i = 0; i < charCount; ++i) {
				if (_remove_nullchar && chars[i + charIndex] == '\0') {
					bytes[bytePos] = _rmt_rev[' '];
					++bytePos;
				} else if (_rmt_rev.ContainsKey(chars[i + charIndex])) {
					bytes[bytePos] = _rmt_rev[chars[i + charIndex]];
					++bytePos;
				} else {
					ushort c = chars[i + charIndex];
					bytes[bytePos] = 0xFF; ++bytePos;
					bytes[bytePos] = ((byte)(c & 0xFF)); ++bytePos;
					bytes[bytePos] = ((byte)(c >> 8));   ++bytePos;
				}
			}
			return bytePos - byteIndex;
		}

		/// <summary>
		///  指定されたバイト配列を文字列へ逆変換(デコード)した時の文字数を計算します。
		/// </summary>
		/// <param name="bytes">計算に利用するバイト配列です。</param>
		/// <param name="index">バイト配列の開始位置(オフセット)です。</param>
		/// <param name="count">計算に利用するバイト数です。</param>
		/// <returns>文字配列に逆変換した後の文字数を表します。</returns>
		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			int charCount = 0;
			for (int i = 0; i < count; ++i) {
				if (bytes[i + index] == 0xFF) {
					i += 2;
				}
				++charCount;
			}
			return charCount;
		}

		/// <summary>
		///  指定されたバイト配列を文字列へ逆変換(デコード)します。
		/// </summary>
		/// <param name="bytes">逆変換するバイト配列です。</param>
		/// <param name="byteIndex">バイト配列の開始位置(オフセット)です。</param>
		/// <param name="byteCount">バイト数です。</param>
		/// <param name="chars">逆変換後の文字配列の格納先です。</param>
		/// <param name="charIndex">文字配列の開始位置(オフセット)です。</param>
		/// <returns>文字配列に逆変換した後の文字数を表します。</returns>
		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			int charPos = charIndex;
			for (int i = 0; i < byteCount; ++i) {
				if (bytes[i + byteIndex] == 0xFF) {
					++i;
					byte low = bytes[i + byteIndex];
					++i;
					byte high = bytes[i + byteIndex];
					chars[charPos] = ((char)((high << 8) | low));
				} else {
					chars[charPos] = _rmt[bytes[i + byteIndex]];
				}
				if (_remove_nullchar && chars[charPos] == '\0') {
					chars[charPos] = ' ';
				}
				++charPos;
			}
			return charPos - charIndex;
		}

		/// <summary>
		///  指定された文字数の文字列をバイト配列へ変換(エンコード)した時の最大バイト数を予測計算します。
		/// </summary>
		/// <param name="charCount">文字数です。</param>
		/// <returns>バイト配列へ変換(エンコード)した時の最大バイト数です。</returns>
		public override int GetMaxByteCount(int charCount)
		{
			return charCount * 3;
		}

		/// <summary>
		///  指定されたバイト数のバイト配列を文字列へ逆変換(デコード)した時の最大文字数を予測計算します。
		/// </summary>
		/// <param name="byteCount">バイト数です。</param>
		/// <returns>文字配列へ逆変換(デコード)した時の最大文字数です。</returns>
		public override int GetMaxCharCount(int byteCount)
		{
			return byteCount;
		}
	}
}
