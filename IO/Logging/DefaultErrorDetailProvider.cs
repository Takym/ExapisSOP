/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  既定の例外の追加情報を提供します。
	///  このクラスは継承できません。
	/// </summary>
	/// <remarks>
	///  情報は英語で出力されます。
	/// </remarks>
	public sealed class DefaultErrorDetailProvider : ICustomErrorDetailProvider
	{
		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.DefaultErrorDetailProvider"/>'の新しいインスタンスを生成します。
		/// </summary>
		public DefaultErrorDetailProvider() { }

		/// <summary>
		///  追加情報を可読な翻訳済みの文字列へ変換します。
		/// </summary>
		/// <param name="exception">変換するデータを保持している例外オブジェクトです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		public string GetLocalizedDetail(Exception exception)
		{
			var sb = new StringBuilder();
			sb.AppendLine("Detailed information (English):");
			switch (exception) {
			case NotImplementedException _:
				sb.AppendLine(" - The process was not implemented.");
				break;
			case SystemException se:
				sb.AppendLine(" - This is a system exception.");
				this.SystemException(sb, se);
				break;
			case ApplicationException _:
				sb.AppendLine(" - This is an application exception.");
				break;
			default:
				sb.AppendLine(" - No more information.");
				break;
			}
			return sb.ToString();
		}

		private void SystemException(StringBuilder sb, SystemException se)
		{
			switch (se) {
			case ArgumentException ae:
				sb.AppendLine($" - The parameter name is: \"{ae.ParamName}\".");
				if (ae is ArgumentOutOfRangeException aoore) {
					sb.AppendLine($" - The actual value is: \"{aoore.ActualValue}\".");
				}
				if (ae is CultureNotFoundException cnfe) {
					sb.AppendLine($" - The invalid culture name is: \"{cnfe.InvalidCultureName}\".");
					sb.AppendLine($" - The invalid culture identifier is: {cnfe.InvalidCultureId}.");
				}
				break;
			case IOException ioe:
				if (ioe is FileNotFoundException fnfe) {
					sb.AppendLine($" - The file \"{fnfe.FileName}\" does not exist.");
					sb.AppendLine($" - The fusion log is: \"{fnfe.FusionLog}\".");
				}
				if (ioe is FileLoadException fle) {
					sb.AppendLine($" - Could not load the file \"{fle.FileName}\".");
					sb.AppendLine($" - The fusion log is: \"{fle.FusionLog}\".");
				}
				if (ioe is InvalidPathFormatException ipfe) {
					sb.AppendLine($" - The invalid path is: \"{ipfe.InvalidPath}\".");
				}
				break;
			case OperationCanceledException oce:
				sb.AppendLine($" - Was cancellation requested? {oce.CancellationToken.IsCancellationRequested}");
				if (oce is TerminationException te) {
					sb.AppendLine($" - The termination reason is: {te.Reason}.");
				}
				break;
			case Win32Exception w32e:
				sb.AppendLine($" - The error code is: 0x{w32e.ErrorCode:X08} ({w32e.ErrorCode}).");
				sb.AppendLine($" - The native error code is: \"{w32e.NativeErrorCode}\".");
				break;
			case XmlException xe:
				sb.AppendLine($" - The target (source URI) is: \"{xe.SourceUri}\".");
				sb.AppendLine($" - The line number   is: {xe.LineNumber}.");
				sb.AppendLine($" - The line position is: {xe.LinePosition}.");
				break;
			}
		}
	}
}
