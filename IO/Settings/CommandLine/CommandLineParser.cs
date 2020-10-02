﻿/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;

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
		/// <param name="args">解析するコマンド行引数です。</param>
		/// <returns>指定されたコマンド行引数と同じ内容を表すスイッチの配列です。</returns>
		public static Switch[] Parse(string[] args)
		{
			var result  = new List<Switch>();
			var options = new List<Option>();
			var values  = new List<string>();
			string cur_s = string.Empty;
			string cur_o = string.Empty;
			for (int i = 0; i < args.Length; ++i) {
				if (args[i].StartsWith("/")) {
					if (options.Count != 0) {
						result.Add(new Switch(cur_s, options.ToArray()));
						options.Clear();
					}
					cur_s = args[i].Substring(1).Trim();
					cur_o = string.Empty;
				} else if (args[i].StartsWith("-")) {
					if (values.Count != 0) {
						options.Add(new Option(cur_o, values.ToArray()));
						values.Clear();
					}
					int a = args[i].IndexOf(":");
					if (a > 0) {
						cur_o = args[i].Substring(1, a - 1);
						values.Add(args[i].Substring(a + 1));
					} else {
						cur_o = args[i].Substring(1).Trim();
					}
				} else if (args[i].StartsWith("$")) {
					values.Add(args[i].Substring(1));
				} else {
					values.Add(args[i]);
				}
			}
			if (values.Count != 0) {
				options.Add(new Option(cur_o, values.ToArray()));
			}
			if (options.Count != 0) {
				result.Add(new Switch(cur_s, options.ToArray()));
			}
			return result.ToArray();
		}

		/// <summary>
		///  指定されたコマンド行引数を表すスイッチの配列からコマンド名を取得します。
		/// </summary>
		/// <param name="switches">コマンド行引数を表すスイッチの配列です。</param>
		/// <returns>
		///  コマンド行引数で指定されているコマンド名、または、指定されていない場合は<see langword="null"/>を返します。
		/// </returns>
		public static string? GetCommandName(this Switch[] switches)
		{
			if (switches == null) {
				throw new ArgumentNullException(nameof(switches));
			}
			if (switches                     .Length > 0 && switches[0]           .Name == string.Empty &&
				switches[0].Options          .Length > 0 && switches[0].Options[0].Name == string.Empty &&
				switches[0].Options[0].Values.Length > 0) {
				return switches[0].Options[0].Values[0];
			}
			return null;
		}
	}
}