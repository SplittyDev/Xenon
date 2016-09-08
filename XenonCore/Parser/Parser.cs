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

            // Parse an expression
            return ParseExpression ();
        }

        /*
         * '{' [ statement ... ] '}'
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
         * 'fn' < name > < argument_list > < code_block >
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
         * '(' [ < name > [ ':' < type_hint > ] [ ',' < name > [ ':' < type_hint > ] ... ] ] ')'
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
         * < ident >
         */
        NameExpression ParseName () {

            // Expect the name
            var name = unit.Expect (TokenClass.Identifier).Value;

            // Return the name expression
            return new NameExpression (unit.Location, name);
        }

        AstNode ParseExpression () {
            return ParseGeneratorExpression ();
        }

        AstNode ParseGeneratorExpression () {
            return ParseAssignment ();
        }

        AstNode ParseAssignment () {
            var expr = ParseTernaryIfElse ();

            // Match all assignments
            while (unit.Match (TokenClass.Operator)) {
                AstNode right;

                // Grab the current operator
                var current = unit.Peek ().Value;

                // Switch on the current operator
                switch (current) {
                    case "=":
                        unit.Accept (TokenClass.Operator);

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );
                        continue;
                    case "+=":
                        unit.Accept (TokenClass.Operator);

                        // Create the addition
                        right = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Add,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "-=":
                        unit.Accept (TokenClass.Operator);

                        // Create the subtraction
                        right = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Subtract,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "*=":
                        unit.Accept (TokenClass.Operator);

                        // Create the multiplication
                        right = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Multiply,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "/=":
                        unit.Accept (TokenClass.Operator);

                        // Create the division
                        right = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Divide,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "%=":
                        unit.Accept (TokenClass.Operator);

                        // Create the modulo division
                        right = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Modulo,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "^=":
                        unit.Accept (TokenClass.Operator);

                        // Create the bitwise exclusive or
                        right = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.BitwiseXor,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "&=":
                        unit.Accept (TokenClass.Operator);

                        // Create the bitwise and
                        right = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.BitwiseAnd,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "|=":
                        unit.Accept (TokenClass.Operator);

                        // Create the bitwise or
                        right = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.BitwiseOr,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "<<=":
                        unit.Accept (TokenClass.Operator);

                        // Create the bitwise shift left
                        right = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.ShiftLeft,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case ">>=":
                        unit.Accept (TokenClass.Operator);

                        // Create the bitwise shift right
                        right = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.ShiftRight,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                }

                // Break from the while loop
                break;
            }

            // Return the expression
            return expr;
        }

        /*
         * < expr > 'when' < condition > 'else' < expr >
         */
        AstNode ParseTernaryIfElse () {
            var expr = ParseRange ();

            // Match all ternary conditionals
            while (unit.Accept (TokenClass.Keyword, "when")) {

                // Parse the condition
                var condition = ParseExpression ();

                // Expect the 'else' keyword
                unit.Expect (TokenClass.Keyword, "else");

                // Parse the right side of the condition
                var right = ParseTernaryIfElse ();

                // Create the ternary expression
                expr = new TernaryExpression (
                    location: unit.Location,
                    condition: condition,
                    left: expr,
                    right: right
                );
            }

            // Return the expression
            return expr;
        }

        /*
         * < number > < '..' | '...' > < number >
         */
        AstNode ParseRange () {
            var expr = ParseBoolOr ();

            // Match all range expressions
            while (unit.Match (TokenClass.Operator)) {

                // Grab the range operator
                var current = unit.Peek ().Value;

                // Switch on the range operator
                switch (current) {
                    case "..":
                        unit.Accept (TokenClass.Operator);

                        // Create the exclusive range expression
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.ExclusiveRange,
                            left: expr,
                            right: ParseBoolOr ()
                        );
                        continue;
                    case "...":
                        unit.Accept (TokenClass.Operator);

                        // Create the inclusive range expression
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.InclusiveRange,
                            left: expr,
                            right: ParseBoolOr ()
                        );
                        continue;
                }

                // Break from the while loop
                break;
            }

            // Return the expression
            return expr;
        }

        /*
         * < expr > < 'or' | '??' > < expr >
         */
        AstNode ParseBoolOr () {
            var expr = ParseBoolAnd ();

            while (unit.Match (TokenClass.Operator)) {

                // Grab the boolean or operator
                var current = unit.Peek ().Value;

                // Switch on the boolean or operator
                switch (current) {
                    case "or":
                        unit.Accept (TokenClass.Operator);

                        // Create the boolean or expression
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.LogicalOr,
                            left: expr,
                            right: ParseBoolAnd ()
                        );
                        continue;
                    case "??":
                        unit.Accept (TokenClass.Operator);

                        // Create the null coalescing expression
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.NullCoalescing,
                            left: expr,
                            right: ParseBoolAnd ()
                        );
                        continue;
                }

                // Break from the while loop
                break;
            }

            // Return the expression
            return expr;
        }

        /*
         * < expr > 'and' < expr >
         */
        AstNode ParseBoolAnd () {
            var expr = ParseBitwiseOr ();

            // Match all boolean and operators
            while (unit.Accept (TokenClass.Operator, "and")) {

                // Create the boolean and expression
                expr = new BinaryExpression (
                    location: unit.Location,
                    op: BinaryOperation.LogicalAnd,
                    left: expr,
                    right: ParseBitwiseOr ()
                );
            }

            // Return the expression
            return expr;
        }

        /*
         * < expr > '|' < expr >
         */
        AstNode ParseBitwiseOr () {
            var expr = ParseBitwiseXor ();

            // Match all bitwise or operators
            while (unit.Accept (TokenClass.Operator, "|")) {

                // Create the bitwise or expression
                expr = new BinaryExpression (
                    location: unit.Location,
                    op: BinaryOperation.BitwiseOr,
                    left: expr,
                    right: ParseBitwiseXor ()
                );
            }

            // Return the expression
            return expr;
        }

        /*
         * < expr > '^' < expr >
         */
        AstNode ParseBitwiseXor () {
            var expr = ParseBitwiseAnd ();

            // Match all bitwise xor operators
            while (unit.Accept (TokenClass.Operator, "^")) {

                // Create the bitwise xor expression
                expr = new BinaryExpression (
                    location: unit.Location,
                    op: BinaryOperation.BitwiseXor,
                    left: expr,
                    right: ParseBitwiseAnd ()
                );
            }

            // Return the expression
            return expr;
        }

        /*
         * < expr > '&' < expr >
         */
        AstNode ParseBitwiseAnd () {
            var expr = ParseEquals ();

            // Match all bitwise and operators
            while (unit.Accept (TokenClass.Operator, "&")) {

                // Create the bitwise and expression
                expr = new BinaryExpression (
                    location: unit.Location,
                    op: BinaryOperation.BitwiseAnd,
                    left: expr,
                    right: ParseEquals ()
                );
            }

            // Return the expression
            return expr;
        }

        /*
         * < expr > < '==' | '!=' > < expr >
         */
        AstNode ParseEquals () {
            var expr = ParseRelationalOp ();

            // Match all equals operators
            while (unit.Match (TokenClass.Operator)) {

                // Grab the equals operator
                var current = unit.Peek ().Value;

                // Switch on the equals operator
                switch (current) {
                    case "==":
                        unit.Accept (TokenClass.Operator);

                        // Create the equals expression
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.Equals,
                            left: expr,
                            right: ParseRelationalOp ()
                        );
                        continue;
                    case "!=":
                        unit.Accept (TokenClass.Operator);

                        // Create the not equals expression
                        expr = new BinaryExpression (
                            location: unit.Location,
                            op: BinaryOperation.NotEquals,
                            left: expr,
                            right: ParseRelationalOp ()
                        );
                        continue;
                }

                // Break from the while loop
                break;
            }

            // Return the expression
            return expr;
        }

        AstNode ParseRelationalOp () {
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

