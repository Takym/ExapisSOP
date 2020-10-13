/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  例外からエラーレポートを作成します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class ErrorReportBuilder
	{
		private string? _text;

		/// <summary>
		///  このオブジェクトが作成された日時を取得します。
		/// </summary>
		public DateTime DateTime { get; }

		/// <summary>
		///  コンストラクタに渡された例外を取得します。
		/// </summary>
		public Exception Exception { get; }

		/// <summary>
		///  作成するエラーレポートのオプションを取得します。
		///  この変数は派生クラスで利用されます。
		/// </summary>
		public string? Option { get; }

		/// <summary>
		///  追加情報を翻訳するオブジェクトの列挙体を取得します。
		/// </summary>
		public IEnumerable<ICustomErrorDetailProvider> DetailProviders { get; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.ErrorReportBuilder"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="exception">作成するエラーレポートの例外オブジェクトです。</param>
		/// <param name="option">
		///  作成するエラーレポートのオプションです。
		///  この引数は派生クラスで利用されます。
		/// </param>
		/// <param name="detailProviders">追加情報を翻訳するオブジェクトの列挙体です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		protected ErrorReportBuilder(Exception exception, string option, IEnumerable<ICustomErrorDetailProvider> detailProviders)
		{
			if (exception == null) {
				throw new ArgumentNullException(nameof(exception));
			}
			this.DateTime        = DateTime.Now;
			this.Exception       = exception;
			this.Option          = option;
			this.DetailProviders = detailProviders;
		}

		/// <summary>
		///  エラーレポートを生成し、指定された名前で既定の場所へ保存します。
		/// </summary>
		/// <param name="paths">ログファイルの既定の場所を表すパス文字列を格納したオブジェクトです。</param>
		/// <param name="name">エラーレポートの名前です。</param>
		/// <returns>自動生成された保存先のファイルパスです。</returns>
		public PathString Save(Paths paths, string? name = null)
		{
			if (string.IsNullOrEmpty(name)) {
				name = "ErrorReport";
			} else {
				name = "ER_" + name;
			}
			var path = paths.Logs + LoggingSystemService.CreateFileName(this.DateTime, name);
			this.Save(path);
			return path;
		}

		/// <summary>
		///  エラーレポートを生成し、指定されたパスを持つファイルへ保存します。
		/// </summary>
		/// <param name="path">エラーレポートの保存先のファイルパスです。</param>
		public void Save(PathString path)
		{
			this.Save(new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read));
		}

		/// <summary>
		///  エラーレポートを生成し、指定されたストリームへ保存します。
		/// </summary>
		/// <param name="stream">エラーレポートの保存先のストリームです。</param>
		public void Save(Stream stream)
		{
			this.Save(new StreamWriter(stream, Encoding.UTF8));
		}

		/// <summary>
		///  エラーレポートを生成し、指定されたライターへ保存します。
		/// </summary>
		/// <param name="writer">エラーレポートの保存先のライターです。</param>
		public void Save(TextWriter writer)
		{
			if (_text == null) {
				_text = this.Build();
			}
			writer.WriteLine(_text);
		}

		/// <summary>
		///  エラーレポートを生成します。
		/// </summary>
		/// <returns>生成されたエラーレポートを表す文字列です。</returns>
		protected virtual string Build()
		{
			var sb = new StringBuilder();
			this.BuildHeader(sb, this.DateTime, Process.GetCurrentProcess().Id);
			this.BuildBody  (sb, this.Exception);
			sb.AppendLine();
			return sb.ToString();
		}

		/// <summary>
		///  エラーレポートの見出しを作成します。
		/// </summary>
		/// <param name="sb">戻り値を格納するオブジェクトです。</param>
		/// <param name="dt">エラーレポートの作成日時です。</param>
		/// <param name="pid">エラーレポートを作成したプロセスのIDです。</param>
		protected virtual void BuildHeader(StringBuilder sb, DateTime dt, int pid)
		{
			sb.AppendLine();
			sb.AppendLine("************************************************");
			sb.AppendLine();
			sb.AppendLine($"  {this.GetLocalizedHeaderLine1_Caption()}");
			sb.AppendLine();
			sb.AppendLine($"  {this.GetLocalizedHeaderLine2_Created  (dt )}");
			sb.AppendLine($"  {this.GetLocalizedHeaderLine3_ProcessId(pid)}");
			sb.AppendLine();
			sb.AppendLine("************************************************");
			sb.AppendLine();
			sb.AppendLine(this.GetLocalizedHeaderLine4_Notice());
			sb.AppendLine();
		}

		/// <summary>
		///  エラーレポートの内容を作成します。
		/// </summary>
		/// <param name="sb">戻り値を格納するオブジェクトです。</param>
		/// <param name="ex">エラーレポートの例外です。</param>
		/// <param name="index">何番目の内部例外かを表す整数値です。<c>0</c>の場合は内部例外ではありません。</param>
		protected virtual void BuildBody(StringBuilder sb, Exception ex, int index = 0)
		{
			if (index == 0) {
				sb.AppendLine("========");
			} else {
				sb.AppendLine($"======== {index,10} >>>");
			}
			sb.AppendLine(this.GetLocalizedBodyLine0_TypeName  (ex.GetType().AssemblyQualifiedName));
			sb.AppendLine(this.GetLocalizedBodyLine1_Message   (ex.Message));
			sb.AppendLine(this.GetLocalizedBodyLine2_HResult   (ex.HResult));
			sb.AppendLine(this.GetLocalizedBodyLine3_HelpLink  (ex.HelpLink));
			sb.AppendLine(this.GetLocalizedBodyLine4_Source    (ex.Source));
			sb.AppendLine(this.GetLocalizedBodyLine5_TargetSite(ex.TargetSite?.Name, ex.TargetSite?.ReflectedType?.AssemblyQualifiedName));
			string[] stacktrace = ex.StackTrace?.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
			for (int i = 0; i < stacktrace.Length; ++i) {
				sb.AppendLine(this.GetLocalizedBodyLine6_StackTrace(i, stacktrace[i].Trim()));
			}
			if (ex.Data == null) {
				sb.AppendLine(this.GetLocalizedBodyLine7_Data("<null>"));
			} else if (ex.Data.Count == 0) {
				sb.AppendLine(this.GetLocalizedBodyLine7_Data("<count>"));
			} else {
				sb.AppendLine(this.GetLocalizedBodyLine7_Data(ex.Data.Count.ToString()));
				var dictenum = ex.Data.GetEnumerator();
				dictenum.Reset();
				while (dictenum.MoveNext()) {
					sb.AppendLine($"\t* [\"{dictenum.Key}\"] = {dictenum.Value}");
				}
			}
			sb.AppendLine("--------");
			foreach (var provider in this.DetailProviders) {
				sb.AppendLine(provider.GetLocalizedDetail(ex));
			}
			sb.AppendLine("--------");
			sb.AppendLine(ex.ToString());
			if (ex is AggregateException aggregateException) {
				var exs = aggregateException.InnerExceptions;
				for (int i = 0; i < exs.Count; ++i) {
					sb.AppendLine($"++++++++ {i,10} >>>");
					this.BuildBody(sb, exs[i]);
					sb.AppendLine($"++++++++ {i,10} <<<");
					sb.AppendLine();
				}
			}
			if (index != 0) {
				sb.AppendLine($"======== {index,10} <<<");
			}
			if (ex.InnerException != null) {
				if (index == 0) {
					sb.AppendLine("########");
				}
				sb.AppendLine();
				sb.AppendLine(this.GetLocalizedBodyLine8_InnerException());
				this.BuildBody(sb, ex.InnerException);
			}
		}

		/// <summary>
		///  上書きされた場合、見出しの1行目(題名)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedHeaderLine1_Caption();

		/// <summary>
		///  上書きされた場合、見出しの2行目(作成日時)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="dt">作成日時です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedHeaderLine2_Created(DateTime dt);

		/// <summary>
		///  上書きされた場合、見出しの3行目(プロセスID)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="pid">プロセスIDです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedHeaderLine3_ProcessId(int pid);

		/// <summary>
		///  上書きされた場合、見出しの4行目(注意文)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedHeaderLine4_Notice();

		/// <summary>
		///  上書きされた場合、内容の0行目(型名)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="typename">例外オブジェクトの型名です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine0_TypeName(string? typename);

		/// <summary>
		///  上書きされた場合、内容の1行目(メッセージ)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="message">メッセージです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine1_Message(string message);

		/// <summary>
		///  上書きされた場合、内容の2行目(H-RESULT)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="hresult">H-RESULTです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine2_HResult(int hresult);

		/// <summary>
		///  上書きされた場合、内容の3行目(ヘルプリンク)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="helplink">ヘルプリンクです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine3_HelpLink(string? helplink);

		/// <summary>
		///  上書きされた場合、内容の4行目(発生源)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="source">発生源です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine4_Source(string? source);

		/// <summary>
		///  上書きされた場合、内容の5行目(発生場所)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="methodName">例外を発生させた関数の名前です。</param>
		/// <param name="className">例外を発生させた型の名前です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine5_TargetSite(string? methodName, string? className);

		/// <summary>
		///  上書きされた場合、内容の6行目(スタックトレース)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="index">スタックフレームの番号です。</param>
		/// <param name="stackframe">スタックフレームを表す文字列です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine6_StackTrace(int index, string stackframe);

		/// <summary>
		///  上書きされた場合、内容の7行目(内部データ)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <param name="content">内部データの個数を表す文字列です。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine7_Data(string content);

		/// <summary>
		///  上書きされた場合、内容の8行目(内部例外)を表す翻訳済みの文字列を取得します。
		/// </summary>
		/// <returns>翻訳済みの文字列です。</returns>
		protected abstract string GetLocalizedBodyLine8_InnerException();
	}
}
