using System;
namespace XenonCore {

    /// <summary>
    /// Lexer constants.
    /// </summary>
    public static class LexerConstants {

        /// <summary>
        /// Operators.
        /// </summary>
        public static readonly string OperatorChars = "+-*/=<>~!&^|%@?.";

        /// <summary>
        /// Operator strings.
        /// </summary>
        public static readonly string[] OperatorStrings = "and,or,is,isnot,as".Split (',');

        /// <summary>
        /// Keywords.
        /// </summary>
        public static readonly string[] Keywords = {
            // control flow
            "if",
            "else",
            "do",
            "while",
            "for",
            "in",
            "break",
            "continue",
            "match",
            "case",
            "default",
            "yield",
            "return",
            // type definitions
            "fn",
            "class",
            "enum",
            "contract",
            "trait",
            "mixin",
            "extend",
            // class stuff
            "super",
            "self",
            // constants
            "true",
            "false",
            "null",
            // exceptions
            "raise",
            "try",
            "except",
            // packages
            "use",
            "from",
            // other
            "with"
        };
    }
}

