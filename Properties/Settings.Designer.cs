﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PubliFaceFilter.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.0.3.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("c:\\my-pictures")]
        public string SavePath {
            get {
                return ((string)(this["SavePath"]));
            }
            set {
                this["SavePath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Say Cheese")]
        public string TakePictureText {
            get {
                return ((string)(this["TakePictureText"]));
            }
            set {
                this["TakePictureText"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int TakePictureTimeSeconds {
            get {
                return ((int)(this["TakePictureTimeSeconds"]));
            }
            set {
                this["TakePictureTimeSeconds"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int PictureSavedTimeSeconds {
            get {
                return ((int)(this["PictureSavedTimeSeconds"]));
            }
            set {
                this["PictureSavedTimeSeconds"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <string>/Library/demos/threejs/anonymous</string>
  <string>/Library/demos/threejs/butterflies</string>
  <string>/Library/demos/threejs/casa_de_papel</string>
  <string>/Library/demos/threejs/celFace</string>
  <string>/Library/demos/threejs/cloud</string>
  <string>/Library/demos/threejs/cube</string>
  <string>/Library/demos/threejs/dog_face</string>
  <string>/Library/demos/threejs/faceDeform</string>
  <string>/Library/demos/threejs/fireworks</string>
  <string>/Library/demos/threejs/football_makeup</string>
  <string>/Library/demos/threejs/glassesVTO</string>
  <string>/Library/demos/threejs/gltf_fullScreen</string>
  <string>/Library/demos/threejs/halloween_spider</string>
  <string>/Library/demos/threejs/luffys_hat_part1</string>
  <string>/Library/demos/threejs/luffys_hat_part2</string>
  <string>/Library/demos/threejs/matrix</string>
  <string>/Library/demos/threejs/miel_pops</string>
  <string>/Library/demos/threejs/multiCubes</string>
  <string>/Library/demos/threejs/multiLiberty</string>
  <string>/Library/demos/threejs/rupy_helmet</string>
  <string>/Library/demos/threejs/tiger</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection Masks {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["Masks"]));
            }
            set {
                this["Masks"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("visualear.stackstorage.com")]
        public string SFTPHost {
            get {
                return ((string)(this["SFTPHost"]));
            }
            set {
                this["SFTPHost"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("22")]
        public int SFTPPort {
            get {
                return ((int)(this["SFTPPort"]));
            }
            set {
                this["SFTPPort"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("pubface@visualear.stackstorage.com")]
        public string SFTPUsername {
            get {
                return ((string)(this["SFTPUsername"]));
            }
            set {
                this["SFTPUsername"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("r1TY1J3gPRnCG7soO5e7mSYgxn0")]
        public string SFTPPassword {
            get {
                return ((string)(this["SFTPPassword"]));
            }
            set {
                this["SFTPPassword"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30")]
        public int UpdateInterval {
            get {
                return ((int)(this["UpdateInterval"]));
            }
            set {
                this["UpdateInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int IdleInterval {
            get {
                return ((int)(this["IdleInterval"]));
            }
            set {
                this["IdleInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Loading...")]
        public string IdleText {
            get {
                return ((string)(this["IdleText"]));
            }
            set {
                this["IdleText"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string UpdateLink {
            get {
                return ((string)(this["UpdateLink"]));
            }
            set {
                this["UpdateLink"] = value;
            }
        }
    }
}
