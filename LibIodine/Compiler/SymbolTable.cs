using System;
using System.Collections.Generic;

namespace IodineCore {
    
    /// <summary>
    /// Symbol table.
    /// </summary>
    public class SymbolTable {

        /// <summary>
        /// The global scope.
        /// </summary>
        readonly Scope GlobalScope;

        /// <summary>
        /// The scopes.
        /// </summary>
        readonly Stack<Scope> Scopes;

        int index;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Lore.SymbolTable"/> class.
        /// </summary>
        public SymbolTable () {
            GlobalScope = new Scope ();
            Scopes = new Stack<Scope> ();
            Scopes.Push (GlobalScope);
        }

        /// <summary>
        /// Gets the top scope.
        /// </summary>
        /// <value>The top scope.</value>
        public Scope TopScope => Scopes.Peek ();

        /// <summary>
        /// Pushes a scope.
        /// </summary>
        public void PushScope () {
            Scopes.Push (new Scope ());
        }

        /// <summary>
        /// Pops a scope.
        /// </summary>
        /// <returns>The scope.</returns>
        public void PopScope () {
            if (Scopes.Count <= 1) {
                throw new ParserException (null)
                    .Describe ("Attempt to leave global scope.")
                    .Describe ("This is a compiler bug.");
            }
            Scopes.Pop ();
        }

        public void InvokeInNewScope (Action action) {
            PushScope ();
            action ();
            PopScope ();
        }

        /// <summary>
        /// Finds a symbol.
        /// </summary>
        /// <returns>The symbol.</returns>
        /// <param name="name">Name.</param>
        /// <param name="symbol">Symbol.</param>
        public bool FindSymbol (string name, out Symbol symbol) {
            symbol = null;
            foreach (var scope in Scopes) {
                if (scope.FindSymbol (name)) {
                    symbol = scope.GetSymbol (name);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds a symbol.
        /// </summary>
        /// <returns>The symbol.</returns>
        /// <param name="name">Name.</param>
        public bool FindSymbol (string name) {
            Symbol _;
            return FindSymbol (name, out _);
        }

        /// <summary>
        /// Adds a symbol.
        /// </summary>
        /// <returns>The symbol index.</returns>
        public int AddSymbol (string name) {
            Scopes.Peek ().AddSymbol (new Symbol (name, index));
            return index++;
        }

        /// <summary>
        /// Adds a global symbol.
        /// </summary>
        /// <returns>The global symbol index.</returns>
        public int AddGlobalSymbol (string name) {
            GlobalScope.AddSymbol (new Symbol (name, index));
            return index++;
        }
    }
}

