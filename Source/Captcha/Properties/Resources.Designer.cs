﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReusableLibrary.Captcha.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ReusableLibrary.Captcha.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
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
        ///   Looks up a localized string similar to If you cannot read, click to generate a new one.
        /// </summary>
        internal static string HtmlHelperImgTitle {
            get {
                return ResourceManager.GetString("HtmlHelperImgTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The challenge code is invalid..
        /// </summary>
        internal static string ValidationChallengeInvalid {
            get {
                return ResourceManager.GetString("ValidationChallengeInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The code you typed has expired after {0:N0} seconds..
        /// </summary>
        internal static string ValidationCodeExpired {
            get {
                return ResourceManager.GetString("ValidationCodeExpired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The code you typed has no match..
        /// </summary>
        internal static string ValidationNoMatch {
            get {
                return ResourceManager.GetString("ValidationNoMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The code was typed too quickly. Wait at least {0:N0} seconds..
        /// </summary>
        internal static string ValidationTooQuickly {
            get {
                return ResourceManager.GetString("ValidationTooQuickly", resourceCulture);
            }
        }
    }
}
