﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.544
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace s3pi.GenericRCOLResource.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("s3pi.GenericRCOLResource.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to ;type
        ///;tag (*=null)
        ///;y=stand alone resource
        ///;n=only embedded in a MODL or MLOD
        ///;desc
        ///0x015A1849 GEOM N Body Geometry ; lies! but the stand alone is not a GenericRCOL
        ///0x01661233 MODL Y Object Geometry
        ///0x01D0E6FB VBUF N Vertex Buffer
        ///0x01D0E70F IBUF N Index Buffer
        ///0x01D0E723 VRTF N Vertex Format
        ///0x01D0E75D MATD Y Material definition
        ///0x01D0E76B SKIN N Mesh skin
        ///0x01D10F34 MLOD Y Object Geometry LODs
        ///0x02019972 MTST Y Material set
        ///0x021D7E8C TREE Y
        ///0x0229684B VBUF N Vertex Buffer(Used in shadow m [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string RCOLResources {
            get {
                return ResourceManager.GetString("RCOLResources", resourceCulture);
            }
        }
    }
}
