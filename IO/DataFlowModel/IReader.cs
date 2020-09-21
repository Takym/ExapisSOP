/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System.Threading.Tasks;

namespace ExapisSOP.IO.DataFlowModel
{
	internal interface IReader : IDataFlow
	{
		int  ReadByte();
		int  Read    (byte[] buffer, int offset, int count);
		int  Peek();
		void Refresh();

		Task<int> ReadAsync(byte[] buffer, int offset, int count);
		Task<int> PeekAsync();
		Task      RefreshAsync();
	}
}
