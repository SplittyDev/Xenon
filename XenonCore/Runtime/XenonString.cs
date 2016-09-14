using System;
namespace XenonCore {

    /// <summary>
    /// Xenon string.
    /// </summary>
    public class XenonString : XenonObject {

        /// <summary>
        /// The type definition.
        /// </summary>
        static readonly XenonType typeDef = new XenonType ("String");

        /// <summary>
        /// The value.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.XenonString"/> class.
        /// </summary>
        /// <param name="value">Value.</param>
        public XenonString (string value) : base (typeDef) {
            Value = value;
        }
    }
}

