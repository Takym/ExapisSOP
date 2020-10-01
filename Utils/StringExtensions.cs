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
			case "on":
			case "enable":
			case "enabled":
			case "allow":
			case "pos":
			case "positive":
			case "one":
			case "1":
			case "high":
			case "t":
			case "y":
			case "+":
				result = true;
				return true;
			case "false":
			case "no":
			case "off":
			case "disable":
			case "disabled":
			case "deny":
			case "neg":
			case "negative":
			case "zero":
			case "0":
			case "low":
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
	}
}
