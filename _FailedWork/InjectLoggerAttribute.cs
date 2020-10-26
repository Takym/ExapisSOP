/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;

namespace ExapisSOP.IO.Logging
{
	/// <summary>
	///  ログ出力処理を割り込ませる様に指示します。
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class InjectLoggerAttribute : Attribute
	{
		private static void PreAction(ILogger logger, MethodBase method)
		{
			logger.Trace($"{nameof(PreAction)} of \'{method.Name}\' at \'{method.ReflectedType?.AssemblyQualifiedName}\'");
		}

		private static void PostAction(ILogger logger, MethodBase method)
		{
			logger.Trace($"{nameof(PostAction)} of \'{method.Name}\' at \'{method.ReflectedType?.AssemblyQualifiedName}\'");
		}

		internal static void InjectLoggerCore()
		{
			var asms = AppDomain.CurrentDomain.GetAssemblies();
			var ab   = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("InjectLogger"), AssemblyBuilderAccess.Run);
			var mb   = ab.DefineDynamicModule("InjectLogger");
			for (int i = 0; i < asms.Length; ++i) {
				var types = asms[i].GetTypes();
				for (int j = 0; j < types.Length; ++j) {
					if (types[j].GetCustomAttribute<InjectLoggerAttribute>() is InjectLoggerAttribute attr && !types[j].IsSealed) {
						var tb = mb.DefineType(types[j].Name + "L", types[j].Attributes, types[j]);
						var funcs = types[j].GetMethods();
						for (int k = 0; k < funcs.Length; ++k) {
							if (funcs[i].IsVirtual) {
								var fb  = tb.DefineMethod(funcs[i].Name, _default_ma);
								var logger = Expression.MakeMemberAccess(
									null,
									typeof(LoggingSystemService).GetProperty(
										nameof(LoggingSystemService.InjectionLogger),
										BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetProperty,
										null, typeof(SystemLogger), new Type[0], null
									)
								);
								var block = Expression.Block(
									Expression.Call(
										typeof(InjectLoggerAttribute).GetMethod(nameof(PreAction)),
										logger, Expression.Constant(funcs[i])
									),
									Expression.Call(
										funcs[i]
									),
									Expression.Call(
										typeof(InjectLoggerAttribute).GetMethod(nameof(PostAction)),
										logger, Expression.Constant(funcs[i])
									)
								);
							}
						}
					}
				}
			}
		}

		private const MethodAttributes _default_ma = MethodAttributes.PrivateScope | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig;
	}
}
