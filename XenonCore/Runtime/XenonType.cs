using System;
namespace XenonCore {

    /// <summary>
    /// Xenon type.
    /// </summary>
    public class XenonType : XenonObject {

        /// <summary>
        /// The type definition.
        /// </summary>
        static XenonTypeTypeDefinition TypeDef = new XenonTypeTypeDefinition ();

        /// <summary>
        /// The name of the type.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.XenonType"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        public XenonType (string name) : base (TypeDef) {
            Name = name;
        }

        /// <summary>
        /// XenonType type definition.
        /// </summary>
        class XenonTypeTypeDefinition : XenonType {

            /// <summary>
            /// Initializes a new instance of the <see cref="T:XenonCore.XenonType.XenonTypeTypeDefinition"/> class.
            /// </summary>
            public XenonTypeTypeDefinition () : base ("TypeDef") {
            }
        }
    }
}

