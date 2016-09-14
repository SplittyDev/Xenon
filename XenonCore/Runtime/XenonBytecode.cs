using System;
namespace XenonCore {

    /// <summary>
    /// Xenon bytecode.
    /// </summary>
    public class XenonBytecode : XenonObject {

        /// <summary>
        /// The type definition.
        /// </summary>
        static readonly XenonType typeDef = new XenonType ("Bytecode");

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.XenonBytecode"/> class.
        /// </summary>
        public XenonBytecode () : base (typeDef) {
        }
    }
}

