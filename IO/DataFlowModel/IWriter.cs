/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System.Threading.Tasks;

namespace ExapisSOP.IO.DataFlowModel
{
	internal interface IWriter : IDataFlow
	{
		void WriteByte(byte value);
		void Write    (byte[] buffer, int offset, int count);
		void Flush();

		Task WriteAsync(byte[] buffer, int offset, int count);
		Task FlushAsync();
	}
}
