/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.IO;

namespace ExapisSOP.IO.DataFlowModel
{
	internal interface IDataFlow : IDisposable
	{
		bool CanSeek    { get; }
		long Position   { get; set; }
		int  Capacity   { get; }
		int  Count      { get; }
		long Length     { get; }
		bool IsDisposed { get; }

		long Seek     (long offset, SeekOrigin origin);
		void SetLength(long value);
		void SetLength(int value);
	}
}
