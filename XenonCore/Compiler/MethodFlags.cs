using System;
namespace XenonCore {

    /// <summary>
    /// Method flags.
    /// </summary>
    [Flags]
    public enum MethodFlags {

        /// <summary>
        /// No flags.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The method accepts variadic arguments.
        /// </summary>
        AcceptsVariadicArgs,

        /// <summary>
        /// The method accepts keyword arguments.
        /// </summary>
        AcceptsKeywordArgs,

        /// <summary>
        /// The method has default parameters.
        /// </summary>
        HasDefaultParameters,

        /// <summary>
        /// The method has type-hinted parameters.
        /// </summary>
        HasTypeHintedParameters,
    }
}

