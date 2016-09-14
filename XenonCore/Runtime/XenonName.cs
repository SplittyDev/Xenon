using System;
namespace XenonCore {

    /// <summary>
    /// Xenon name.
    /// </summary>
    public class XenonName : XenonObject {

        /// <summary>
        /// The type definition.
        /// </summary>
        static readonly XenonType typeDef = new XenonType ("Name");

        /// <summary>
        /// The value.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.XenonName"/> class.
        /// </summary>
        /// <param name="value">Value.</param>
        public XenonName (string value) : base (typeDef) {
            Value = value;
        }
    }
}

