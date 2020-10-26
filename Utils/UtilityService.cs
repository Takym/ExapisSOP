/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading.Tasks;
using ExapisSOP.Binary;
using ExapisSOP.Numerics;
using ExapisSOP.Text;

namespace ExapisSOP.Utils
{
	internal sealed class UtilityService : IUtilityService
	{
		public SimpleEncoding SimpleEncoding { get; } = new SimpleEncoding();

		public Task InitializeAsync(IContext context)
		{
			return Task.CompletedTask;
		}

		public DataValue CreateDataValue(object o)
		{
			return new DataValue(o);
		}

		public SimpleString CreateSimpleString(string s)
		{
			return new SimpleString(s);
		}

		public IRandom CreateRandom(long seed)
		{
			return new Xorshift(seed);
		}

		public Task FinalizeAsync(IContext context)
		{
			return Task.CompletedTask;
		}
	}
}
