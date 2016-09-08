using System;
using System.Collections.Generic;

namespace XenonCore {

    /// <summary>
    /// Parser.
    /// </summary>
    public class Parser {

        /// <summary>
        /// The parsing unit.
        /// </summary>
        readonly ParsingUnit unit;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.Parser"/> class.
        /// </summary>
        /// <param name="unit">Unit.</param>
        public Parser (ParsingUnit unit) {
            this.unit = unit;
        }

        /// <summary>
        /// Parses the tokenized source.
        /// </summary>
        /// <returns>The root AST node.</returns>
        public AstRoot Parse () {
            
            // Create the root node
            var root = new AstRoot (unit.See () ? unit.Location : SourceLocation.Zero);

            try {

                // Start parsing
                while (unit.See ()) {

                    // Parse a statement
                    root.AddChild (ParseStatement ());
                }

                // Print a visualization of the AST
                AstVisualizer.Visualize (root);

            } catch (ParserException e) {

                // Print the error message
                Console.WriteLine (e.Message);

                // Exit the program with a return value indicating failure
                Environment.Exit (1);
            }

            // Return the root node
            return root;
        }

        AstNode ParseStatement () {
            var current = unit.Current;

            // Try parsing a keyword
            if (current.Is (TokenClass.Keyword)) {
                switch (current.Value) {
                    case "fn":
                        return ParseFunctionDeclaration ();
                }
            }

            // Try parsing a block
            if (unit.Match (TokenClass.OpenBrace)) {
                return ParseBlock ();
            }

            return null;
            // return ParseExpression ();
        }

        /*
         * '{' [statement ...] '}'
         */
        CodeBlock ParseBlock () {
            var block = new CodeBlock (unit.Location);

            // Expect opening curly brace
            unit.Expect (TokenClass.OpenBrace);

            // Match all statements
            while (!unit.Match (TokenClass.CloseBrace)) {

                // Add the next statement to the code block
                block.AddChild (ParseStatement ());
            }

            // Expect closing curly brace
            unit.Expect (TokenClass.CloseBrace);

            // Return the code block
            return block;
        }

        /*
         * 'fn' <name> <argument_list> <code_block>
         */
        FunctionDeclaration ParseFunctionDeclaration () {
            var node = new FunctionDeclaration (unit.Location);

            // Expect the 'fn' keyword
            unit.Expect (TokenClass.Keyword, "fn");

            // Read the name of the function
            node.Name = ParseName ();

            // Read the argument list
            node.Arguments.AddRange (ParseArgumentList ());

            // Parse the function body
            node.Body = ParseBlock ();

            // Return the function declaration
            return node;
        }

        /*
         * '(' [<name> [ ':' <type_hint> ] [ ',' <name> [ ':' <type_hint> ] ... ] ] ')'
         */
        List<NamedParameter> ParseArgumentList () {
            var arguments = new List<NamedParameter> ();

            // Expect opening parenthesis
            unit.Expect (TokenClass.OpenParen);

            // Match all arguments
            while (unit.Match (TokenClass.Identifier)) {

                // Read the name of the argument
                var argument = new NamedParameter (ParseName ());

                // Test if the argument has a type hint
                if (unit.Accept (TokenClass.Colon)) {

                    // Parse the type hint of the argument
                    argument.TypeHint = ParseName ();
                }

                // Add the argument to the argument list
                arguments.Add (argument);

                // Accept a comma
                if (!unit.Accept (TokenClass.Comma))
                    break;
            }

            // Expect closing parenthesis
            unit.Expect (TokenClass.CloseParen);

            // Return the argument list
            return arguments;
        }

        /*
         * <ident>
         */
        NameExpression ParseName () {

            // Expect the name
            var name = unit.Expect (TokenClass.Identifier).Value;

            // Return the name expression
            return new NameExpression (unit.Location, name);
        }

        internal void Synchronize () {
            while (unit.Current != null) {
                var tk = unit.Read ();
                if (tk.Is (TokenClass.CloseBracket)
                    || tk.Is (TokenClass.Semicolon)) {
                    return;
                }
            }
        }
    }
}

