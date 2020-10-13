/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace ExapisSOP.Utils
{
	/// <summary>
	///  破棄可能なオブジェクトの基底クラスです。
	/// </summary>
	public abstract class DisposableBase : IDisposable
#if NETCOREAPP3_1
		, IAsyncDisposable
#endif
	{
		private readonly List<object> _objs;

		/// <summary>
		///  破棄可能なオブジェクトを格納したリストを取得します。
		/// </summary>
		protected IList<object> DisposableObjects => _objs;

		/// <summary>
		///  このオブジェクトが破棄されている場合は<see langword="true"/>、有効な場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsDisposed { get; private set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.Utils.DisposableBase"/>'の新しいインスタンスを生成します。
		/// </summary>
		public DisposableBase()
		{
			_objs = new List<object>();
			this.IsDisposed = false;
		}

		/// <summary>
		///  型'<see cref="ExapisSOP.Utils.DisposableBase"/>'の現在のインスタンスを破棄します。
		/// </summary>
		~DisposableBase()
		{
			this.Dispose(false);
		}

		/// <summary>
		///  現在のインスタンスと<see cref="ExapisSOP.Utils.DisposableBase.DisposableObjects"/>が破棄されていない事を確認します。
		///  実行速度を優先する場合は<see cref="ExapisSOP.Utils.DisposableBase.ThrowOnObjectDisposed"/>を呼び出してください。
		/// </summary>
		/// <remarks>
		///  論理値を返す公開された<c>IsDisposed</c>が実装されていない場合、判定する事はできません。
		/// </remarks>
		/// <exception cref="System.ObjectDisposedException" />
		[DebuggerHidden()]
		[StackTraceHidden()]
		protected void EnsureNotDisposed()
		{
			this.ThrowOnObjectDisposed();
			for (int i = 0; i < _objs.Count; ++i) {
				if (_objs[i] is DisposableBase disp) {
					disp.EnsureNotDisposed();
				} else {
					var t = _objs[i]?.GetType();
					if (t != null) {
						var m = t.GetMember(nameof(this.IsDisposed));
						for (int j = 0; j < m.Length; ++j) {
							if ((m[j] is FieldInfo    fi && fi.FieldType    == typeof(bool) && (fi.GetValue(_objs[i])       as bool? ?? false)) ||
								(m[j] is PropertyInfo pi && pi.PropertyType == typeof(bool) && (pi.GetValue(_objs[i])       as bool? ?? false)) ||
								(m[j] is MethodInfo   mi && mi.ReturnType   == typeof(bool) && (mi.Invoke  (_objs[i], null) as bool? ?? false))) {
								throw new ObjectDisposedException(t.Name);
							}
						}
					}
				}
			}
		}

		/// <summary>
		///  現在のインスタンスが破棄されている場合に例外を発生させます。
		///  堅牢性を優先する場合は<see cref="ExapisSOP.Utils.DisposableBase.EnsureNotDisposed"/>を呼び出してください。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException" />
		[DebuggerHidden()]
		[StackTraceHidden()]
		protected void ThrowOnObjectDisposed()
		{
			if (this.IsDisposed) {
				throw new ObjectDisposedException(this.GetType().Name);
			}
		}

		/// <summary>
		///  現在のオブジェクトインスタンスと利用しているリソースを破棄します。
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

#if NETCOREAPP3_1
		/// <summary>
		///  現在のオブジェクトインスタンスと利用しているリソースを非同期的に破棄します。
		/// </summary>
		public async ValueTask DisposeAsync()
		{
			this.Dispose();
			await Task.CompletedTask;
		}
#endif

		/// <summary>
		///  現在のオブジェクトインスタンスと利用しているリソースを破棄します。
		///  この関数内で例外を発生させてはいけません。
		/// </summary>
		/// <remarks>
		///  この関数を上書きする場合は、必ず基底関数を呼び出してください。
		///  以下の様に実装します：
		///  <code>
		///   protected override void Dispose(disposing)
		///   {
		///     if (!this.IsDisposed) {
		///       if (disposing) {
		///         /* TODO: ここでマネージドオブジェクトを破棄 */
		///       }
		///       /* TODO: ここでアンマネージオブジェクトを破棄 */
		///       base.Dispose(disposing);
		///     }
		///   }
		///  </code>
		/// </remarks>
		/// <param name="disposing">
		///  マネージドオブジェクトとアンマネージオブジェクト両方を破棄する場合は<see langword="true"/>、
		///  アンマネージオブジェクトのみを破棄する場合は<see langword="false"/>を設定します。
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (!this.IsDisposed) {
				if (disposing) {
					for (int i = 0; i < _objs.Count; ++i) {
#pragma warning disable CS0618 // 型またはメンバーが旧型式です
						Delete(_objs[i]);
#pragma warning restore CS0618 // 型またはメンバーが旧型式です
					}
				}
				_objs.Clear();
				_objs.Capacity = 0;
				this.IsDisposed = true;
			}
		}

		/// <summary>
		///  指定したオブジェクトを削除します。
		///  <see cref="System.IDisposable"/>を継承しているクラスの場合、<see cref="System.IDisposable.Dispose"/>を呼び出し、
		///  それ以外のクラスはリフレクションを利用し強制的にデストラクタ(例：<see cref="object.Finalize"/>)を強制的に実行します。
		/// </summary>
		/// <remarks>
		///  この関数を利用して削除したオブジェクトは利用しないでください。
		/// </remarks>
		/// <param name="obj">削除対象のオブジェクトです。</param>
		/// <exception cref="System.Exception">
		///  処理が失敗しました。
		/// </exception>
		[Obsolete("予期せぬ不具合が発生する可能性がある為、この関数は呼び出さないでください。")]
		public static void Delete(object obj)
		{
			if (obj is null) return;
			try {
				switch (obj) {
				case IDisposable disp1:
					disp1.Dispose();
					break;
#if NETCOREAPP3_1
				case IAsyncDisposable disp2:
					// 破棄処理なので完了を待つ必要は無い
					disp2.DisposeAsync();
					break;
#endif
				default:
					var mi = obj.GetType().GetMethod(
						nameof(Finalize),
						BindingFlags.NonPublic    |
						BindingFlags.InvokeMethod |
						BindingFlags.Instance);
					mi?.Invoke(obj, null);
					GC.SuppressFinalize(obj);
					break;
				}
			} catch (Exception e) {
				throw new Exception(e.Message, e);
			}
		}
	}
}
