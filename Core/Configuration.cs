/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

namespace ExapisSOP.Core
{
	internal class Configuration : IConfiguration
	{
		private readonly DefaultHostRunner _runner;

		internal Configuration(DefaultHostRunner runner)
		{
			_runner = runner;
		}

		public IConfiguration AddService(IService service)
		{
			_runner._services.Add(service);
			return this;
		}

		public IService[] GetServices()
		{
			return _runner._services.ToArray();
		}
	}
}
