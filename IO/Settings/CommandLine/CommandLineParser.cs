/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExapisSOP.IO.Settings.CommandLine
{
	/// <summary>
	///  コマンド行引数を解析します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class CommandLineParser
	{
		/// <summary>
		///  指定されたコマンド行引数を表す配列を解析し、スイッチの配列へ変換します。
		/// </summary>
		/// <param name="args">解析するコマンド行引数を格納した配列です。</param>
		/// <returns>指定されたコマンド行引数と同じ内容を表すスイッチの配列です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static Switch[] Parse(params string[] args)
		{
			if (args == null) {
				throw new ArgumentNullException(nameof(args));
			}
			var result  = new List<Switch>();
			var options = new List<Option>();
			var values  = new List<Option.Value>();
			string cur_s = string.Empty;
			string cur_o = string.Empty;
			for (int i = 0; i < args.Length; ++i) {
				if (args[i].StartsWith("/")) {
					options.Add(new Option(cur_o, values.ToArray()));
					values .Clear();
					result .Add(new Switch(cur_s, options.ToArray()));
					options.Clear();
					cur_s = args[i].Substring(1).Trim();
					cur_o = string.Empty;
				} else if (args[i].StartsWith("-")) {
					options.Add(new Option(cur_o, values.ToArray()));
					values .Clear();
					int a = args[i].IndexOf(":");
					if (a > 0) {
						cur_o = args[i].Substring(1, a - 1);
						values.Add(new Option.Value(args[i].Substring(a + 1)));
					} else {
						cur_o = args[i].Substring(1).Trim();
					}
				} else if (args[i].StartsWith("$")) {
					values.Add(new Option.Value(args[i].Substring(1)));
				} else if (args[i].StartsWith("@")) {
					var fname = args[i].Substring(1);
					if (File.Exists(fname)) {
						using (var fs = new FileStream(fname, FileMode.Open, FileAccess.Read, FileShare.Read)) {
							values.Add(new Option.Value(fs));
						}
					} else {
						values.Add(new Option.Value(fname));
					}
				} else {
					values.Add(new Option.Value(args[i]));
				}
			}
			options.Add(new Option(cur_o, values .ToArray()));
			result .Add(new Switch(cur_s, options.ToArray()));
			return result.ToArray();
		}

		/// <summary>
		///  指定されたコマンド行引数を表す配列を解析し、スイッチの配列へ変換します。
		/// </summary>
		/// <param name="args">解析するコマンド行引数を格納した列挙体です。</param>
		/// <returns>指定されたコマンド行引数と同じ内容を表すスイッチの配列です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static Switch[] Parse(IEnumerable<string> args)
		{
			if (args == null) {
				throw new ArgumentNullException(nameof(args));
			}
			return Parse(args.ToArray());
		}

		/// <summary>
		///  指定されたコマンド行引数を表すスイッチの配列からコマンド名を取得します。
		/// </summary>
		/// <param name="switches">コマンド行引数を表すスイッチの配列です。</param>
		/// <returns>
		///  コマンド行引数で指定されているコマンド名、または、指定されていない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static string? GetCommandName(this Switch[] switches)
		{
			if (switches == null) {
				throw new ArgumentNullException(nameof(switches));
			}
			if (switches                     .Length > 0 && switches[0]           .Name == string.Empty &&
				switches[0].Options          .Length > 0 && switches[0].Options[0].Name == string.Empty &&
				switches[0].Options[0].Values.Length > 0) {
				return switches[0].Options[0].Values[0].Text;
			}
			return null;
		}

		/// <summary>
		///  コマンド行引数をオブジェクトへ変換します。
		/// </summary>
		/// <param name="converter">実際に変換を行うオブジェクトです。</param>
		/// <param name="args">コマンド行引数の全部または一部を表す文字列の列挙体です。</param>
		/// <returns>変換結果を表す新しいオブジェクトです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static object? Convert(this IArgumentConverter converter, IEnumerable<string> args)
		{
			if (converter == null) {
				throw new ArgumentNullException(nameof(converter));
			}
			if (args == null) {
				throw new ArgumentNullException(nameof(args));
			}
			return converter.Convert(args.ToArray());
		}

		/// <summary>
		///  コマンド行引数を指定された型'<typeparamref name="T"/>'へ変換します。
		/// </summary>
		/// <typeparam name="T">変換後のオブジェクトです。</typeparam>
		/// <param name="converter">実際に変換を行うオブジェクトです。</param>
		/// <param name="args">コマンド行引数の全部または一部を表す文字列配列です。</param>
		/// <returns>変換結果を表す新しい指定された型'<typeparamref name="T"/>'のオブジェクトです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static T Convert<T>(this IArgumentConverter<T> converter, IEnumerable<string> args)
		{
			if (converter == null) {
				throw new ArgumentNullException(nameof(converter));
			}
			if (args == null) {
				throw new ArgumentNullException(nameof(args));
			}
			return converter.Convert(args.ToArray());
		}

		/// <summary>
		///  <see cref="ExapisSOP.IO.Settings.CommandLine.CommandLineConverter"/>の変換結果から値を取得します。
		/// </summary>
		/// <typeparam name="T">取得する値の種類です。</typeparam>
		/// <param name="converterResult">変換結果を格納している辞書です。</param>
		/// <returns>
		///  指定された型の値が存在する場合は有効なインスタンス、
		///  または、存在しない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static T GetValue<T>(this IDictionary<Type, object> converterResult)
		{
			if (converterResult == null) {
				throw new ArgumentNullException(nameof(converterResult));
			}
			if (converterResult.ContainsKey(typeof(T))) {
				return ((T)(converterResult[typeof(T)]));
			} else {
				return default!;
			}
		}
	}
}
