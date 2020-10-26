/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using ExapisSOP.Numerics;

namespace ExapisSOP.Utils
{
	/// <summary>
	///  文字列をより便利に扱う為の機能を提供します。
	///  このクラスは静的です。
	/// </summary>
	public static class StringUtil
	{
		private const           string   _chars    = "!#$0aAbBcCdDeEfF%&'gGhH1234()-=IiJjKkLlMmNnOoPp^~@[{]}5678qrstuv;+QRSTUV9WXYZwxyz,._";
		private static readonly Xorshift _xorshift = new Xorshift2();

		/// <summary>
		///  英数字と記号をランダムに並べた8～64文字の文字列を生成します。
		///  生成された文字列はファイル名に使用できます。
		/// </summary>
		/// <returns>生成された結果の分からない文字列です。</returns>
		public static string GetRandomText()
		{
			return GetRandomText(8, 64);
		}

		/// <summary>
		///  英数字と記号をランダムに並べた文字列を生成します。
		///  文字数は指定された範囲から自動的に決定します。
		///  生成された文字列はファイル名に使用できます。
		/// </summary>
		/// <param name="min">最小の文字数です。</param>
		/// <param name="max">最大の文字数です。</param>
		/// <returns>生成された結果の分からない文字列です。</returns>
		public static string GetRandomText(int min, int max)
		{
			return GetRandomText(min, max, _xorshift);
		}

		/// <summary>
		///  英数字と記号をランダムに並べた文字列を生成します。
		///  生成された文字列はファイル名に使用できます。
		/// </summary>
		/// <param name="len">生成される文字列の長さです。</param>
		/// <returns>生成された結果の分からない文字列です。</returns>
		public static string GetRandomText(int len)
		{
			return GetRandomText(len, _xorshift);
		}

		/// <summary>
		///  英数字と記号をランダムに並べた文字列を生成します。
		///  文字数は指定された範囲から自動的に決定します。
		///  生成された文字列はファイル名に使用できます。
		/// </summary>
		/// <param name="min">最小の文字数です。</param>
		/// <param name="max">最大の文字数です。</param>
		/// <param name="service"><see cref="ExapisSOP.Utils"/>サービスオブジェクトです。</param>
		/// <returns>生成された結果の分からない文字列です。</returns>
		public static string GetRandomText(int min, int max, IUtilityService service)
		{
#if NETCOREAPP3_1
			return GetRandomText(min, max, service.CreateRandom(Environment.TickCount64));
#elif NET48
			return GetRandomText(min, max, service.CreateRandom(Environment.TickCount));
#endif
		}

		/// <summary>
		///  英数字と記号をランダムに並べた文字列を生成します。
		///  生成された文字列はファイル名に使用できます。
		/// </summary>
		/// <param name="len">生成される文字列の長さです。</param>
		/// <param name="service"><see cref="ExapisSOP.Utils"/>サービスオブジェクトです。</param>
		/// <returns>生成された結果の分からない文字列です。</returns>
		public static string GetRandomText(int len, IUtilityService service)
		{
#if NETCOREAPP3_1
			return GetRandomText(len, service.CreateRandom(Environment.TickCount64));
#elif NET48
			return GetRandomText(len, service.CreateRandom(Environment.TickCount));
#endif
		}

		/// <summary>
		///  英数字と記号をランダムに並べた文字列を生成します。
		///  文字数は指定された範囲から自動的に決定します。
		///  生成された文字列はファイル名に使用できます。
		/// </summary>
		/// <param name="min">最小の文字数です。</param>
		/// <param name="max">最大の文字数です。</param>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <returns>生成された結果の分からない文字列です。</returns>
		public static string GetRandomText(int min, int max, IRandom random)
		{
			return GetRandomText(random.NextSInt32(min, max), random);
		}

		/// <summary>
		///  英数字と記号をランダムに並べた文字列を生成します。
		///  生成された文字列はファイル名に使用できます。
		/// </summary>
		/// <param name="length">生成される文字列の長さです。</param>
		/// <param name="random">疑似乱数生成器です。</param>
		/// <returns>生成された結果の分からない文字列です。</returns>
		public static string GetRandomText(int length, IRandom random)
		{
			string result = string.Empty;
			for (int i = 0; i < length; ++i) {
				result += _chars[random.NextSInt32(_chars.Length)];
			}
			return result;
		}
	}
}
