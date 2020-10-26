/****
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
	///  <see cref="ExapisSOP.IO.Settings.CommandLine.ICommandLineService"/>の動作方法を指定します。
	/// </summary>
	public class CommandLineServiceOptions
	{
		/// <summary>
		///  スイッチ配列を任意のオブジェクトへの変換を行うかどうかを表す論理値を取得または設定します。
		/// </summary>
		public bool ConvertToObject { get; set; }

		/// <summary>
		///  スイッチ配列の変換後の型を取得します。
		/// </summary>
		public IList<Type> ResultTypes { get; }

		/// <summary>
		///  コマンド行引数の型変換器を返す関数を取得または設定します。
		/// </summary>
		public CreateConverterMappingTable GetConverters { get; set; }

		/// <summary>
		///  大文字と小文字を区別する場合は<see langword="true"/>、区別しない場合は<see langword="false"/>を設定します。
		/// </summary>
		public bool CaseSensitive { get; set; }

		/// <summary>
		///  コマンド行引数から設定情報を上書きできるかどうかを表す論理値を取得または設定します。
		/// </summary>
		public bool AllowOverrideSettings { get; set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.CommandLine.CommandLineServiceOptions"/>'の新しいインスタンスを生成します。
		/// </summary>
		public CommandLineServiceOptions()
		{
			this.ConvertToObject       = false;
			this.ResultTypes           = new List<Type>();
			this.GetConverters         = () => null;
			this.CaseSensitive         = true;
			this.AllowOverrideSettings = false;
		}

		/// <summary>
		///  コマンド行引数をオブジェクトへ変換する為の変換器を取得します。
		/// </summary>
		/// <returns>
		///  キー名に型の種類を渡すと変換器を返す辞書です。
		/// </returns>
		public delegate IDictionary<Type, IArgumentConverter>? CreateConverterMappingTable();
	}
}
