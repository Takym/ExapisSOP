/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace ExapisSOP.IO
{
	/// <summary>
	///  <see cref="ExapisSOP.IO.PathString"/>の書式設定を行います。
	/// </summary>
	public class PathStringFormatter : IFormatProvider, ICustomFormatter
	{
		private readonly IFormatProvider? _provider;

		/// <summary>
		///  既定のカルチャを利用して、
		///  型'<see cref="ExapisSOP.IO.PathStringFormatter"/>'の新しいインスタンスを生成します。
		/// </summary>
		public PathStringFormatter()
		{
			_provider = CultureInfo.CurrentCulture;
		}

		/// <summary>
		///  既定の書式設定サービスを提供する事ができるオブジェクトを指定して、
		///  型'<see cref="ExapisSOP.IO.PathStringFormatter"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="formatProvider">書式設定プロバイダです。</param>
		public PathStringFormatter(IFormatProvider formatProvider)
		{
			_provider = formatProvider;
		}

		/// <summary>
		///  指定された型の書式設定サービスを取得します。
		/// </summary>
		/// <param name="formatType">書式設定サービスの種類です。</param>
		/// <returns>書式設定サービスを表すオブジェクトです。</returns>
		public virtual object? GetFormat(Type? formatType)
		{
			if (formatType?.IsAssignableFrom(this.GetType()) ?? false) {
				return this;
			} else {
				return _provider?.GetFormat(formatType);
			}
		}

		/// <summary>
		///  指定されたオブジェクトを書式設定し文字列へ変換します。
		/// </summary>
		/// <remarks>
		///  記号対応表：
		///  <list type="bullet">
		///   <listheader>
		///    <term>記号</term>
		///    <description>変換結果</description>
		///   </listheader>
		///   <item>
		///    <term>B</term>
		///    <description>基底パス。</description>
		///   </item>
		///   <item>
		///    <term>D</term>
		///    <description>親ディレクトリ名。</description>
		///   </item>
		///   <item>
		///    <term>F</term>
		///    <description>拡張子を含むファイル名。</description>
		///   </item>
		///   <item>
		///    <term>N</term>
		///    <description>拡張子を除くファイル名。</description>
		///   </item>
		///   <item>
		///    <term>O</term>
		///    <description>コンストラクタに渡されたパス文字列。</description>
		///   </item>
		///   <item>
		///    <term>P</term>
		///    <description>絶対パス。</description>
		///   </item>
		///   <item>
		///    <term>R</term>
		///    <description>ルートディレクトリ。</description>
		///   </item>
		///   <item>
		///    <term>U</term>
		///    <description>URI形式のパス文字列。</description>
		///   </item>
		///   <item>
		///    <term>X</term>
		///    <description>拡張子。</description>
		///   </item>
		///   <item>
		///    <term>/</term>
		///    <description>パス区切り記号。</description>
		///   </item>
		///   <item>
		///    <term>\</term>
		///    <description>次の文字を無視。</description>
		///   </item>
		///  </list>
		/// </remarks>
		/// <param name="format">書式設定文字列です。</param>
		/// <param name="arg">文字列へ変換するオブジェクトです。</param>
		/// <param name="formatProvider">書式設定サービスを提供する書式設定プロバイダです。</param>
		/// <returns>現在のパス文字列を表す可読な文字列です。</returns>
		public virtual string Format(string? format, object? arg, IFormatProvider? formatProvider)
		{
			if (arg is PathString path) {
				format ??= string.Empty;
				if (format.Length == 0) {
					return path.ToString();
				}
				var  formatted = new StringBuilder();
				bool ignore    = false;
				for (int i = 0; i < format.Length; ++i) {
					if (ignore) {
						formatted.Append(format[i]);
						ignore = false;
					} else {
						switch (format[i]) {
						case 'B':
							formatted.Append(path.BasePath);
							break;
						case 'D':
							formatted.Append(path.GetDirectoryName()?.GetFileName());
							break;
						case 'F':
							formatted.Append(path.GetFileName());
							break;
						case 'N':
							formatted.Append(path.GetFileNameWithoutExtension());
							break;
						case 'O':
							formatted.Append(path.GetOriginalString());
							break;
						case 'P':
							formatted.Append(path);
							break;
						case 'R':
							formatted.Append(path.GetRootPath());
							break;
						case 'U':
							formatted.Append(path.AsUri().AbsoluteUri);
							break;
						case 'X':
							formatted.Append(path.GetExtension());
							break;
						case '/':
							formatted.Append(Path.DirectorySeparatorChar);
							break;
						case '\\':
							ignore = true;
							break;
						default:
							formatted.Append(format[i]);
							break;
						}
					}
				}
				return formatted.ToString();
			} else {
				if (arg is IFormattable formattable) {
					return formattable.ToString(format, formatProvider ?? _provider) ?? string.Empty;
				} else {
					return arg?.ToString() ?? string.Empty;
				}
			}
		}
	}
}
