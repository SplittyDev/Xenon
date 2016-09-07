using System;
namespace XenonCore {

    /// <summary>
    /// Token class.
    /// </summary>
    public enum TokenClass {
        
        /// <summary>
        /// A unicode string literal.
        /// </summary>
        StringLiteral,

        /// <summary>
        /// A unicode binary string literal.
        /// </summary>
        BinaryStringLiteral,

        /// <summary>
        /// A unicode template string literal.
        /// Allows for string interpolation.
        /// </summary>
        TemplateStringLiteral,

        /// <summary>
        /// A whole number.
        /// </summary>
        IntLiteral,

        /// <summary>
        /// A floating-point number.
        /// </summary>
        FloatLiteral,

        /// <summary>
        /// A reserved language-keyword.
        /// </summary>
        Keyword,

        /// <summary>
        /// A reserved language-operator.
        /// </summary>
        Operator,

        /// <summary>
        /// A user-defined identifier.
        /// </summary>
        Identifier,

        /// <summary>
        /// Member access operator.
        /// </summary>
        MemberAccess,

        /// <summary>
        /// Member default access operator.
        /// Performs null-conditional member access.
        /// </summary>
        MemberDefaultAccess,

        /// <summary>
        /// Opening curly brace.
        /// </summary>
        OpenBrace,

        /// <summary>
        /// Closing curly brace.
        /// </summary>
        CloseBrace,

        /// <summary>
        /// Opening parenthesis.
        /// </summary>
        OpenParen,

        /// <summary>
        /// Closing parenthesis.
        /// </summary>
        CloseParen,

        /// <summary>
        /// Opening bracket.
        /// </summary>
        OpenBracket,

        /// <summary>
        /// Closing bracket.
        /// </summary>
        CloseBracket,

        /// <summary>
        /// Semicolon.
        /// </summary>
        Semicolon,

        /// <summary>
        /// Colon.
        /// </summary>
        Colon,

        /// <summary>
        /// Comma.
        /// </summary>
        Comma,

        /// <summary>
        /// A hint for source code analysis.
        /// </summary>
        SourceAnalysisHint,
    }
}

