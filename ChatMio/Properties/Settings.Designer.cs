﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.34011
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChatMio.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        /// <summary>
        /// 最後にログインしたユーザー
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("最後にログインしたユーザー")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string LastUser {
            get {
                return ((string)(this["LastUser"]));
            }
            set {
                this["LastUser"] = value;
            }
        }
        
        /// <summary>
        /// 最後に接続したIP
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("最後に接続したIP")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string LastIP {
            get {
                return ((string)(this["LastIP"]));
            }
            set {
                this["LastIP"] = value;
            }
        }
        
        /// <summary>
        /// 最後に使用されたIPのリスト中でのインデックス
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("最後に使用されたIPのリスト中でのインデックス")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int LastIPIndex {
            get {
                return ((int)(this["LastIPIndex"]));
            }
            set {
                this["LastIPIndex"] = value;
            }
        }
        
        /// <summary>
        /// 管理者アカウントのパスワード
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("管理者アカウントのパスワード")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("akiyama")]
        public string Piyo {
            get {
                return ((string)(this["Piyo"]));
            }
            set {
                this["Piyo"] = value;
            }
        }
        
        /// <summary>
        /// 管理者アカウントか否か
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("管理者アカウントか否か")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool Pyon {
            get {
                return ((bool)(this["Pyon"]));
            }
            set {
                this["Pyon"] = value;
            }
        }
        
        /// <summary>
        /// ユーザーが設定したChatFormの幅
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("ユーザーが設定したChatFormの幅")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int ChatFormWidth {
            get {
                return ((int)(this["ChatFormWidth"]));
            }
            set {
                this["ChatFormWidth"] = value;
            }
        }
        
        /// <summary>
        /// ユーザーが設定したChatFormの高さ
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("ユーザーが設定したChatFormの高さ")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int ChatFormHeight {
            get {
                return ((int)(this["ChatFormHeight"]));
            }
            set {
                this["ChatFormHeight"] = value;
            }
        }
        
        /// <summary>
        /// ユーザーが設定したChatFormの位置X
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("ユーザーが設定したChatFormの位置X")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int ChatFormGeometryX {
            get {
                return ((int)(this["ChatFormGeometryX"]));
            }
            set {
                this["ChatFormGeometryX"] = value;
            }
        }
        
        /// <summary>
        /// ユーザーが設定したChatFormの位置Y
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("ユーザーが設定したChatFormの位置Y")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int ChatFormGeometryY {
            get {
                return ((int)(this["ChatFormGeometryY"]));
            }
            set {
                this["ChatFormGeometryY"] = value;
            }
        }
        
        /// <summary>
        /// SQLServerに接続するときに用いるサーバー名
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("SQLServerに接続するときに用いるサーバー名")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Server = localhost; Integrated security = SSPI;")]
        public string SQLServerConnectionString {
            get {
                return ((string)(this["SQLServerConnectionString"]));
            }
        }
    }
}
