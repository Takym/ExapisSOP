/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System.Threading.Tasks;

namespace ExapisSOP.Core
{
	internal sealed class TerminationProcess : IProcess
	{
		internal static readonly TerminationProcess Empty = new TerminationProcess();

		private TerminationProcess()
		{
			this.NextProcess = this;
		}

		public bool     IsExecutable { get; }
		public IProcess NextProcess  { get; set; }

		public void Init(IContext context)
		{
			// do nothing
		}

		public Task<object?> InvokeAsync(IContext context, object? arg)
		{
			return Task.FromResult(arg);
		}
	}
}
