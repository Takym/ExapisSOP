/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace ExapisSOP.IO.Settings.CommandLine
{
	internal class CLManualsCore
	{
		private readonly CommandLineConverter                 _owner;
		private readonly CultureInfo                          _culture;
		private readonly Dictionary<Type, SwitchOptionInfo>   _switch_table;
		private readonly Dictionary<Type, SwitchOptionInfo[]> _option_table;

		internal CLManualsCore(CommandLineConverter owner, CultureInfo? info)
		{
			_owner        = owner;
			_culture      = info ?? CultureInfo.InstalledUICulture;
			_switch_table = new Dictionary<Type, SwitchOptionInfo>();
			_option_table = new Dictionary<Type, SwitchOptionInfo[]>();
			this.ResetCache();
		}

		private void ResetCache()
		{
			for (int i = 0; i < _owner.ResultTypes.Count; ++i) {
				var sattr = _owner.ResultTypes[i].GetCustomAttribute<SwitchAttribute>();
				if (sattr != null) {
					var    man  = _owner.ResultTypes[i].GetCustomAttributes<ManualAttribute>();
					string desc = string.Empty;
					foreach (var item in man) {
						if (_culture        == item.CultureInfo ||
							_culture.Parent == item.CultureInfo) {
							desc = item.Description;
						}
					}
					_switch_table.Add(_owner.ResultTypes[i], new SwitchOptionInfo(sattr.LongName, sattr.ShortName, sattr.AltName, desc));
					var members = _owner.ResultTypes[i].GetMembers();
					var optinfo = new List<SwitchOptionInfo>();
					for (int j = 0; j < members.Length; ++j) {
						var oattr = members[j].GetCustomAttribute<OptionAttribute>();
						if (oattr != null) {
							man  = _owner.ResultTypes[i].GetCustomAttributes<ManualAttribute>();
							desc = string.Empty;
							foreach (var item in man) {
								if (_culture        == item.CultureInfo ||
									_culture.Parent == item.CultureInfo) {
									desc = item.Description;
								}
							}
							optinfo.Add(new SwitchOptionInfo(oattr.LongName, oattr.ShortName, oattr.AltName, desc));
						}
					}
					_option_table.Add(_owner.ResultTypes[i], optinfo.ToArray());
				}
			}
		}

		internal string GetHelpText()
		{
			var sb = new StringBuilder();
			foreach (var pair in _switch_table) {
				sb.Append($"/{pair.Value.LongName}");
				if (pair.Value.ShortName != null) {
					sb.Append($", /{pair.Value.ShortName}");
				}
				if (pair.Value.AltName != null) {
					sb.Append($", /{pair.Value.AltName}");
				}
				sb.AppendLine();
				sb.AppendLine($"  {pair.Value.Desc}");
				var optinfo = _option_table[pair.Key];
				for (int i = 0; i < optinfo.Length; ++i) {
					sb.Append($"  -{optinfo[i].LongName}");
					if (pair.Value.ShortName != null) {
						sb.Append($", /{optinfo[i].ShortName}");
					}
					if (pair.Value.AltName != null) {
						sb.Append($", /{optinfo[i].AltName}");
					}
					sb.AppendLine();
					sb.AppendLine($"    {optinfo[i].Desc}");
				}
			}
			return sb.ToString();
		}

		internal string GetVersionText(string libver, string libcn, string libcopy, string appver, string appcn, string appcopy)
		{
			var sb = new StringBuilder();
			sb.AppendLine($"app: [v{libver}, cn:{libcn}]");
			sb.AppendLine($"app: {libcopy}");
			sb.AppendLine($"lib: [v{libver}, cn:{libcn}]");
			sb.AppendLine($"lib: {libcopy}");
			return sb.ToString();
		}

		private class SwitchOptionInfo
		{
			public string  LongName  { get; }
			public string? ShortName { get; }
			public string? AltName   { get; }
			public string  Desc      { get; }

			public SwitchOptionInfo(string longName, string? shortName, string? altName, string desc)
			{
				this.LongName  = longName;
				this.ShortName = shortName;
				this.AltName   = altName;
				this.Desc      = desc;
			}
		}
	}
}
