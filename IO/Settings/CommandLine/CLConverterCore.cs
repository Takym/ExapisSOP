/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Reflection;
using ExapisSOP.Utils;

namespace ExapisSOP.IO.Settings.CommandLine
{
	internal sealed class CLConverterCore : IEqualityComparer<string>, IEqualityComparer<(Type, string)>
	{
		private readonly CommandLineConverter                       _owner;
		private readonly Dictionary<string, Type>                   _switch_table;
		private readonly Dictionary<(Type, string), MemberInfo>     _option_table;
		private readonly Dictionary<MemberInfo, IArgumentConverter> _val_conv;

		internal CLConverterCore(CommandLineConverter owner)
		{
			_owner         = owner;
			_switch_table  = new Dictionary<string, Type>              (this);
			_option_table  = new Dictionary<(Type, string), MemberInfo>(this);
			_val_conv      = new Dictionary<MemberInfo, IArgumentConverter>();
			this.ResetCache();
		}

		private void ResetCache()
		{
			for (int i = 0; i < _owner.ResultTypes.Count; ++i) {
				var sattr = _owner.ResultTypes[i].GetCustomAttribute<SwitchAttribute>();
				if (sattr != null) {
					this.AddSwitchType(sattr.LongName,  _owner.ResultTypes[i]);
					this.AddSwitchType(sattr.ShortName, _owner.ResultTypes[i]);
					this.AddSwitchType(sattr.AltName,   _owner.ResultTypes[i]);
					var members = _owner.ResultTypes[i].GetMembers();
					for (int j = 0; j < members.Length; ++j) {
						var oattr = members[j].GetCustomAttribute<OptionAttribute>();
						if (oattr != null) {
							if ((members[j] is FieldInfo    fi && fi.IsPublic && !fi.IsInitOnly) ||
								(members[j] is PropertyInfo pi && (pi.SetMethod?.IsPublic ?? false))) {
								this.AddOptionType(oattr.LongName,  _owner.ResultTypes[i], members[j]);
								this.AddOptionType(oattr.ShortName, _owner.ResultTypes[i], members[j]);
								this.AddOptionType(oattr.AltName,   _owner.ResultTypes[i], members[j]);
								var ac = oattr.ArgumentConverter;
								if (ac != null && !ac.IsAbstract && (ac.IsClass || ac.IsValueType) &&
									typeof(IArgumentConverter).IsAssignableFrom(ac)) {
									var obj = ac.GetConstructor(Array.Empty<Type>())?.Invoke(null) as IArgumentConverter;
									if (obj != null) {
										_val_conv.Add(members[j], obj);
									}
								}
							}
						}
					}
				}
			}
		}

		internal bool SwitchesToDictionary(Switch[] swt, out IDictionary<Type, object> result)
		{
			bool ret = true;
			result = new Dictionary<Type, object>();
			for (int i = 0; i < swt.Length; ++i) {
				if (_switch_table.ContainsKey(swt[i].Name)) {
					try {
						var t   = _switch_table[swt[i].Name];
						var obj = Activator.CreateInstance(t);
						var opt = swt[i].Options;
						for (int j = 0; j < opt.Length; ++j) {
							var key = (t, opt[j].Name);
							if (_option_table.ContainsKey(key)) {
								var mi  = _option_table[key];
								var ac  = _val_conv.ContainsKey(mi) ? _val_conv[mi] : null;
								var miw = new MemberInfoWrapper(mi);
								if (miw.Valid) {
									if (ac == null && _owner.Converters.ContainsKey(miw.MemberType)) {
										ac = _owner.Converters[miw.MemberType];
									}
									if (ac == null) {
										var val = this.StringArrayToObject(miw.MemberType, this.OptionValuesToStrings(opt[j].Values));
										if (val != null) {
											miw.SetValue(obj, val);
										}
									} else {
										miw.SetValue(obj, ac.Convert(this.OptionValuesToStrings(opt[j].Values)));
									}
								}
							} else if (string.IsNullOrEmpty(opt[j].Name)) {
								continue;
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
				} else if (string.IsNullOrEmpty(swt[i].Name)) {
					continue;
				} else {
					ret = false;
				}
			}
			return ret;
		}

		private object? StringArrayToObject(Type type, string[] values)
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
						set?.Invoke(result, new object?[] { this.StringToObject(type.GetElementType()!, values[i]), i });
					}
				}
				return result;
			} else if (values.Length >= 1) {
				return this.StringToObject(type, values[0]);
			} else {
				return null;
			}
		}

		private object? StringToObject(Type type, string value)
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

		public bool Equals(string? x, string? y)
		{
			if (_owner.CaseSensitive) {
				return x == y;
			} else {
				if (x == y) {
					return true;
				}
				if (x == null || y == null) {
					return false;
				}
				return x.ToLower() == y.ToLower();
			}
		}

		public bool Equals((Type, string) x, (Type, string) y)
		{
			if (_owner.CaseSensitive) {
				return x == y;
			} else {
				if (x == y) {
					return true;
				}
				return x.Item1           == y.Item1
					&& x.Item2.ToLower() == x.Item2.ToLower();
			}
		}

		public int GetHashCode(string obj)
		{
			if (_owner.CaseSensitive) {
				return obj.GetHashCode();
			} else {
				return obj.ToLower().GetHashCode();
			}
		}

		public int GetHashCode((Type, string) obj)
		{
			if (_owner.CaseSensitive) {
				return obj.GetHashCode();
			} else {
				return obj.Item1.GetHashCode() ^ obj.Item2.ToLower().GetHashCode();
			}
		}

		private void AddSwitchType(string? name, Type type)
		{
			if (name != null) {
				_switch_table.Add(name, type);
			}
		}

		private void AddOptionType(string? name, Type type, MemberInfo mi)
		{
			if (name != null) {
				_option_table.Add((type, name), mi);
			}
		}

		private string[] OptionValuesToStrings(Option.Value[] values)
		{
			var result = new string[values.Length];
			for (int i = 0; i < result.Length; ++i) {
				result[i] = values[i].Text;
			}
			return result;
		}

		private class MemberInfoWrapper
		{
			private readonly MemberInfo _mi;

			internal bool Valid => _mi is FieldInfo || _mi is PropertyInfo;

			internal Type MemberType
			{
				get
				{
					if (_mi is FieldInfo fi) {
						return fi.FieldType;
					} else if (_mi is PropertyInfo pi) {
						return pi.PropertyType;
					} else {
						return null!;
					}
				}
			}

			internal MemberInfoWrapper(MemberInfo mi)
			{
				_mi = mi;
			}

			internal void SetValue(object? obj, object? value)
			{
				if (obj == null || value == null) {
					return;
				}
				if (_mi is FieldInfo fi) {
					fi.SetValue(obj, value);
				} else if (_mi is PropertyInfo pi) {
					pi.SetValue(obj, value);
				}
			}
		}
	}
}
