/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

#if NET48
using System.Text;
using ExapisSOP.Properties;
#endif

namespace ExapisSOP.IO
{
	/// <summary>
	///  パス文字列を表します。
	///  このクラスは継承できません。
	/// </summary>
	/// <remarks>
	///  <see cref="System.IO.Path"/>クラスと併用してください。
	/// </remarks>
	[TypeConverter(typeof(PathStringConverter))]
	public sealed class PathString : IFormattable, IEquatable<PathString?>, IEquatable<string?>, IComparable, IComparable<PathString?>, IComparable<string?>
	{
		private readonly string _org_path;
		private readonly string _path;
		private readonly Uri    _uri;

		/// <summary>
		///  基底のパス文字列を取得します。
		/// </summary>
		/// <remarks>
		///  <see cref="ExapisSOP.IO.PathString.GetDirectoryName"/>と全く同じ動作を行います。
		/// </remarks>
		public PathString? BasePath => this.GetDirectoryName();

		/// <summary>
		///  現在のパス文字列がルートディレクトリを表しているかどうかを判定します。
		/// </summary>
		public bool IsRoot => this == this.GetRootPath();

		/// <summary>
		///  現在のパス文字列が実際に存在し、ドライブである場合は<see langword="true"/>を返します。
		///  それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsDrive => this.IsRoot && this.IsDirectory;

		/// <summary>
		///  現在のパス文字列が実際に存在し、ディレクトリである場合は<see langword="true"/>を返します。
		///  それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsDirectory => Directory.Exists(_path);

		/// <summary>
		///  現在のパス文字列が実際に存在し、ファイルである場合は<see langword="true"/>を返します。
		///  それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsFile => File.Exists(_path);

		/// <summary>
		///  現在のパス文字列が実際に存在する場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool Exists => this.IsDirectory || this.IsFile;

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.PathString"/>'の新しいインスタンスを生成します。
		/// </summary>
		public PathString() : this(Environment.CurrentDirectory) { }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.PathString"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="path">
		///  新しいインスタンスに設定するパス文字列です。
		///  相対パスの場合、絶対パスへ自動的に変換されます。
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="path"/>が<see langword="null"/>に設定されています。
		/// </exception>
		/// <exception cref="ExapisSOP.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public PathString(string path)
		{
			if (path == null) {
				throw new ArgumentNullException(nameof(path));
			}
			_org_path = path;
			try {
				_path = Path.GetFullPath(path);
				if (_path.EndsWith(Path.DirectorySeparatorChar.ToString())) {
					_path.Remove(_path.Length - 1);
				}
				_uri = new Uri(_path);
			} catch (ArgumentException ae) {
				throw new InvalidPathFormatException(path, ae);
			} catch (NotSupportedException nse) {
				throw new InvalidPathFormatException(path, nse);
			} catch (PathTooLongException ptle) {
				throw new InvalidPathFormatException(path, ptle);
			} catch (UriFormatException ufe) {
				throw new InvalidPathFormatException(path, ufe);
			}
		}

		/// <summary>
		///  指定されたパスを現在のパスと結合します。
		/// </summary>
		/// <param name="path">結合するパス文字列です。</param>
		/// <returns>結合された新しいパス文字列、または、<paramref name="path"/>が空の場合は現在のインスタンスを返します。</returns>
		/// <exception cref="ExapisSOP.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public PathString Combine(string path)
		{
			if (string.IsNullOrEmpty(path)) {
				return this;
			}
			return new PathString(Path.Combine(_path, path!));
		}

		/// <summary>
		///  指定された2つのパスを現在のパスと結合します。
		/// </summary>
		/// <param name="path1">結合する1つ目のパス文字列です。</param>
		/// <param name="path2">結合する2つ目のパス文字列です。</param>
		/// <returns>結合された新しいパス文字列、または、指定された全てのパスが空の場合は現在のインスタンスを返します。</returns>
		/// <exception cref="ExapisSOP.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public PathString Combine(string path1, string path2)
		{
			if (string.IsNullOrEmpty(path1) && string.IsNullOrEmpty(path2)) {
				return this;
			}
			return new PathString(Path.Combine(_path, path1 ?? string.Empty, path2 ?? string.Empty));
		}

		/// <summary>
		///  指定された3つのパスを現在のパスと結合します。
		/// </summary>
		/// <param name="path1">結合する1つ目のパス文字列です。</param>
		/// <param name="path2">結合する2つ目のパス文字列です。</param>
		/// <param name="path3">結合する3つ目のパス文字列です。</param>
		/// <returns>結合された新しいパス文字列、または、指定された全てのパスが空の場合は現在のインスタンスを返します。</returns>
		/// <exception cref="ExapisSOP.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public PathString Combine(string path1, string path2, string path3)
		{
			if (string.IsNullOrEmpty(path1) && string.IsNullOrEmpty(path2) && string.IsNullOrEmpty(path3)) {
				return this;
			}
			return new PathString(Path.Combine(_path, path1 ?? string.Empty, path2 ?? string.Empty, path3 ?? string.Empty));
		}

		/// <summary>
		///  複数のパスを一つのパスに結合します。
		/// </summary>
		/// <param name="paths">結合する複数のパス文字列です。</param>
		/// <returns>結合された新しいパス文字列、または、<paramref name="paths"/>が空の場合は現在のインスタンスを返します。</returns>
		/// <exception cref="ExapisSOP.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public PathString Combine(params string?[]? paths)
		{
			if (paths == null || paths.Length == 0) {
				return this;
			} else {
				var paths2 = new List<string>(paths.Length + 1);
				paths2.Add(_path);
				for (int i = 0; i < paths.Length; ++i) {
					if (!string.IsNullOrEmpty(paths[i])) {
						paths2.Add(paths[i]!);
					}
				}
				return new PathString(Path.Combine(paths2.ToArray()));
			}
		}

		/// <summary>
		///  現在のパス文字列からディレクトリ情報を取得します。
		/// </summary>
		/// <returns>
		///  現在のパス文字列のディレクトリ情報を返します。
		///  ディレクトリ情報が存在しない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.Security.SecurityException" />
		public PathString? GetDirectoryName()
		{
			string? baseDir = Path.GetDirectoryName(_path);
			if (string.IsNullOrEmpty(baseDir)) {
				return null;
			}
			return new PathString(baseDir);
		}

		/// <summary>
		///  現在のパス文字列からファイル名情報を取得します。
		/// </summary>
		/// <returns>
		///  現在のパス文字列の拡張子を含むファイル名を返します。
		///  ファイル名情報が存在しない場合は<see langword="null"/>を返します。
		/// </returns>
		public string? GetFileName()
		{
			string? fname = Path.GetFileName(_path);
			if (string.IsNullOrEmpty(fname)) {
				return null;
			}
			return fname;
		}

		/// <summary>
		///  現在のパス文字列から拡張子を除くファイル名情報を取得します。
		/// </summary>
		/// <returns>
		///  現在のパス文字列の拡張子を含まないファイル名を返します。
		///  ファイル名情報が存在しない場合は<see langword="null"/>を返します。
		/// </returns>
		public string? GetFileNameWithoutExtension()
		{
			string? fname = Path.GetFileNameWithoutExtension(_path);
			if (string.IsNullOrEmpty(fname)) {
				return null;
			}
			return fname;
		}

		/// <summary>
		///  現在のパス文字列から拡張子情報を取得します。
		/// </summary>
		/// <returns>
		///  現在のパス文字列のピリオド付きの拡張子情報を返します。
		///  拡張子情報が存在しない場合は<see langword="null"/>を返します。
		/// </returns>
		public string? GetExtension()
		{
			string? ext = Path.GetExtension(_path);
			if (string.IsNullOrEmpty(ext)) {
				return null;
			}
			return ext;
		}

		/// <summary>
		///  パス文字列の拡張子を含むファイル名を変更します。
		/// </summary>
		/// <param name="filename">変更後の拡張子を含むファイル名、または、親ディレクトリを取得する場合は空値を指定してください。</param>
		/// <returns>
		///  ファイル名が変更されたパス文字列、または、
		///  ファイル名が変更されなかった場合は現在のインスタンスを返します。
		///  親ディレクトリの情報が存在しない場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="ExapisSOP.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public PathString? ChangeFileName(string? filename)
		{
			var dir = this.GetDirectoryName();
			if (dir is null) {
				return null;
			} else if (string.IsNullOrEmpty(filename)) {
				return dir;
			} else {
				var newpath = dir + filename;
				if (newpath == this) {
					return this;
				} else {
					return newpath;
				}
			}
		}

		/// <summary>
		///  パス文字列の拡張子を変更します。
		/// </summary>
		/// <param name="extension">変更後の拡張子、または、拡張子を削除する場合は空値を指定してください。</param>
		/// <returns>
		///  拡張子が変更されたパス文字列、または、
		///  拡張子が変更されなかった場合は現在のインスタンスを返します。
		/// </returns>
		/// <exception cref="ExapisSOP.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.ArgumentException" />
		/// <exception cref="System.Security.SecurityException" />
		public PathString ChangeExtension(string? extension)
		{
			string newpath = Path.ChangeExtension(_path, extension);
			if (newpath == _path) {
				return this;
			}
			return new PathString(newpath);
		}

		/// <summary>
		///  現在のパス文字列の拡張子を変更し実際に存在しないパス文字列を取得します。
		/// </summary>
		/// <returns>新しい実際にファイルまたはディレクトリが存在しないパス文字列です。</returns>
		/// <exception cref="System.Security.SecurityException" />
		public PathString EnsureNotFound()
		{
			int i = 0;
			var path = this;
			string? ext = this.GetExtension();
			while (path.Exists) {
				path = this.ChangeExtension(++i + ext);
			}
			return path;
		}

		/// <summary>
		///  現在のパス文字列からルートディレクトリ情報を取得します。
		/// </summary>
		/// <returns>
		///  現在のパス文字列のルートディレクトリ情報を返します。
		///  ルートディレクトリ情報が存在しない場合は<see langword="null"/>を返します。
		///  現在のパス文字列がルートディレクトリを指し示す場合は現在のインスタンスを返します。
		/// </returns>
		/// <exception cref="System.Security.SecurityException" />
		public PathString? GetRootPath()
		{
			string? root = Path.GetPathRoot(_path);
			if (string.IsNullOrEmpty(root)) {
				return null;
			}
			if (root == _path) {
				return this;
			}
			return new PathString(root);
		}

		/// <summary>
		///  現在の作業ディレクトリを基にした相対パスを取得します。
		/// </summary>
		/// <returns>現在のパスへの相対パスを表す文字列です。</returns>
		/// <exception cref="System.PlatformNotSupportedException" />
		public string? GetRelativePath()
		{
			return this.GetRelativePath(new PathString());
		}

		/// <summary>
		///  指定したパスを基にした相対パスを取得します。
		/// </summary>
		/// <param name="relativeTo">相対パスの基底となる絶対パスです。</param>
		/// <returns>現在のパスへの相対パスを表す文字列です。</returns>
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="relativeTo"/>が<see langword="null"/>に設定されています。
		/// </exception>
		/// <exception cref="System.PlatformNotSupportedException">
		///  .NET Framework v4.8 上で相対パスの生成に失敗した場合に発生します。
		///  .NET Core 3.1 以上のランタイムで実行し直してください。
		/// </exception>
		public string? GetRelativePath(PathString relativeTo)
		{
			if (relativeTo == null) {
				throw new ArgumentNullException(nameof(relativeTo));
			}
#if NETCOREAPP3_1
			return Path.GetRelativePath(relativeTo._path, _path);
#elif NET48
			try {
				string[] tp =            _path.Split(Path.DirectorySeparatorChar);
				string[] bp = relativeTo._path.Split(Path.DirectorySeparatorChar);
				var rp = new StringBuilder();
				int i = 0;
				while (i < tp.Length && i < bp.Length && tp[i] == bp[i]) ++i;
				int j = i;
				for (; i < bp.Length; ++i) {
					rp.Append("..\\");
				}
				for (; j < tp.Length; ++j) {
					rp.Append(tp[j]).Append("\\");
				}
				rp.Remove(rp.Length - 1, 1);
				return rp.ToString();
			} catch (Exception e) {
				throw new PlatformNotSupportedException(Resources.PathString_PlatformNotSupportedException, e);
			}
#endif
		}

		/// <summary>
		///  コンストラクタに渡されたパス文字列を取得します。
		/// </summary>
		/// <returns>コンストラクタに渡されたパス文字列を返します。</returns>
		public string GetOriginalString()
		{
			return _org_path;
		}

		/// <summary>
		///  パス文字列をURIへ変換します。
		/// </summary>
		/// <returns><see cref="System.Uri"/>形式のオブジェクトです。</returns>
		public Uri AsUri()
		{
			return _uri;
		}

		/// <summary>
		///  パス文字列を可読な文字列へ変換します。
		/// </summary>
		/// <returns>現在のパス文字列を表す可読な文字列です。</returns>
		public override string ToString()
		{
			return _path;
		}

		/// <summary>
		///  書式設定を利用してパス文字列を可読な文字列へ変換します。
		/// </summary>
		/// <remarks>
		///  書式設定文字列(<paramref name="format"/>)の指定方法は、
		///  <see cref="ExapisSOP.IO.PathStringFormatter.Format(string?, object?, IFormatProvider?)"/>の
		///  説明を確認してください。
		/// </remarks>
		/// <param name="format">書式設定文字列です。</param>
		/// <returns>現在のパス文字列を表す可読な文字列です。</returns>
		public string ToString(string? format)
		{
			return this.ToString(format, null);
		}

		/// <summary>
		///  書式設定を利用してパス文字列を可読な文字列へ変換します。
		/// </summary>
		/// <param name="formatProvider">書式設定サービスを提供する書式設定プロバイダです。</param>
		/// <returns>現在のパス文字列を表す可読な文字列です。</returns>
		public string ToString(IFormatProvider? formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		/// <summary>
		///  書式設定を利用してパス文字列を可読な文字列へ変換します。
		/// </summary>
		/// <remarks>
		///  書式設定文字列(<paramref name="format"/>)の指定方法は、
		///  <see cref="ExapisSOP.IO.PathStringFormatter.Format(string?, object?, IFormatProvider?)"/>の
		///  説明を確認してください。
		/// </remarks>
		/// <param name="format">書式設定文字列です。</param>
		/// <param name="formatProvider">書式設定サービスを提供する書式設定プロバイダです。</param>
		/// <returns>現在のパス文字列を表す可読な文字列です。</returns>
		public string ToString(string? format, IFormatProvider? formatProvider)
		{
			formatProvider ??= new PathStringFormatter();
			var formatter = formatProvider.GetFormat(typeof(PathStringFormatter)) as PathStringFormatter;
			if (formatter == null) {
				return this.ToString();
			} else {
				return formatter.Format(format, this, formatProvider);
			}
		}

		/// <summary>
		///  指定したオブジェクトインスタンスの値と現在のインスタンスの値が等価かどうか判定します。
		/// </summary>
		/// <param name="obj">判定対象のオブジェクトです。</param>
		/// <returns>等しい場合は<see langword="true"/>、等しくない場合は<see langword="false"/>を返します。</returns>
		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(this, obj)) {
				return true;
			} else if (obj is PathString path) {
				return this.Equals(path);
			} else if (obj is string text) {
				return this.Equals(text);
			} else {
				return base.Equals(obj);
			}
		}

		/// <summary>
		///  指定したパス文字列と現在のパス文字列が等価かどうか判定します。
		/// </summary>
		/// <param name="other">判定対象のパス文字列です。</param>
		/// <returns>等しい場合は<see langword="true"/>、等しくない場合は<see langword="false"/>を返します。</returns>
		public bool Equals(PathString? other)
		{
			return _path == other?._path;
		}

		/// <summary>
		///  指定した文字列と現在のパス文字列が等価かどうか判定します。
		/// </summary>
		/// <param name="other">判定対象の文字列です。</param>
		/// <returns>等しい場合は<see langword="true"/>、等しくない場合は<see langword="false"/>を返します。</returns>
		public bool Equals(string? other)
		{
			return _path == other || _org_path == other;
		}

		/// <summary>
		///  指定したオブジェクトインスタンスの値と現在のインスタンスの値を比較します。
		/// </summary>
		/// <param name="obj">比較対象のオブジェクトです。</param>
		/// <returns>
		///  等価の場合は<code>0</code>、
		///  現在のインスタンスの方が大きい場合は正の値、
		///  現在のインスタンスの方が小さい場合は負の値を返します。
		/// </returns>
		public int CompareTo(object? obj)
		{
			if (obj is PathString path) {
				return this.CompareTo(path);
			} else if (obj is string text) {
				return this.CompareTo(text);
			} else {
				return _path.CompareTo(null);
			}
		}

		/// <summary>
		///  指定したパス文字列と現在のパス文字列を比較します。
		/// </summary>
		/// <param name="other">比較対象のパス文字列です。</param>
		/// <returns>
		///  等価の場合は<code>0</code>、
		///  現在のインスタンスの方が大きい場合は正の値、
		///  現在のインスタンスの方が小さい場合は負の値を返します。
		/// </returns>
		public int CompareTo(PathString? other)
		{
			return _path.CompareTo(other?._path);
		}

		/// <summary>
		///  指定した文字列と現在のパス文字列を比較します。
		/// </summary>
		/// <param name="other">比較対象の文字列です。</param>
		/// <returns>
		///  等価の場合は<code>0</code>、
		///  現在のインスタンスの方が大きい場合は正の値、
		///  現在のインスタンスの方が小さい場合は負の値を返します。
		/// </returns>
		public int CompareTo(string? other)
		{
			return _path.CompareTo(other);
		}

		/// <summary>
		///  現在のパス文字列のハッシュコードを取得します。
		/// </summary>
		/// <returns>現在のパス文字列が格納している文字列のハッシュ値を返します。</returns>
		public override int GetHashCode()
		{
			return _path.GetHashCode();
		}

		/// <summary>
		///  指定された二つのパス文字列を結合します。
		/// </summary>
		/// <param name="left">基底パスです。</param>
		/// <param name="right">相対パスです。</param>
		/// <returns>結合されたパス文字列です。</returns>
		/// <exception cref="ExapisSOP.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public static PathString operator +(PathString left, string? right)
			=> left.Combine(right ?? string.Empty);

		/// <summary>
		///  <paramref name="right"/>を基にした<paramref name="left"/>の相対パスを計算します。
		/// </summary>
		/// <param name="left">絶対パスです。</param>
		/// <param name="right">基底パスです。</param>
		/// <returns><paramref name="left"/>への相対パスです。</returns>
		/// <exception cref="System.PlatformNotSupportedException" />
		public static string? operator -(PathString left, PathString? right)
			=> left.GetRelativePath(right ?? new PathString());

		/// <summary>
		///  指定された二つのパス文字列が等価かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値です。</param>
		/// <param name="right">右辺の値です。</param>
		/// <returns>等しい場合は<see langword="true"/>、等しくない場合は<see langword="false"/>を返します。</returns>
		public static bool operator ==(PathString left, PathString? right)
			=> left.Equals(right);

		/// <summary>
		///  指定された二つのパス文字列が不等価かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値です。</param>
		/// <param name="right">右辺の値です。</param>
		/// <returns>等しい場合は<see langword="false"/>、等しくない場合は<see langword="true"/>を返します。</returns>
		public static bool operator !=(PathString left, PathString? right)
			=> !left.Equals(right);

		/// <summary>
		///  左辺が右辺未満かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値です。</param>
		/// <param name="right">右辺の値です。</param>
		/// <returns>左辺の方が右辺より小さい場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public static bool operator <(PathString left, PathString? right)
			=> left.CompareTo(right) < 0;

		/// <summary>
		///  左辺が右辺以下かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値です。</param>
		/// <param name="right">右辺の値です。</param>
		/// <returns>左辺の方が右辺より小さいか等しい場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public static bool operator <=(PathString left, PathString? right)
			=> left.CompareTo(right) <= 0;

		/// <summary>
		///  左辺が右辺超過かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値です。</param>
		/// <param name="right">右辺の値です。</param>
		/// <returns>左辺の方が右辺より大きい場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public static bool operator >(PathString left, PathString? right)
			=> left.CompareTo(right) > 0;

		/// <summary>
		///  左辺が右辺以上かどうか判定します。
		/// </summary>
		/// <param name="left">左辺の値です。</param>
		/// <param name="right">右辺の値です。</param>
		/// <returns>左辺の方が右辺より大きいか等しい場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public static bool operator >=(PathString left, PathString? right)
			=> left.CompareTo(right) >= 0;

		/// <summary>
		///  パス文字列を通常の文字列へ暗黙的に変換(キャスト)します。
		/// </summary>
		/// <param name="path">通常の文字列へ変換するパス文字列です。</param>
		public static implicit operator string(PathString path) => path._path;

		/// <summary>
		///  通常の文字列をパス文字列へ明示的に変換(キャスト)します。
		/// </summary>
		/// <param name="path">パス文字列へ変換する通常の文字列です。</param>
		/// <exception cref="System.ArgumentNullException">
		///  <paramref name="path"/>が<see langword="null"/>に設定されています。
		/// </exception>
		/// <exception cref="ExapisSOP.IO.InvalidPathFormatException">
		///  無効なパス文字列が渡されました。
		/// </exception>
		/// <exception cref="System.Security.SecurityException" />
		public static explicit operator PathString(string path) => new PathString(path);
	}
}
