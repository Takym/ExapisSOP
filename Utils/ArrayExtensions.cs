/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExapisSOP.Utils
{
	/// <summary>
	///  <see cref="System.Array"/>の機能を拡張します。
	///  このクラスは静的です。
	/// </summary>
	public static class ArrayExtensions
	{
		/// <summary>
		///  指定された配列を結合し新たな配列を作成します。
		///  この操作は時間がかかる可能性があります。
		/// </summary>
		/// <typeparam name="T">配列の要素型です。</typeparam>
		/// <param name="baseArray">基本となる配列です。</param>
		/// <param name="arrays">結合する配列です。</param>
		/// <returns>結合された新しい配列です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static T[] Join<T>(this T[] baseArray, params T[][] arrays)
		{
			if (baseArray == null) {
				throw new ArgumentNullException(nameof(baseArray));
			}
			if (arrays == null) {
				throw new ArgumentNullException(nameof(arrays));
			}
			var result = new List<T>(baseArray);
			for (int i = 0; i < arrays.Length; ++i) {
				if (arrays[i] == null) {
					throw new ArgumentNullException($"{nameof(arrays)}[{i}]");
				}
				result.AddRange(arrays[i]);
			}
			return result.ToArray();
		}

		/// <summary>
		///  指定されたオブジェクト配列を結合し新たなオブジェクト配列を作成します。
		///  この操作は時間がかかる可能性があります。
		/// </summary>
		/// <param name="baseArray">基本となるオブジェクト配列です。</param>
		/// <param name="arrays">結合するオブジェクト配列です。</param>
		/// <returns>結合された新しいオブジェクト配列です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static object?[] Join(this object?[] baseArray, params object?[][] arrays)
		{
			if (baseArray == null) {
				throw new ArgumentNullException(nameof(baseArray));
			}
			if (arrays == null) {
				throw new ArgumentNullException(nameof(arrays));
			}
			var result = new List<object?>(baseArray);
			for (int i = 0; i < arrays.Length; ++i) {
				if (arrays[i] == null) {
					throw new ArgumentNullException($"{nameof(arrays)}[{i}]");
				}
				result.AddRange(arrays[i]);
			}
			return result.ToArray();
		}

		/// <summary>
		///  指定された配列を結合し新たな配列を作成します。
		/// </summary>
		/// <typeparam name="T">配列の要素型です。</typeparam>
		/// <param name="baseArray">基本となる配列です。</param>
		/// <param name="arrays">結合する配列です。</param>
		/// <returns>結合された新しい配列を含む非同期操作です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static async Task<T[]> JoinAsync<T>(this T[] baseArray, params T[][] arrays)
		{
			if (baseArray == null) {
				throw new ArgumentNullException(nameof(baseArray));
			}
			if (arrays == null) {
				throw new ArgumentNullException(nameof(arrays));
			}
			return await Task.FromResult(baseArray.Join(arrays));
		}

		/// <summary>
		///  指定されたオブジェクト配列を結合し新たなオブジェクト配列を作成します。
		/// </summary>
		/// <param name="baseArray">基本となるオブジェクト配列です。</param>
		/// <param name="arrays">結合するオブジェクト配列です。</param>
		/// <returns>結合された新しいオブジェクト配列を含む非同期操作です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static async Task<object?[]> JoinAsync(this object?[] baseArray, params object?[][] arrays)
		{
			if (baseArray == null) {
				throw new ArgumentNullException(nameof(baseArray));
			}
			if (arrays == null) {
				throw new ArgumentNullException(nameof(arrays));
			}
			return await Task.FromResult(baseArray.Join(arrays));
		}
	}
}
