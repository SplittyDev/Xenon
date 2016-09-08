using System;
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
            var root = new AstRoot (unit.Location);

            try {

                // Start parsing
                while (unit.See ()) {

                    // Parse a statement
                    // root.AddChild (ParseStatement ());
                }

                // Print a visualization of the AST
                AstVisualizer.Visualize (root);

            } catch (ParserException e) {
                Console.WriteLine (e.Message);
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
                        return null;
                        // return ParseFunctionDeclaration ();
                }
            }

            // Try parsing a block
            if (unit.Match (TokenClass.OpenBrace)) {
                return null;
                // return ParseBlock ();
            }

            return null;
            // return ParseExpression ();
        }

        FunctionDeclaration ParseFunctionDeclaration () {
            // TODO: Implement this
            return null;
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

