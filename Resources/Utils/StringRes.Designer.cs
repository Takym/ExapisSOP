﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExapisSOP.Resources.Utils {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class StringRes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal StringRes() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ExapisSOP.Resources.Utils.StringRes", typeof(StringRes).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   CryptionRandom_InvalidOperationException に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string CryptionRandom_InvalidOperationException {
            get {
                return ResourceManager.GetString("CryptionRandom_InvalidOperationException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   DataValue_NotSupportedException {0} に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string DataValue_NotSupportedException {
            get {
                return ResourceManager.GetString("DataValue_NotSupportedException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Random_ArgumentOutOfRangeException に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string Random_ArgumentOutOfRangeException {
            get {
                return ResourceManager.GetString("Random_ArgumentOutOfRangeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   RandomExtension_ArgumentOutOfRangeException {0} {1} に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string RandomExtension_ArgumentOutOfRangeException {
            get {
                return ResourceManager.GetString("RandomExtension_ArgumentOutOfRangeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SecureStringExtensions_SecurityException に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SecureStringExtensions_SecurityException {
            get {
                return ResourceManager.GetString("SecureStringExtensions_SecurityException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SystemRandom_InvalidOperationException に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string SystemRandom_InvalidOperationException {
            get {
                return ResourceManager.GetString("SystemRandom_InvalidOperationException", resourceCulture);
            }
        }
    }
}
