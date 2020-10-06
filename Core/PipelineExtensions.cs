﻿/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Threading.Tasks;

namespace ExapisSOP.Core
{
	/// <summary>
	///  <see cref="ExapisSOP.IPipeline"/>の機能を拡張します。
	///  このクラスは静的です。
	/// </summary>
	public static class PipelineExtensions
	{
		/// <summary>
		///  指定された処理を表すデリゲートを指定されたパイプラインの末尾に追加します。
		/// </summary>
		/// <param name="pipeline">登録先のパイプラインです。</param>
		/// <param name="processFunc">追加する処理です。</param>
		/// <returns>
		///  <paramref name="pipeline"/>そのもの、または、
		///  指定された処理が追加された新しい<paramref name="pipeline"/>のコピーです。
		/// </returns>
		/// <exception cref="System.ArgumentNullException" />
		public static IPipeline Append(this IPipeline pipeline, ProcessFunc processFunc)
		{
			if (pipeline == null) {
				throw new ArgumentNullException(nameof(pipeline));
			}
			if (processFunc == null) {
				throw new ArgumentNullException(nameof(processFunc));
			}
			return pipeline.Append(new ProcessFuncWrapper(processFunc));
		}

		/// <summary>
		///  指定された処理を表すデリゲートを指定されたパイプラインの末尾に追加します。
		/// </summary>
		/// <typeparam name="TParam">追加する処理の引数の種類です。</typeparam>
		/// <typeparam name="TResult">追加する処理の戻り値の種類です。</typeparam>
		/// <param name="pipeline">登録先のパイプラインです。</param>
		/// <param name="processFunc">追加する処理です。</param>
		/// <returns>
		///  <paramref name="pipeline"/>そのもの、または、
		///  指定された処理が追加された新しい<paramref name="pipeline"/>のコピーです。
		/// </returns>
		/// <exception cref="System.ArgumentNullException" />
		public static IPipeline Append<TParam, TResult>(this IPipeline pipeline, ProcessFunc<TParam, TResult> processFunc)
		{
			if (pipeline == null) {
				throw new ArgumentNullException(nameof(pipeline));
			}
			if (processFunc == null) {
				throw new ArgumentNullException(nameof(processFunc));
			}
			return pipeline.Append(new ProcessFuncWrapper<TParam, TResult>(processFunc));
		}

		/// <summary>
		///  指定された例外の処理を行うデリゲートを指定されたパイプラインの末尾に追加します。
		/// </summary>
		/// <param name="pipeline">登録先のパイプラインです。</param>
		/// <param name="processFunc">追加する処理です。処理を再試行するには<c>next</c>関数を呼び出します。</param>
		/// <returns>
		///  <paramref name="pipeline"/>そのもの、または、
		///  指定された処理が追加された新しい<paramref name="pipeline"/>のコピーです。
		/// </returns>
		/// <exception cref="System.ArgumentNullException" />
		public static IPipeline AppendExceptionHandler(this IPipeline pipeline, ProcessFunc<(Exception exception, object? arg), object?> processFunc)
		{
			if (pipeline == null) {
				throw new ArgumentNullException(nameof(pipeline));
			}
			if (processFunc == null) {
				throw new ArgumentNullException(nameof(processFunc));
			}
			return pipeline.Append(new ExceptionHandler(processFunc));
		}

		/// <summary>
		///  指定された例外の処理を行うデリゲートを指定されたパイプラインの末尾に追加します。
		/// </summary>
		/// <typeparam name="TParam">追加する処理の引数の種類です。</typeparam>
		/// <typeparam name="TResult">追加する処理の戻り値の種類です。</typeparam>
		/// <param name="pipeline">登録先のパイプラインです。</param>
		/// <param name="processFunc">追加する処理です。処理を再試行するには<c>next</c>関数を呼び出します。</param>
		/// <returns>
		///  <paramref name="pipeline"/>そのもの、または、
		///  指定された処理が追加された新しい<paramref name="pipeline"/>のコピーです。
		/// </returns>
		/// <exception cref="System.ArgumentNullException" />
		public static IPipeline AppendExceptionHandler<TParam, TResult>(this IPipeline pipeline, ProcessFunc<(Exception exception, TParam arg), TResult> processFunc)
		{
			if (pipeline == null) {
				throw new ArgumentNullException(nameof(pipeline));
			}
			if (processFunc == null) {
				throw new ArgumentNullException(nameof(processFunc));
			}
			return pipeline.Append(new ExceptionHandler<TParam, TResult>(processFunc));
		}

		/// <summary>
		///  次に実行すべき処理を表します。
		/// </summary>
		/// <param name="context">実行に必要な文脈情報です。</param>
		/// <param name="arg">処理に必要な引数です。</param>
		/// <returns>戻り値を含むこの処理を表す非同期操作です。</returns>
		public delegate Task<object?> NextProcessFunc(IContext context, object? arg);

		/// <summary>
		///  パイプラインで実行される処理を表します。
		/// </summary>
		/// <param name="context">実行に必要な文脈情報です。</param>
		/// <param name="arg">処理に必要な引数です。</param>
		/// <param name="next">次に実行すべき処理です。</param>
		/// <returns>戻り値を含むこの処理を表す非同期操作です。</returns>
		public delegate Task<object?> ProcessFunc(IContext context, object? arg, NextProcessFunc next);

		/// <summary>
		///  パイプラインで実行される処理を表します。
		/// </summary>
		/// <typeparam name="TParam">この処理の引数の種類です。</typeparam>
		/// <typeparam name="TResult">この処理の戻り値の種類です。</typeparam>
		/// <param name="context">実行に必要な文脈情報です。</param>
		/// <param name="arg">処理に必要な引数です。</param>
		/// <param name="next">次に実行すべき処理です。</param>
		/// <returns>戻り値を含むこの処理を表す非同期操作です。</returns>
		public delegate Task<TResult> ProcessFunc<TParam, TResult>(IContext context, TParam arg, NextProcessFunc next);

		/// <summary>
		///  <see cref="ExapisSOP.Core.PipelineExtensions.ProcessFunc"/>を<see cref="ExapisSOP.IProcess"/>として扱います。
		/// </summary>
		public class ProcessFuncWrapper : CustomPipelineProcess<object?, object?>
		{
			private readonly ProcessFunc _func;

			/// <summary>
			///  この処理が実行可能な状態かどうかを表す論理値を取得します。
			/// </summary>
			public sealed override bool IsExecutable => _func != null;

			/// <summary>
			///  型'<see cref="ExapisSOP.Core.PipelineExtensions.ProcessFuncWrapper"/>'の新しいインスタンスを生成します。
			/// </summary>
			/// <param name="func">処理を表すデリゲートです。</param>
			public ProcessFuncWrapper(ProcessFunc func)
			{
				_func = func;
			}

			/// <summary>
			///  この処理の実行を非同期的に開始します。
			/// </summary>
			/// <param name="context">実行に必要な文脈情報です。</param>
			/// <param name="arg">処理に必要な引数です。</param>
			/// <returns>戻り値を含むこの処理を表す非同期操作です。</returns>
			protected override Task<object?> InvokeCore(IContext context, object? arg)
			{
				return _func(context, arg, new NextProcessFunc(this.NextProcess.InvokeAsync));
			}
		}

		/// <summary>
		///  <see cref="ExapisSOP.Core.PipelineExtensions.ProcessFunc{TParam, TResult}"/>を<see cref="ExapisSOP.IProcess"/>として扱います。
		/// </summary>
		/// <typeparam name="TParam">この処理の引数の種類です。</typeparam>
		/// <typeparam name="TResult">この処理の戻り値の種類です。</typeparam>
		public class ProcessFuncWrapper<TParam, TResult> : CustomPipelineProcess<TParam, TResult>
		{
			private readonly ProcessFunc<TParam, TResult> _func;

			/// <summary>
			///  この処理が実行可能な状態かどうかを表す論理値を取得します。
			/// </summary>
			public sealed override bool IsExecutable => _func != null;

			/// <summary>
			///  型'<see cref="ExapisSOP.Core.PipelineExtensions.ProcessFuncWrapper"/>'の新しいインスタンスを生成します。
			/// </summary>
			/// <param name="func">処理を表すデリゲートです。</param>
			public ProcessFuncWrapper(ProcessFunc<TParam, TResult> func)
			{
				_func = func;
			}

			/// <summary>
			///  この処理の実行を非同期的に開始します。
			/// </summary>
			/// <param name="context">実行に必要な文脈情報です。</param>
			/// <param name="arg">処理に必要な引数です。</param>
			/// <returns>戻り値を含むこの処理を表す非同期操作です。</returns>
			protected override Task<TResult> InvokeCore(IContext context, TParam arg)
			{
				return _func(context, arg, new NextProcessFunc(this.NextProcess.InvokeAsync));
			}
		}

		/// <summary>
		///  発生した例外を処理します。
		/// </summary>
		public class ExceptionHandler : CustomPipelineProcess<object?, object?>
		{
			private readonly ProcessFunc<(Exception exception, object? arg), object?> _func;

			/// <summary>
			///  この処理が実行可能な状態かどうかを表す論理値を取得します。
			/// </summary>
			public sealed override bool IsExecutable => _func != null;

			/// <summary>
			///  型'<see cref="ExapisSOP.Core.PipelineExtensions.ProcessFuncWrapper"/>'の新しいインスタンスを生成します。
			/// </summary>
			/// <param name="func">処理を表すデリゲートです。</param>
			public ExceptionHandler(ProcessFunc<(Exception exception, object? arg), object?> func)
			{
				_func = func;
			}

			/// <summary>
			///  この処理の実行を非同期的に開始します。
			/// </summary>
			/// <param name="context">実行に必要な文脈情報です。</param>
			/// <param name="arg">処理に必要な引数です。</param>
			/// <returns>戻り値を含むこの処理を表す非同期操作です。</returns>
			protected override Task<object?> InvokeCore(IContext context, object? arg)
			{
				try {
					return this.NextProcess.InvokeAsync(context, arg);
				} catch (Exception e) {
					return _func(context, (e, arg), new NextProcessFunc(this.NextProcess.InvokeAsync));
				}
			}
		}

		/// <summary>
		///  発生した例外を処理します。
		/// </summary>
		/// <typeparam name="TParam">この処理の引数の種類です。</typeparam>
		/// <typeparam name="TResult">この処理の戻り値の種類です。</typeparam>
		public class ExceptionHandler<TParam, TResult> : CustomPipelineProcess<TParam, TResult>
		{
			private readonly ProcessFunc<(Exception exception, TParam arg), TResult> _func;

			/// <summary>
			///  この処理が実行可能な状態かどうかを表す論理値を取得します。
			/// </summary>
			public sealed override bool IsExecutable => _func != null;

			/// <summary>
			///  型'<see cref="ExapisSOP.Core.PipelineExtensions.ProcessFuncWrapper"/>'の新しいインスタンスを生成します。
			/// </summary>
			/// <param name="func">処理を表すデリゲートです。</param>
			public ExceptionHandler(ProcessFunc<(Exception exception, TParam arg), TResult> func)
			{
				_func = func;
			}

			/// <summary>
			///  この処理の実行を非同期的に開始します。
			/// </summary>
			/// <param name="context">実行に必要な文脈情報です。</param>
			/// <param name="arg">処理に必要な引数です。</param>
			/// <returns>戻り値を含むこの処理を表す非同期操作です。</returns>
			protected async override Task<TResult> InvokeCore(IContext context, TParam arg)
			{
				try {
					var value = await this.NextProcess.InvokeAsync(context, arg);
					if (value is TResult result) {
						return result;
					} else {
						return default!;
					}
				} catch (Exception e) {
					return await _func(context, (e, arg), new NextProcessFunc(this.NextProcess.InvokeAsync));
				}
			}
		}
	}
}
