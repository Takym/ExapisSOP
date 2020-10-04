/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

namespace ExapisSOP.Utils
{
	/// <summary>
	///  <see cref="string"/>クラスを拡張します。
	///  このクラスは静的です。
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		///  指定された文字列を論理値へ変換します。
		///  <see langword="true"/>、<see langword="false"/>以外の一部の単語にも対応しています。
		/// </summary>
		/// <param name="s">変換する文字列です。</param>
		/// <param name="result">変換結果を格納する変数です。</param>
		/// <returns>
		///  指定された文字列が有効な論理値を表す場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </returns>
		public static bool TryToBoolean(this string s, out bool result)
		{
			string text = s.ToLower().Trim();
			switch (text) {
			case "true":
			case "yes":
			case "ot":
			case "on":
			case "enable":
			case "enabled":
			case "allow":
			case "high":
			case "pos":
			case "positive":
			case "one":
			case "1":
			case "t":
			case "y":
			case "+":
				result = true;
				return true;
			case "false":
			case "no":
			case "not":
			case "off":
			case "disable":
			case "disabled":
			case "deny":
			case "low":
			case "neg":
			case "negative":
			case "zero":
			case "0":
			case "f":
			case "n":
			case "-":
				result = false;
				return true;
			default:
				result = false;
				return false;
			}
		}

		/// <summary>
		///  指定された文字列を指定された文字数に省略します。
		/// </summary>
		/// <remarks>
		///  <paramref name="s"/>の文字数が<paramref name="count"/>より大きい場合は、
		///  先頭の<c><paramref name="count"/>-3</c>文字に三つのピリオドを付加して省略後の文字数に合わせます。
		///  <paramref name="s"/>の文字数が<paramref name="count"/>より小さい場合は、
		///  <see cref="string.PadRight(int)"/>を利用して省略後の文字数に合わせます。
		///  <paramref name="s"/>の文字数が<paramref name="count"/>と同じ場合はそのまま返します。
		/// </remarks>
		/// <param name="s">省略する文字列です。</param>
		/// <param name="count">省略後の文字数です。</param>
		/// <returns>
		///  指定された文字数に収まる文字列です。
		/// </returns>
		public static string Abridge(this string s, int count)
		{
			s ??= string.Empty;
			if (s.Length > count) {
				return s.Remove(count - 3) + "...";
			} else if (s.Length == count) {
				return s;
			} else {
				return s.PadRight(count);
			}
		}

		/// <summary>
		///  指定された文字列を1行に収めます。
		/// </summary>
		/// <param name="s">1行に収める必要のある文字列です。</param>
		/// <returns>改行やタブが削除され、1行で表現された文字列です。</returns>
		public static string FitToLine(this string s)
		{
			return s.Replace("\r", "[CR]").Replace("\n", "[LF]").Replace("\t", "[TB]").Replace("　", "[SP]");
		}
	}
}
