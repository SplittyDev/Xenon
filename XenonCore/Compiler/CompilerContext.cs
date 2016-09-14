using System;
namespace XenonCore {

    /// <summary>
    /// Compiler context.
    /// </summary>
    public class CompilerContext {

        /// <summary>
        /// The symbol table.
        /// </summary>
        public SymbolTable SymbolTable;

        /// <summary>
        /// The module.
        /// </summary>
        public XenonModule Module;

        /// <summary>
        /// The emitter.
        /// </summary>
        public Emitter Emitter;

        public bool IsWithinClass;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.CompilerContext"/> class.
        /// </summary>
        /// <param name="symbolTable">Symbol table.</param>
        /// <param name="module">Module.</param>
        /// <param name="emitter">Emitter.</param>
        /// <param name="isWithinClass">Modification flag.</param>
        public CompilerContext (
            SymbolTable symbolTable,
            XenonModule module,
            Emitter emitter,
            bool isWithinClass = false
        ) {

            // Assign required arguments
            SymbolTable = symbolTable;
            Module = module;
            Emitter = emitter;

            // Assign optional arguments
            IsWithinClass = isWithinClass;
        }
    }
}

