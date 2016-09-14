using System;
namespace IodineCore {

    /// <summary>
    /// Xenon name.
    /// </summary>
    public class IodineName : IodineObject {

        /// <summary>
        /// The type definition.
        /// </summary>
        static readonly IodineType typeDef = new IodineType ("Name");

        /// <summary>
        /// The value.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.XenonName"/> class.
        /// </summary>
        /// <param name="value">Value.</param>
        public IodineName (string value) : base (typeDef) {
            Value = value;
        }
    }
}

