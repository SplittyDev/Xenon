using System;
namespace IodineCore {

    /// <summary>
    /// Xenon string.
    /// </summary>
    public class IodineString : IodineObject {

        /// <summary>
        /// The type definition.
        /// </summary>
        static readonly IodineType typeDef = new IodineType ("String");

        /// <summary>
        /// The value.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.XenonString"/> class.
        /// </summary>
        /// <param name="value">Value.</param>
        public IodineString (string value) : base (typeDef) {
            Value = value;
        }
    }
}

