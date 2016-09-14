using System;
namespace IodineCore {

    /// <summary>
    /// Opcode.
    /// </summary>
    public enum Opcode {

        /// <summary>
        /// No operation.
        /// </summary>
        Nop = 0,

        /// <summary>
        /// Load a local.
        /// </summary>
        LoadLocal,

        /// <summary>
        /// Load a global.
        /// </summary>
        LoadGlobal,

        /// <summary>
        /// Load a constant.
        /// </summary>
        LoadConst,

        /// <summary>
        /// Load an attribute.
        /// </summary>
        LoadAttribute,

        /// <summary>
        /// Perform a behavior check on a local.
        /// </summary>
        CastLocal,

        /// <summary>
        /// Build a tuple.
        /// </summary>
        BuildTuple,

        /// <summary>
        /// Build a function.
        /// </summary>
        BuildFunction,
    }
}

