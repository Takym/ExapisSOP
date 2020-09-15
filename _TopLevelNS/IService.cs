/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System.Threading.Tasks;

#if NET48
using System;
using System.Collections.Generic;
using System.Reflection;
#endif

namespace ExapisSOP
{
	/// <summary>
	///  サービスを表します。
	/// </summary>
	public interface IService
	{
		/// <summary>
		///  非同期でサービスを初期化します。
		///  この関数内で例外を発生させてはいけません。
		/// </summary>
		/// <param name="context">文脈情報です。</param>
		/// <returns>サービスの初期化処理を格納した非同期操作です。</returns>
		Task InitializeAsync(IContext context);

		/// <summary>
		///  非同期でサービスを破棄します。
		///  この関数内で例外を発生させてはいけません。
		/// </summary>
		/// <param name="context">文脈情報です。</param>
		/// <returns>サービスの破棄処理を格納した非同期操作です。</returns>
		Task FinalizeAsync(IContext context);

#if NETCOREAPP3_1
		/// <summary>
		///  サービスの初期化処理を同期的に実行します。
		///  この関数内で例外を発生させてはいけません。
		/// </summary>
		/// <param name="context">文脈情報です。</param>
		public void Initialize(IContext context)
		{
			this.InitializeAsync(context).Wait();
		}

		/// <summary>
		///  サービスの破棄処理を同期的に実行します。
		///  この関数内で例外を発生させてはいけません。
		/// </summary>
		/// <param name="context">文脈情報です。</param>
		public void Finalize(IContext context)
		{
			this.FinalizeAsync(context).Wait();
		}
#endif
	}

#if NET48
	/// <summary>
	///  .NET Framework用に<see cref="ExapisSOP.IService"/>を拡張します。
	///  このクラスは静的クラスです。
	/// </summary>
	/// <remarks>
	///  .NET Framework で既定のインターフェース実装の呼び出しと互換性を持つコードを書く為のクラスです。
	///  互換性を保つ為にこのクラスから直接呼び出す代わりに拡張メソッドを利用してください。
	/// </remarks>
	[Obsolete("代わりに拡張メソッドを利用してください。", true)]
	public static class NetframeworkIServiceExtensions
	{
		private delegate void ServiceMethod(IContext context);

		private static readonly Dictionary<Type, ServiceMethod?> _cache_i = new Dictionary<Type, ServiceMethod?>();
		private static readonly Dictionary<Type, ServiceMethod?> _cache_f = new Dictionary<Type, ServiceMethod?>();

		private static ServiceMethod? GetServiceMethod(IService obj, Type type, string name, Dictionary<Type, ServiceMethod?> cache)
		{
			if (cache.ContainsKey(type)) {
				return cache[type];
			} else {
				try {
					var m = type.GetMethod(
						name, BindingFlags.Instance | BindingFlags.Public,
						null, new Type[] { typeof(IContext) }, null
					);
					var result = m?.CreateDelegate(typeof(ServiceMethod), obj) as ServiceMethod;
					cache.Add(type, result);
					return result;
				} catch {
					return null;
				}
			}
		}

		private static ServiceMethod? GetInitializeMethod(IService service)
		{
			return GetServiceMethod(service, service.GetType(), nameof(Initialize), _cache_i);
		}

		private static ServiceMethod? GetFinalizeMethod(IService service)
		{
			return GetServiceMethod(service, service.GetType(), nameof(Finalize), _cache_f);
		}

		/// <summary>
		///  サービスの初期化処理を同期的に実行します。
		///  この関数内で例外を発生させてはいけません。
		/// </summary>
		/// <param name="service">初期化するサービスオブジェクトです。</param>
		/// <param name="context">文脈情報です。</param>
		public static void Initialize(this IService service, IContext context)
		{
			var m = GetInitializeMethod(service);
			if (m == null) {
				service.InitializeAsync(context).Wait();
			} else {
				m(context);
			}
		}

		/// <summary>
		///  サービスの破棄処理を同期的に実行します。
		///  この関数内で例外を発生させてはいけません。
		/// </summary>
		/// <param name="service">破棄するサービスオブジェクトです。</param>
		/// <param name="context">文脈情報です。</param>
		public static void Finalize(this IService service, IContext context)
		{
			var m = GetFinalizeMethod(service);
			if (m == null) {
				service.FinalizeAsync(context).Wait();
			} else {
				m(context);
			}
		}
	}
#endif
}
