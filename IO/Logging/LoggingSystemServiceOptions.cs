/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System.IO;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  <see cref="ExapisSOP.IO.Logging.ILoggingSystemService"/>の動作方法を指定します。
	/// </summary>
	public class LoggingSystemServiceOptions
	{
		private LogFileType    _file_type;
		private CreateLogFile? _file_factory;

		/// <summary>
		///  ログファイルの種類を取得または設定します。
		/// </summary>
		public LogFileType FileType
		{
			get => _file_type;
			set
			{
				_file_type = value;
				if (_file_type != LogFileType.Custom) {
					_file_factory = null;
				}
			}
		}

		/// <summary>
		///  ログファイルの作成を行う関数を取得または設定します。
		/// </summary>
		public CreateLogFile FileFactory
		{
			get => _file_factory ?? (_ => EmptyLogFile.Instance);
			set
			{
				_file_factory = value;
				_file_type    = LogFileType.Custom;
			}
		}

		/// <summary>
		///  システムサービスの状態を確認しログ出力を行う場合は<see langword="true"/>、行わない場合は<see langword="false"/>を設定します。
		/// </summary>
		public bool CheckServiceState { get; set; }

		/// <summary>
		///  <see cref="ExapisSOP.AppWorker.OnUpdate(ContextEventArgs)"/>
		///  呼び出し時にログ出力を行う場合は<see langword="true"/>、行わない場合は<see langword="false"/>を設定します。
		/// </summary>
		public bool LogOnUpdate { get; set; }

		/// <summary>
		///  ログファイルとエラーレポートファイルのファイル名を長い形式にする場合は<see langword="true"/>、しない場合は<see langword="false"/>を設定します。
		///  既定値は<see langword="false"/>です。
		/// </summary>
		public bool UseLongName { get; set; }

		/// <summary>
		///  型'<see cref="ExapisSOP.IO.Logging.LoggingSystemServiceOptions"/>'の新しいインスタンスを生成します。
		/// </summary>
		public LoggingSystemServiceOptions()
		{
			_file_type             = LogFileType.Text;
			_file_factory          = null;
			this.CheckServiceState = true;
			this.LogOnUpdate       = false;
			this.UseLongName       = false;
		}

		/// <summary>
		///  指定されたストリームにログファイルを作成し、そのログファイルの操作を行うオブジェクトを返します。
		/// </summary>
		/// <param name="stream">作成するログファイルのストリームです。</param>
		/// <returns>ログファイルの操作を行うオブジェクトです。</returns>
		public delegate ILogFile CreateLogFile(Stream stream);
	}
}
