using System;
namespace IodineCore {

    /// <summary>
    /// Xenon bytecode.
    /// </summary>
    public class IodineBytecode : IodineObject {

        /// <summary>
        /// The type definition.
        /// </summary>
        static readonly IodineType typeDef = new IodineType ("Bytecode");

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.XenonBytecode"/> class.
        /// </summary>
        public IodineBytecode () : base (typeDef) {
        }
    }
}

