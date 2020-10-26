/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ExapisSOP.IO.Settings.CommandLine
{
	/// <summary>
	///  コマンド行引数を<see cref="ExapisSOP.IO.Settings.CommandLine.Switch"/>の配列へ変換する機能を提供します。
	///  また、スイッチ配列を任意のオブジェクトへ変換します。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class CommandLineConverter : IArgumentConverter<Switch[]>
	{
		private bool             _caseSensitive;
		private CLConverterCore? _cache;

		internal bool ResetCache => _cache == null;

		/// <summary>
		///  スイッチ配列の変換後の型を取得します。
		/// </summary>
		public ObservableCollection<Type> ResultTypes { get; }

		/// <summary>
		///  コマンド行引数の値の型変換を行うオブジェクトを保持した辞書を取得します。
		/// </summary>
		public Dictionary<Type, IArgumentConverter> Converters { get; }

		/// <summary>
		///  大文字と小文字を区別する場合は<see langword="true"/>、区別しない場合は<see langword="false"/>を設定します。
		/// </summary>
		public bool CaseSensitive
		{
			get => _caseSensitive;
			set
			{
				_cache         = null;
				_caseSensitive = value;
			}
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.CommandLine.CommandLineConverter"/>'の新しいインスタンスを生成します。
		/// </summary>
		public CommandLineConverter()
		{
			_cache         = null;
			_caseSensitive = true;
			this.ResultTypes                    = new ObservableCollection<Type>();
			this.ResultTypes.CollectionChanged += this.ResultTypes_CollectionChanged;
			this.Converters                     = new Dictionary<Type, IArgumentConverter>();
			this.Converters                     .Add(typeof(Switch[]), this);
		}

		/// <summary>
		///  コマンド行引数をスイッチ配列へ変換します。
		/// </summary>
		/// <param name="args">コマンド行引数の全部または一部を表す文字列配列です。</param>
		/// <returns>変換結果を表す新しいスイッチ配列です。</returns>
		public Switch[] Convert(params string[] args)
		{
			return CommandLineParser.Parse(args);
		}

		/// <summary>
		///  指定されたスイッチ配列をオブジェクトへ変換します。
		/// </summary>
		/// <param name="switches">スイッチ配列です。</param>
		/// <param name="result">変換後のオブジェクトです。失敗した場合でも一部の値を格納します。</param>
		/// <returns>変換に成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public bool TryConvert(Switch[] switches, out IDictionary<Type, object> result)
		{
			if (_cache == null) {
				_cache = new CLConverterCore(this);
			}
			return _cache.SwitchesToDictionary(switches, out result);
		}

		private void ResultTypes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			_cache = null;
		}

#if NET48
		object? IArgumentConverter.Convert(string[] args)
		{
			return this.Convert(args);
		}
#endif
	}
}
