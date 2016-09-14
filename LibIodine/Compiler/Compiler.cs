using System;
using System.Collections.Generic;

namespace IodineCore {

    /// <summary>
    /// Compiler.
    /// </summary>
    public class Compiler : AstVisitor {

        /// <summary>
        /// The AST root node.
        /// </summary>
        readonly AstNode root;

        /// <summary>
        /// The context stack.
        /// </summary>
        readonly Stack<CompilerContext> haystack;

        /// <summary>
        /// The symbol table.
        /// </summary>
        SymbolTable symbolTable;

        /// <summary>
        /// Gets the current context.
        /// </summary>
        /// <value>The current context.</value>
        CompilerContext Context => haystack.Peek ();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.Compiler"/> class.
        /// </summary>
        /// <param name="root">Root.</param>
        public Compiler (AstNode root) {
            this.root = root;
            haystack = new Stack<CompilerContext> ();
        }

        /// <summary>
        /// Compiles the AST.
        /// </summary>
        public IodineModule Compile () {

            // Analyze the AST
            symbolTable = new SemanticAnalyzer ().Analyze (root);

            // Create the module
            var module = new IodineModule ();

            // Create the context
            var context = new CompilerContext (
                symbolTable: symbolTable,
                module: module,
                emitter: module.Initializer
            );

            // Push the context
            haystack.Push (context);

            // Compile the AST
            root.Visit (this);

            // Resolve the initializer
            module.Initializer.Resolve ();

            // Pop the context
            haystack.Pop ();

            // Return the module
            return module;
        }

        public override void Accept (AstRoot node) {

            // Compile the children of the node
            node.VisitChildren (this);
        }

        public override void Accept (NameExpression node) {

            // Create the name object
            var name = new IodineName (node.Value);

            // Make the name constant
            var constant = Context.Module.DefineConstant (name);

            // Test if the name exists in the current scope
            if (Context.SymbolTable.FindSymbol (node.Value)) {

                // Load the name as local
                Context.Emitter.Emit (
                    opcode: Opcode.LoadLocal,
                    arg: constant
                );

                // We're done here
                return;
            }

            // Test if the contaxt exists within a class
            if (Context.IsWithinClass) {

                // Load the name as attribute
                Context.Emitter.Emit (
                    opcode: Opcode.LoadAttribute,
                    arg: constant
                );

                // We're done here
                return;
            }

            // Load the name as global
            Context.Emitter.Emit (
                opcode: Opcode.LoadGlobal,
                arg: constant
            );
        }

        public override void Accept (FunctionDeclaration node) {

            // Add the function name to the scope
            Context.SymbolTable.AddSymbol (node.Name.Value);

            // Invoke the following action within a new scope
            Context.SymbolTable.InvokeInNewScope (() => {

                // Create an emitter for the function body
                var body = new Emitter ();

                // Invoke the following action within a new context
                InvokeInNewContext (body, () => {

                    // Iterate over the arguments of the function
                    foreach (var arg in node.Arguments) {

                        // Add the argument name to the scope
                        Context.SymbolTable.AddSymbol (arg.Name.Value);

                        // Test if the argument has a type hint
                        if (arg.HasTypeHint) {

                            // Compile the type hint
                            arg.TypeHint.Visit (this);

                            // Create a name object for the argument name
                            var name = new IodineName (arg.Name.Value);

                            // Make the name constant
                            var constant = Context.Module.DefineConstant (name);

                            // Perform a behavior check on the object
                            Context.Emitter.Emit (
                                opcode: Opcode.CastLocal,
                                arg: constant
                            );
                        }
                    }

                    // Visit the children of the function
                    node.VisitChildren (this);
                });

                // Resolve the function body
                body.Resolve ();

                // Create method flags
                var flags = MethodFlags.None;

                // TODO: Implement method flags

                // Iterate over the arguments of the function
                foreach (var arg in node.Arguments) {

                    // Create a string object with the name of the argument
                    var str = new IodineString (arg.Name.Value);

                    // Make the string object constant
                    var constant = Context.Module.DefineConstant (str);

                    // Load the constant argument name string
                    Context.Emitter.Emit (
                        opcode: Opcode.LoadConst,
                        arg: constant
                    );
                }

                // Build a tuple to hold [argument_count] elements
                Context.Emitter.Emit (
                    opcode: Opcode.BuildTuple,
                    arg: node.Arguments.Count
                );

                // Enter a new scope to satisfy the C# compiler
                {
                    // Make the generated byte code constant
                    var constant = Context.Module.DefineConstant (body);

                    // Load the generated byte code as constant
                    Context.Emitter.Emit (
                        opcode: Opcode.LoadConst,
                        arg: constant
                    );
                }

                // Build the function
                Context.Emitter.Emit (
                    opcode: Opcode.BuildFunction,
                    arg: (int) flags
                );
            });
        }

        void InvokeInNewContext (Emitter emitter, Action action) {

            // Create a new context
            var context = new CompilerContext (
                symbolTable: Context.SymbolTable,
                module: Context.Module,
                emitter: emitter,
                isWithinClass: false
            );

            // Push the context
            haystack.Push (context);

            // Invoke the action within the context
            action ();

            // Pop the context
            haystack.Pop ();
        }
    }
}

