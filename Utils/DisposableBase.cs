/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
using System.Reflection;

namespace ExapisSOP.Utils
{
	/// <summary>
	///  破棄可能なオブジェクトの基底クラスです。
	/// </summary>
	public abstract class DisposableBase : IDisposable
	{
		/// <summary>
		///  このオブジェクトが破棄されている場合は<see langword="true"/>、有効な場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsDisposed { get; private set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.Utils.DisposableBase"/>'の新しいインスタンスを生成します。
		/// </summary>
		public DisposableBase()
		{
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
		///  現在のオブジェクトインスタンスと利用しているリソースを破棄します。
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

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
				this.IsDisposed = true;
			}
		}

		/// <summary>
		///  現在のインスタンスが破棄されている場合に例外を発生させます。
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
			try {
				var t = obj.GetType();
				if (obj is IDisposable disp) {
					disp.Dispose();
				} else {
					var mi = obj.GetType().GetMethod(
						nameof(Finalize),
						BindingFlags.NonPublic    |
						BindingFlags.InvokeMethod |
						BindingFlags.Instance);
					mi?.Invoke(obj, null);
					GC.SuppressFinalize(obj);
				}
			} catch (Exception e) {
				throw new Exception(e.Message, e);
			}
		}
	}
}
