using System;
namespace XenonCore {

    /// <summary>
    /// Compiler.
    /// </summary>
    public class Compiler : AstVisitor {

        /// <summary>
        /// The symbol table.
        /// </summary>
        readonly SymbolTable symbolTable;

        /// <summary>
        /// The AST root node.
        /// </summary>
        readonly AstNode root;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.XenonCompiler"/> class.
        /// </summary>
        /// <param name="root">Root.</param>
        /// <param name="symbolTable">Symbol table.</param>
        public Compiler (AstNode root, SymbolTable symbolTable) {
            this.root = root;
            this.symbolTable = symbolTable;
        }

        public XenonModule Compile () {

            // TODO: Implement this

            return null;
        }
    }
}

