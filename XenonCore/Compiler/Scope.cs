using System;
using System.Collections.Generic;
using System.Linq;

namespace XenonCore {
    
    /// <summary>
    /// Scope.
    /// </summary>
    public class Scope {

        /// <summary>
        /// The symbols.
        /// </summary>
        readonly Stack<Symbol> Symbols;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scope"/> class.
        /// </summary>
        public Scope () {
            Symbols = new Stack<Symbol> ();
        }

        /// <summary>
        /// Adds a symbol.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="index">Index.</param>
        public void AddSymbol (string name, int index) {
            Symbols.Push (new Symbol (name, index));
        }

        /// <summary>
        /// Adds a symbol.
        /// </summary>
        /// <returns>The symbol.</returns>
        /// <param name="symbol">Symbol.</param>
        public void AddSymbol (Symbol symbol) {
            Symbols.Push (symbol);
        }

        /// <summary>
        /// Finds a symbol.
        /// </summary>
        /// <returns>The symbol.</returns>
        /// <param name="name">Name.</param>
        public bool FindSymbol (string name) {
            return Symbols.Any (s => s.Name == name);
        }

        /// <summary>
        /// Finds a symbol.
        /// </summary>
        /// <returns>The symbol.</returns>
        /// <param name="index">Index.</param>
        public bool FindSymbol (int index) {
            return Symbols.Any (s => s.Index == index);
        }

        /// <summary>
        /// Finds a symbol.
        /// </summary>
        /// <returns>The symbol.</returns>
        /// <param name="name">Name.</param>
        /// <param name="symbol">Symbol.</param>
        public bool FindSymbol (string name, out Symbol symbol) {
            symbol = null;
            if (FindSymbol (name)) {
                symbol = GetSymbol (name);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Finds a symbol by reference.
        /// </summary>
        /// <returns>The symbol by reference.</returns>
        /// <param name="index">Index.</param>
        /// <param name="symbol">Symbol.</param>
        public bool FindSymbolByIndex (int index, out Symbol symbol) {
            symbol = null;
            if (FindSymbol (index)) {
                symbol = GetSymbol (index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a symbol.
        /// </summary>
        /// <returns>The symbol.</returns>
        /// <param name="name">Name.</param>
        public Symbol GetSymbol (string name) {
            return Symbols.First (s => s.Name == name);
        }

        /// <summary>
        /// Gets a symbol.
        /// </summary>
        /// <returns>The symbol.</returns>
        /// <param name="index">Index.</param>
        public Symbol GetSymbol (int index) {
            return Symbols.First (s => s.Index == index);
        }
    }
}

