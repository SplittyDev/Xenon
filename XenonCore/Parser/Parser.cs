using System;
using System.Collections.Generic;
using System.Numerics;

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
            node.Arguments.AddRange (ParseDeclarationArgumentList ());

            // Parse the function body
            node.Body = ParseBlock ();

            // Return the function declaration
            return node;
        }

        /*
         * '(' [ < name > [ ':' < type_hint > ] [ ',' < name > [ ':' < type_hint > ] ... ] ] ')'
         */
        List<NamedParameter> ParseDeclarationArgumentList () {
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

        /*
         * < expr > < '=' | '+=' | '-=' | '*=' | '/=' | '%=' | '^=' | '&=' | '|=' | '<<=' | '>>=' > < expr >
         */
        AstNode ParseAssignment () {
            var expr = ParseTernaryIfElse ();

            // Match all assignments
            while (unit.Match (TokenClass.Operator)) {
                AstNode right;

                // Grab the current operator
                var current = unit.Peek ();

                // Switch on the current operator
                switch (current.Value) {
                    case "=":
                        unit.Skip ();

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );
                        continue;
                    case "+=":
                        unit.Skip ();

                        // Create the addition
                        right = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Add,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "-=":
                        unit.Skip ();

                        // Create the subtraction
                        right = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Subtract,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "*=":
                        unit.Skip ();

                        // Create the multiplication
                        right = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Multiply,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "/=":
                        unit.Skip ();

                        // Create the division
                        right = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Divide,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "%=":
                        unit.Skip ();

                        // Create the modulo division
                        right = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Modulo,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "^=":
                        unit.Skip ();

                        // Create the bitwise exclusive or
                        right = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.BitwiseXor,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "&=":
                        unit.Skip ();

                        // Create the bitwise and
                        right = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.BitwiseAnd,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "|=":
                        unit.Skip ();

                        // Create the bitwise or
                        right = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.BitwiseOr,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case "<<=":
                        unit.Skip ();

                        // Create the bitwise shift left
                        right = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.ShiftLeft,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Assign,
                            left: expr,
                            right: right
                        );
                        continue;
                    case ">>=":
                        unit.Skip ();

                        // Create the bitwise shift right
                        right = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.ShiftRight,
                            left: expr,
                            right: ParseTernaryIfElse ()
                        );

                        // Create the assignment
                        expr = new BinaryExpression (
                            location: current.Location,
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
            while (unit.Match (TokenClass.Keyword, "when")) {
                var location = unit.Read ().Location;

                // Parse the condition
                var condition = ParseExpression ();

                // Expect the 'else' keyword
                unit.Expect (TokenClass.Keyword, "else");

                // Parse the right side of the condition
                var right = ParseTernaryIfElse ();

                // Create the ternary expression
                expr = new TernaryExpression (
                    location: location,
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
                var current = unit.Peek ();

                // Switch on the range operator
                switch (current.Value) {
                    case "..":
                        unit.Skip ();

                        // Create the exclusive range expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.ExclusiveRange,
                            left: expr,
                            right: ParseBoolOr ()
                        );
                        continue;
                    case "...":
                        unit.Skip ();

                        // Create the inclusive range expression
                        expr = new BinaryExpression (
                            location: current.Location,
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
                var current = unit.Peek ();

                // Switch on the boolean or operator
                switch (current.Value) {
                    case "or":
                        unit.Skip ();

                        // Create the boolean or expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.LogicalOr,
                            left: expr,
                            right: ParseBoolAnd ()
                        );
                        continue;
                    case "??":
                        unit.Skip ();

                        // Create the null coalescing expression
                        expr = new BinaryExpression (
                            location: current.Location,
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
            while (unit.Match (TokenClass.Operator, "and")) {
                var current = unit.Read ();

                // Create the boolean and expression
                expr = new BinaryExpression (
                    location: current.Location,
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
            while (unit.Match (TokenClass.Operator, "|")) {
                var current = unit.Read ();

                // Create the bitwise or expression
                expr = new BinaryExpression (
                    location: current.Location,
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
            while (unit.Match (TokenClass.Operator, "^")) {
                var current = unit.Read ();

                // Create the bitwise xor expression
                expr = new BinaryExpression (
                    location: current.Location,
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
            while (unit.Match (TokenClass.Operator, "&")) {
                var current = unit.Read ();

                // Create the bitwise and expression
                expr = new BinaryExpression (
                    location: current.Location,
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
                var current = unit.Peek ();

                // Switch on the equals operator
                switch (current.Value) {
                    case "==":
                        unit.Skip ();

                        // Create the equals expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Equals,
                            left: expr,
                            right: ParseRelationalOp ()
                        );
                        continue;
                    case "!=":
                        unit.Skip ();

                        // Create the not equals expression
                        expr = new BinaryExpression (
                            location: current.Location,
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

        /*
         * < expr > < '>' | '<' | '>=' | '<=' | 'is' | 'isnot' > < expr >
         */
        AstNode ParseRelationalOp () {
            var expr = ParseBitshift ();

            // Match all relational operators
            while (unit.Match (TokenClass.Operator)) {

                // Grab the relational operator
                var current = unit.Peek ();

                // Switch on the relational operator
                switch (current.Value) {
                    case ">":
                        unit.Skip ();

                        // Create the greater than expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.GreaterThan,
                            left: expr,
                            right: ParseBitshift ()
                        );
                        continue;
                    case "<":
                        unit.Skip ();

                        // Create the less than expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.LessThan,
                            left: expr,
                            right: ParseBitshift ()
                        );
                        continue;
                    case ">=":
                        unit.Skip ();

                        // Create the less than or equal expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.LessThanOrEqual,
                            left: expr,
                            right: ParseBitshift ()
                        );
                        continue;
                    case "<=":
                        unit.Skip ();

                        // Create the greater than or equal expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.GreaterThanOrEqual,
                            left: expr,
                            right: ParseBitshift ()
                        );
                        continue;
                    case "is":
                        unit.Skip ();

                        // Create the compatible with expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.CompatibleWith,
                            left: expr,
                            right: ParseBitshift ()
                        );
                        continue;
                    case "isnot":
                        unit.Skip ();

                        // Create the not compatible with expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.NotCompatibleWith,
                            left: expr,
                            right: ParseBitshift ()
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
         * < expr > < '<<' | '>>' > < expr >
         */
        AstNode ParseBitshift () {
            var expr = ParseAdditive ();

            // Match all bitshift operators
            while (unit.Match (TokenClass.Operator)) {

                // Grab the bitshift operator
                var current = unit.Peek ();

                // Switch on the bitshift operator
                switch (current.Value) {
                    case "<<":
                        unit.Skip ();

                        // Create the bitwise shift left expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.ShiftLeft,
                            left: expr,
                            right: ParseAdditive ()
                        );
                        continue;
                    case ">>":
                        unit.Skip ();

                        // Create the bitwise shift right expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.ShiftRight,
                            left: expr,
                            right: ParseAdditive ()
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
         * < expr > < '+' | '-' > < expr >
         */
        AstNode ParseAdditive () {
            var expr = ParseMultiplicative ();

            // Match all additive operators
            while (unit.Match (TokenClass.Operator)) {

                // Grab the additive operator
                var current = unit.Peek ();

                // Switch on the additive operator
                switch (current.Value) {
                    case "+":
                        unit.Skip ();

                        // Create the addition expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Add,
                            left: expr,
                            right: ParseMultiplicative ()
                        );
                        continue;
                    case "-":
                        unit.Skip ();

                        // Create the subtraction expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Subtract,
                            left: expr,
                            right: ParseMultiplicative ()
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
         * < expr > < '*' | '/' | '%' > < expr >
         */
        AstNode ParseMultiplicative () {
            var expr = ParseUnary ();

            // Match all multiplicative operators
            while (unit.Match (TokenClass.Operator)) {

                // Grab the multiplicative operator
                var current = unit.Peek ();

                // Switch on the multiplicative operator
                switch (current.Value) {
                    case "*":
                        unit.Skip ();

                        // Create the multiplication expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Multiply,
                            left: expr,
                            right: ParseUnary ()
                        );
                        continue;
                    case "/":
                        unit.Skip ();

                        // Create the division expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Divide,
                            left: expr,
                            right: ParseUnary ()
                        );
                        continue;
                    case "%":
                        unit.Skip ();

                        // Create the modulo division expression
                        expr = new BinaryExpression (
                            location: current.Location,
                            op: BinaryOperation.Modulo,
                            left: expr,
                            right: ParseUnary ()
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
         * < '-' | '~' | '!' > < expr >
         */
        AstNode ParseUnary () {
            UnaryExpression expr;

            // Try matching a unary operator
            if (unit.Match (TokenClass.Operator)) {

                // Grab the unary operator
                var current = unit.Peek ();

                // Switch on the unary operator
                switch (current.Value) {
                    case "-":
                        unit.Skip ();

                        // Create the negation expression
                        expr = new UnaryExpression (
                            location: current.Location,
                            op: UnaryOperation.Negate,
                            child: ParseUnary ()
                        );
                        return expr;
                    case "~":
                        unit.Skip ();

                        // Create the bitwise not expression
                        expr = new UnaryExpression (
                            location: current.Location,
                            op: UnaryOperation.BitwiseNot,
                            child: ParseUnary ()
                        );
                        return expr;
                    case "!":
                        unit.Skip ();

                        // Create the boolean not expression
                        expr = new UnaryExpression (
                            location: current.Location,
                            op: UnaryOperation.LogicalNot,
                            child: ParseUnary ()
                        );
                        return expr;
                }
            }

            return ParseCallOrAccess ();
        }

        AstNode ParseCallOrAccess () {
            var term = ParseTerm ();
            return ParseCallOrAccess (term);
        }

        AstNode ParseCallOrAccess (AstNode left) {

            // Try parsing a function call
            if (unit.Match (TokenClass.OpenParen)) {

                // Create call expression
                var call = new CallExpression (unit.Location);
                call.SetTarget (left);

                // Parse the argument list
                call.SetArguments (ParseArgumentList ());

                // Return the expression
                return ParseCallOrAccess (call);
            }

            // Return the expression
            return left;
        }

        AstNode ParseTerm () {

            // Throw an exception if there are no more tokens to read
            if (!unit.See ())
                throw new ParserException (unit.Location).Describe ("Unexpected end of file.");

            // Grab the next token
            var current = unit.Peek ();

            // Switch on the token
            switch (current.Type) {
                case TokenClass.Identifier:
                    return ParseName ();
                case TokenClass.IntLiteral:
                    return ParseInteger ();
                case TokenClass.FloatLiteral:
                    return ParseFloat ();
                case TokenClass.OpenBracket:
                    return ParseList ();
                case TokenClass.OpenParen:
                    unit.Skip ();
                    var istuple = false;

                    // Parse the first expression
                    var expr = ParseExpression ();

                    // Create a temprary tuple expression
                    // that holds the arguments of the lambda expression
                    TupleExpression tmp = new TupleExpression (unit.Location);
                    tmp.Add (expr);

                    // Try parsing a tuple
                    TupleExpression tpl = null;
                    if (unit.Accept (TokenClass.Comma)) {
                        tpl = ParseTuple (expr);
                        istuple = true;
                    } else {

                        // Read the remaining parenthesis if
                        // the tuple was not successfuly parsed.
                        unit.Expect (TokenClass.CloseParen);
                    }

                    // Get the correct tuple expression
                    tpl = tpl ?? tmp;

                    // Try parsing a lambda expression
                    if (unit.Match (TokenClass.Operator, "=>")) {
                        // TODO: Implement lambda expressions
                        return null;
                        // return ParseLambda (tpl);
                    }

                    // Return the tuple if needed
                    if (istuple) {
                        return tpl;
                    }

                    // This is neither a tuple nor a lambda
                    return expr;
                case TokenClass.Keyword:

                    // TODO: Implement true, false, null, etc.
                    switch (unit.Peek ().Value) {
                        case "true":
                            unit.Skip ();
                            return null;
                        case "false":
                            unit.Skip ();
                            return null;
                        case "null":
                            unit.Skip ();
                            return null;
                    }
                    break;
                case TokenClass.StringLiteral:
                    return ParseString ();
            }

            // Unable to parse the term
            throw new ParserException (unit.Location).Describe ("Unexpected end of term.");
        }

        /*
         * '(' [ < expr > [ ',' < expr > ... ] ] ')'
         */
        ArgumentList ParseArgumentList () {
            var list = new ArgumentList (unit.Location);

            // Expect the opening parenthesis
            unit.Expect (TokenClass.OpenParen);

            // Match all arguments
            while (!unit.Match (TokenClass.CloseParen)) {

                // Parse the next expression
                list.Add (ParseExpression ());

                // Accept a comma
                if (!unit.Accept (TokenClass.Comma))
                    break;
            }

            // Expect the closing parenthesis
            unit.Expect (TokenClass.CloseParen);

            // Return the argument list
            return list;
        }

        /*
         * < number >
         */
        IntegerExpression ParseInteger () {
            return new IntegerExpression (unit.Location, BigInteger.Parse (unit.Expect (TokenClass.IntLiteral).Value));
        }

        /*
         * < float >
         */
        FloatExpression ParseFloat () {
            return new FloatExpression (unit.Location, float.Parse (unit.Expect (TokenClass.FloatLiteral).Value));
        }

        /*
         * < string >
         */
        StringExpression ParseString () {
            return new StringExpression (unit.Location, unit.Expect (TokenClass.StringLiteral).Value);
        }

        /*
         * < expr > [ ',' < expr > ... ] ')'
         */
        TupleExpression ParseTuple (AstNode initial) {

            // Create the tuple expression
            var tuple = new TupleExpression (unit.Location);

            // Add the initial value to the tuple
            tuple.Add (initial);

            // Match all expressions within the tuple
            while (!unit.Match (TokenClass.CloseParen)) {

                // Parse the next expression
                tuple.Add (ParseExpression ());

                // Accept a comma
                if (!unit.Accept (TokenClass.Comma))
                    break;
            }

            // Expect closing parenthesis
            unit.Expect (TokenClass.CloseParen);

            // Return the tuple expression
            return tuple;
        }

        /*
         * '[' < expr [ ',' < expr > ... ] > ']'
         */
        ListExpression ParseList () {

            // Create the list expression
            var list = new ListExpression (unit.Location);

            // Expect opening bracket
            unit.Expect (TokenClass.OpenBracket);

            // Match all expressions within the list
            while (!unit.Match (TokenClass.CloseBracket)) {

                // Parse the next expression
                list.Add (ParseExpression ());

                // Accept a comma
                if (!unit.Accept (TokenClass.Comma))
                    break;
            }

            // Expect closing bracket
            unit.Expect (TokenClass.CloseBracket);

            // Return the list expression
            return list;
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

