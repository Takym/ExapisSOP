/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;

#if NET48
using System.Collections.Generic;
using System.Reflection;
#endif

namespace ExapisSOP.NativeWrapper
{
	/// <summary>
	///  環境に依存した方法でネイティブコードを呼び出す機能を提供します。
	/// </summary>
	public interface INativeCaller
	{
		/// <summary>
		///  ネイティブコードの呼び出しが実行できる環境かどうか判定します。
		/// </summary>
		/// <param name="reason">サポートされない場合、その理由を表す例外オブジェクトを返します。</param>
		/// <returns>サポートされる場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		bool IsSupported(out PlatformNotSupportedException? reason);

#if NETCOREAPP3_1
		/// <summary>
		///  ネイティブコードの呼び出しが実行できる環境かどうか判定します。
		/// </summary>
		/// <returns>サポートされる場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public bool IsSupported()
		{
			return this.IsSupported(out _);
		}
#endif
	}

#if NET48
	/// <summary>
	///  .NET Framework用に<see cref="ExapisSOP.NativeWrapper.INativeCaller"/>を拡張します。
	///  このクラスは静的クラスです。
	/// </summary>
	/// <remarks>
	///  .NET Framework で既定のインターフェース実装の呼び出しと互換性を持つコードを書く為のクラスです。
	///  互換性を保つ為にこのクラスから直接呼び出す代わりに拡張メソッドを利用してください。
	/// </remarks>
	[Obsolete("代わりに拡張メソッドを利用してください。", true)]
	public static class NetframeworkINativeCallerExtensions
	{
		private delegate bool Method();

		private static readonly Dictionary<Type, Method?> _cache = new Dictionary<Type, Method?>();

		private static Method? GetMethod(INativeCaller obj, Type type)
		{
			if (_cache.ContainsKey(type)) {
				return _cache[type];
			} else {
				try {
					var m = type.GetMethod(
						nameof(IsSupported), BindingFlags.Instance | BindingFlags.Public,
						null, Array.Empty<Type>(), null
					);
					var result = m?.CreateDelegate(typeof(Method), obj) as Method;
					_cache.Add(type, result);
					return result;
				} catch {
					return null;
				}
			}
		}

		/// <summary>
		///  ネイティブコードの呼び出しが実行できる環境かどうか判定します。
		/// </summary>
		/// <param name="nativeCaller">実際の処理を格納したオブジェクトです。</param>
		/// <returns>サポートされる場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。</returns>
		public static bool IsSupported(this INativeCaller nativeCaller)
		{
			var m = GetMethod(nativeCaller, nativeCaller.GetType());
			if (m == null) {
				return nativeCaller.IsSupported(out _);
			} else {
				return m();
			}
		}
	}
#endif
}
