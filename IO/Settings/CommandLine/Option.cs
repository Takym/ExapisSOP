/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;

namespace ExapisSOP.IO.Settings.CommandLine
{
	/// <summary>
	///  コマンド行引数のオプションを表します。
	/// </summary>
	public class Option
	{
		/// <summary>
		///  このオプションの名前を取得します。
		/// </summary>
		public string Name { get; }

		/// <summary>
		///  このオプションに登録されている値を取得します。
		/// </summary>
		public Value[] Values { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.CommandLine.Option"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="name">新しいオプションの名前です。</param>
		/// <param name="values">新しいオプションに登録する値です。</param>
		public Option(string name, Value[] values)
		{
			this.Name   = name;
			this.Values = values;
		}

		/// <summary>
		///  オプションに登録する値を表します。
		/// </summary>
		public class Value
		{
			/// <summary>
			///  引数が応答ファイルを指し示していた場合、応答ファイルへの絶対パスを取得します。
			///  それ以外の場合はコマンド行引数に格納されていた値を取得します。
			/// </summary>
			public string Source { get; }

			/// <summary>
			///  引数が応答ファイルを指し示していた場合、応答ファイルから読み取った値を取得します。
			///  それ以外の場合はコマンド行引数に格納されていた値を取得します。
			/// </summary>
			public string Text { get; }

			/// <summary>
			///  型'<see cref="ExapisSOP.IO.Settings.CommandLine.Option.Value"/>'の新しいインスタンスを生成します。
			/// </summary>
			/// <param name="arg">この値が保持すべき引数を表す文字列です。</param>
			public Value(string arg)
			{
				this.Source = arg;
				this.Text   = arg;
			}

			/// <summary>
			///  型'<see cref="ExapisSOP.IO.Settings.CommandLine.Option.Value"/>'の新しいインスタンスを生成します。
			/// </summary>
			/// <param name="stream">応答ファイルを表すストリームです。</param>
			/// <exception cref="System.ArgumentNullException"/>
			public Value(Stream stream)
			{
				if (stream == null) {
					throw new ArgumentNullException(nameof(stream));
				}
				var sr = new StreamReader(stream, true);
				this.Source = stream is FileStream fs ? fs.Name : stream.GetType().FullName ?? string.Empty;
				this.Text   = sr.ReadToEnd();
			}
		}
	}
}
