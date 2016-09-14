using System;
namespace IodineCore {

    /// <summary>
    /// Symbol.
    /// </summary>
    public class Symbol {

        /// <summary>
        /// The name.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The index.
        /// </summary>
        public readonly int Index;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Lore.Symbol"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="index">Index.</param>
        public Symbol (string name, int index) {
            Name = name;
            Index = index;
        }
    }
}

