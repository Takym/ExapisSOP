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
using System.Reflection;
using ExapisSOP.Utils;

namespace ExapisSOP.IO.Settings.CommandLine
{
	/// <summary>
	///  コマンド行引数を<see cref="ExapisSOP.IO.Settings.CommandLine.Switch"/>の配列へ変換する機能を提供します。
	///  また、スイッチ配列を任意のオブジェクトへ変換します。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class CommandLineConverter : IArgumentConverter<Switch[]>
	{
		private          bool                                       _need_to_reset_cache;
		private readonly Dictionary<string, Type>                   _switch_table;
		private readonly Dictionary<(Type, string), MemberInfo>     _option_table;
		private readonly Dictionary<MemberInfo, IArgumentConverter> _val_conv;

		/// <summary>
		///  スイッチ配列の変換後の型を表します。
		/// </summary>
		public ObservableCollection<Type> ResultTypes { get; }

		/// <summary>
		///  コマンド行引数の値の型変換を行うオブジェクトを保持した辞書を取得します。
		/// </summary>
		public Dictionary<Type, IArgumentConverter> Converters { get; }

		/*
		/// <summary>
		///  大文字と小文字を区別する場合は<see langword="true"/>、区別しない場合は<see langword="false"/>を設定します。
		/// </summary>
		public bool CaseSensitive { get; set; }
		//*/

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Settings.CommandLine.CommandLineConverter"/>'の新しいインスタンスを生成します。
		/// </summary>
		public CommandLineConverter()
		{
			_need_to_reset_cache = true;
			_switch_table        = new Dictionary<string, Type>();
			_option_table        = new Dictionary<(Type, string), MemberInfo>();
			_val_conv            = new Dictionary<MemberInfo, IArgumentConverter>();
			this.ResultTypes                    = new ObservableCollection<Type>();
			this.ResultTypes.CollectionChanged += this.ResultTypes_CollectionChanged;
			this.Converters                     = new Dictionary<Type, IArgumentConverter>();
			this.Converters                     .Add(typeof(Switch[]), this);
			//this.CaseSensitive                = true;
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
			this.ResetCache();
			bool ret = true;
			result = new Dictionary<Type, object>();
			for (int i = 0; i < switches.Length; ++i) {
				if (_switch_table.ContainsKey(switches[i].Name)) {
					try {
						var t   = _switch_table[switches[i].Name];
						var obj = Activator.CreateInstance(t);
						var opt = switches[i].Options;
						for (int j = 0; j < opt.Length; ++j) {
							var key = (t, opt[j].Name);
							if (_option_table.ContainsKey(key)) {
								var mi = _option_table[key];
								var ac = _val_conv[mi];
								if (mi is FieldInfo fi) {
									if (ac == null && this.Converters.ContainsKey(fi.FieldType)) {
										ac = this.Converters[fi.FieldType];
									}
									if (ac == null) {
										var val = this.ConvertCore(fi.FieldType, opt[j].Values);
										if (val != null) {
											fi.SetValue(obj, val);
										}
									} else {
										fi.SetValue(obj, ac.Convert(opt[j].Values));
									}
								} else if (mi is PropertyInfo pi) {
									if (ac == null && this.Converters.ContainsKey(pi.PropertyType)) {
										ac = this.Converters[pi.PropertyType];
									}
									if (ac == null) {
										var val = this.ConvertCore(pi.PropertyType, opt[j].Values);
										if (val != null) {
											pi.SetValue(obj, val);
										}
									} else {
										pi.SetValue(obj, ac.Convert(opt[j].Values));
									}
								}
							} else {
								ret = false;
							}
						}
						if (obj == null) {
							ret = false;
						} else {
							result.Add(t, obj);
						}
					} catch {
						ret = false;
					}
				} else {
					ret = false;
				}
			}
			return ret;
		}

		private void ResultTypes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			_need_to_reset_cache = true;
		}

		private void ResetCache()
		{
			if (_need_to_reset_cache) {
				_switch_table.Clear();
				_option_table.Clear();
				_val_conv    .Clear();
				for (int i = 0; i < this.ResultTypes.Count; ++i) {
					var sattr = this.ResultTypes[i].GetCustomAttribute<SwitchAttribute>();
					if (sattr != null) {
						_switch_table.Add(sattr.LongName, this.ResultTypes[i]);
						if (sattr.ShortName != null) {
							_switch_table.Add(sattr.ShortName, this.ResultTypes[i]);
						}
						var members = this.ResultTypes[i].GetMembers();
						for (int j = 0; j < members.Length; ++j) {
							var oattr = members[j].GetCustomAttribute<OptionAttribute>();
							if (oattr != null) {
								if ((members[j] is FieldInfo    fi && fi.IsPublic && !fi.IsInitOnly) ||
									(members[j] is PropertyInfo pi && (pi.SetMethod?.IsPublic ?? false))) {
									_option_table.Add((this.ResultTypes[i], oattr.LongName), members[j]);
									if (oattr.ShortName != null) {
										_option_table.Add((this.ResultTypes[i], oattr.ShortName), members[j]);
									}
									var ac = oattr.ArgumentConverter;
									if (ac != null && !ac.IsAbstract && (ac.IsClass || ac.IsValueType) &&
										typeof(IArgumentConverter).IsAssignableFrom(ac)) {
										var obj = ac.GetConstructor(new Type[0])?.Invoke(null) as IArgumentConverter;
										if (obj != null) {
											_val_conv.Add(members[j], obj);
										}
									}
								}
							}
						}
					}
				}
				_need_to_reset_cache = false;
			}
		}

		private object? ConvertCore(Type type, string[] values)
		{
			if (values.Length == 0) {
				if (type == typeof(bool)) {
					return true;
				} else {
					return null;
				}
			} else if (type == typeof(object[]) || type == typeof(string[])) {
				return values;
			} else if (typeof(Array).IsAssignableFrom(type)) {
				var ci  = type.GetConstructors()[0];
				var set = type.GetMethod(nameof(Array.SetValue), BindingFlags.Public | BindingFlags.Instance);
				var result = ci.Invoke(new object[] { values.Length });
				if (result != null) {
					for (int i = 0; i < values.Length; ++i) {
						set?.Invoke(result, new object?[] { this.ConvertCore(type.GetElementType()!, values[i]), i });
					}
				}
				return result;
			} else if (values.Length >= 1) {
				return this.ConvertCore(type, values[0]);
			} else {
				return null;
			}
		}

		private object? ConvertCore(Type type, string value)
		{
			if (typeof(string).IsAssignableFrom(type)) {
				return value;
			} else if (typeof(char).IsAssignableFrom(type) && value.Length >= 1) {
				return value[0];
			} else if (typeof(bool).IsAssignableFrom(type)) {
				value.TryToBoolean(out bool val);
				return val;
			} else if (typeof(byte).IsAssignableFrom(type)) {
				byte.TryParse(value, out byte val);
				return val;
			} else if (typeof(sbyte).IsAssignableFrom(type)) {
				sbyte.TryParse(value, out sbyte val);
				return val;
			} else if (typeof(ushort).IsAssignableFrom(type)) {
				ushort.TryParse(value, out ushort val);
				return val;
			} else if (typeof(short).IsAssignableFrom(type)) {
				short.TryParse(value, out short val);
				return val;
			} else if (typeof(uint).IsAssignableFrom(type)) {
				uint.TryParse(value, out uint val);
				return val;
			} else if (typeof(int).IsAssignableFrom(type)) {
				int.TryParse(value, out int val);
				return val;
			} else if (typeof(ulong).IsAssignableFrom(type)) {
				ulong.TryParse(value, out ulong val);
				return val;
			} else if (typeof(long).IsAssignableFrom(type)) {
				long.TryParse(value, out long val);
				return val;
			} else if (typeof(float).IsAssignableFrom(type)) {
				float.TryParse(value, out float val);
				return val;
			} else if (typeof(double).IsAssignableFrom(type)) {
				double.TryParse(value, out double val);
				return val;
			} else if (typeof(decimal).IsAssignableFrom(type)) {
				decimal.TryParse(value, out decimal val);
				return val;
			} else if (typeof(Guid).IsAssignableFrom(type)) {
				Guid.TryParse(value, out var val);
				return val;
			} else if (typeof(DateTime).IsAssignableFrom(type)) {
				DateTime.TryParse(value, out var val);
				return val;
			} else {
				return null;
			}
		}

#if NET48
		object? IArgumentConverter.Convert(string[] args)
		{
			return this.Convert(args);
		}
#endif
	}
}
