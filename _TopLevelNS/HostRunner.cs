/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Threading.Tasks;
using ExapisSOP.Properties;

namespace ExapisSOP
{
	/// <summary>
	///  プログラムの実行環境を表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class HostRunner
	{
		/// <summary>
		///  構成設定を実際に設定する非同期コールバック関数です。
		/// </summary>
		protected Func<IConfiguration, Task>? ConfigureCallBackFunc { get; private set; }

		/// <summary>
		///  構成設定を現在の実行環境に対して設定します。
		/// </summary>
		/// <param name="callBackFunc">実際に設定を行う非同期コールバック関数です。</param>
		/// <returns>このインスタンスを返します。</returns>
		public virtual HostRunner Configure(Func<IConfiguration, Task> callBackFunc)
		{
			this.ConfigureCallBackFunc = callBackFunc;
			return this;
		}

#pragma warning disable CS1998 // 非同期メソッドは、'await' 演算子がないため、同期的に実行されます
		/// <summary>
		///  構成設定を現在の実行環境に対して設定します。
		/// </summary>
		/// <param name="callBackAction">
		///  実際に設定を行うコールバック関数です。
		///  内部で非同期操作に変換されます。
		/// </param>
		/// <returns>このインスタンスを返します。</returns>
		public HostRunner Configure(Action<IConfiguration> callBackAction)
		{
			return this.Configure(async (config) => callBackAction(config));
		}
#pragma warning restore CS1998 // 非同期メソッドは、'await' 演算子がないため、同期的に実行されます

		/// <summary>
		///  オーバーライドされた場合、プログラムの実行を非同期で開始します。
		/// </summary>
		/// <returns>OSへの戻り値を含む非同期操作です。</returns>
		public abstract Task<int> RunAsync();

		/// <summary>
		///  プログラムの実行を同期的に開始します。
		/// </summary>
		/// <returns>OSへの戻り値です。</returns>
		public int Run()
		{
			return this.RunAsync().Result;
		}

		/// <summary>
		///  新しい既定の種類のプログラムの実行環境を生成します。
		/// </summary>
		/// <param name="args">OSから渡されたコマンド行引数です。</param>
		/// <returns>新しく生成された実行環境を表すオブジェクトです。</returns>
		public static HostRunner? Create(params string[] args)
		{
			return Create<HostRunner>(args);
		}

		/// <summary>
		///  指定した種類のプログラムの実行環境を生成します。
		/// </summary>
		/// <param name="args">OSから渡されたコマンド行引数です。</param>
		/// <typeparam name="T">実行環境を表すクラスです。文字列配列を受け入れるコンストラクタを持っている必要があります。</typeparam>
		/// <returns>新しく生成された実行環境を表すオブジェクトです。</returns>
		/// <exception cref="System.ArgumentException" />
		/// <exception cref="System.MemberAccessException" />
		/// <exception cref="System.MissingMemberException" />
		/// <exception cref="System.TypeLoadException" />
		/// <exception cref="System.InvalidOperationException" />
		public static T? Create<T>(params string[] args) where T: HostRunner
		{
			try {
				return ((T?)(Activator.CreateInstance(typeof(T), new object[] { args })));
			} catch (ArgumentException ae) when (ae.ParamName == "type") {
				throw new ArgumentException(Resources.HostRunner_Create_ArgumentException, nameof(T), ae);
			} catch (NotSupportedException nse) {
				throw new ArgumentException(Resources.HostRunner_Create_ArgumentException, nameof(T), nse);
			} catch (MethodAccessException mae) {
				throw new MemberAccessException(mae.Message, mae);
			} catch (MissingMethodException mme) {
				throw new MissingMemberException(mme.Message, mme);
			} catch (TypeLoadException) {
				throw;
			} catch (Exception e) {
				throw new InvalidOperationException(Resources.HostRunner_Create_InvalidOperationException, e);
			}
		}
	}
}
