/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;

#if NET48
using System.Collections.Generic;
using System.Reflection;
#endif

namespace ExapisSOP.NativeWrapper
{
	/// <summary>
	///  ���Ɉˑ��������@�Ńl�C�e�B�u�R�[�h���Ăяo���@�\��񋟂��܂��B
	/// </summary>
	public interface INativeCaller
	{
		/// <summary>
		///  �l�C�e�B�u�R�[�h�̌Ăяo�������s�ł�������ǂ������肵�܂��B
		/// </summary>
		/// <param name="reason">�T�|�[�g����Ȃ��ꍇ�A���̗��R��\����O�I�u�W�F�N�g��Ԃ��܂��B</param>
		/// <returns>�T�|�[�g�����ꍇ��<see langword="true"/>�A����ȊO�̏ꍇ��<see langword="false"/>��Ԃ��܂��B</returns>
		bool IsSupported(out PlatformNotSupportedException? reason);

#if NETCOREAPP3_1
		/// <summary>
		///  �l�C�e�B�u�R�[�h�̌Ăяo�������s�ł�������ǂ������肵�܂��B
		/// </summary>
		/// <returns>�T�|�[�g�����ꍇ��<see langword="true"/>�A����ȊO�̏ꍇ��<see langword="false"/>��Ԃ��܂��B</returns>
		public bool IsSupported()
		{
			return this.IsSupported(out _);
		}
#endif
	}

#if NET48
	/// <summary>
	///  .NET Framework�p��<see cref="ExapisSOP.NativeWrapper.INativeCaller"/>���g�����܂��B
	///  ���̃N���X�͐ÓI�N���X�ł��B
	/// </summary>
	/// <remarks>
	///  .NET Framework �Ŋ���̃C���^�[�t�F�[�X�����̌Ăяo���ƌ݊��������R�[�h�������ׂ̃N���X�ł��B
	///  �݊�����ۂׂɂ��̃N���X���璼�ڌĂяo������Ɋg�����\�b�h�𗘗p���Ă��������B
	/// </remarks>
	[Obsolete("����Ɋg�����\�b�h�𗘗p���Ă��������B", true)]
	public static class NetframeworkINativeCallerExtensions
	{
		private delegate bool Method();

		private static readonly Dictionary<Type, Method?> _cache = new Dictionary<Type, Method?>();

		private static Method? GetMethod(INativeCaller obj, Type type)
		{
			if (_cache.ContainsKey(type)) {
				return _cache[type];
			} else {
				try {
					var m = type.GetMethod(
						nameof(IsSupported), BindingFlags.Instance | BindingFlags.Public,
						null, Array.Empty<Type>(), null
					);
					var result = m?.CreateDelegate(typeof(Method), obj) as Method;
					_cache.Add(type, result);
					return result;
				} catch {
					return null;
				}
			}
		}

		/// <summary>
		///  �l�C�e�B�u�R�[�h�̌Ăяo�������s�ł�������ǂ������肵�܂��B
		/// </summary>
		/// <param name="nativeCaller">���ۂ̏������i�[�����I�u�W�F�N�g�ł��B</param>
		/// <returns>�T�|�[�g�����ꍇ��<see langword="true"/>�A����ȊO�̏ꍇ��<see langword="false"/>��Ԃ��܂��B</returns>
		public static bool IsSupported(this INativeCaller nativeCaller)
		{
			var m = GetMethod(nativeCaller, nativeCaller.GetType());
			if (m == null) {
				return nativeCaller.IsSupported(out _);
			} else {
				return m();
			}
		}
	}
#endif
}
