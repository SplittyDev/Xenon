using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using static IodineCore.LexerConstants;

namespace IodineCore {

    /// <summary>
    /// Lexer.
    /// </summary>
    public class Lexer {

        /// <summary>
        /// The source.
        /// </summary>
        readonly SourceUnit source;

        /// <summary>
        /// The lexemes.
        /// </summary>
        readonly List<Lexeme> lexemes;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.Lexer"/> class.
        /// </summary>
        /// <param name="source">Source.</param>
        public Lexer (string source) {
            lexemes = new List<Lexeme> ();
            this.source = SourceUnit.FromSource (source);
        }

        /// <summary>
        /// Tokenizes the source.
        /// </summary>
        public List<Lexeme> Scan () {
            while (source.See (1))
                ScanToken ();
            return lexemes;
        }

        /// <summary>
        /// Scans a token.
        /// </summary>
        void ScanToken () {
            var c = source.Peek ();

            // Test for whitespace
            if (char.IsWhiteSpace (source.Peek ())) {

                // Skip the whitespace
                source.SkipWhitespace ();
                return;
            }

            // Test for hexadecimal number
            if (source.See (2) && source.Peeks (2) == "0x") {

                // Read a hexadecimal number
                ReadHexNumber ();
                return;
            }

            // Test for decimal number
            if (char.IsDigit (c)) {

                // Read a decimal number
                ReadNumber ();
                return;
            }

            // Test for single-character operator
            if (OperatorChars.Contains (c.ToString ())) {

                // Read an operator
                ReadOperator ();
                return;
            }

            // Build key-value-pairs of characters and their
            // corresponding token classes.
            var dict = new Dictionary<char, TokenClass> {
                { '{', TokenClass.OpenBrace },
                { '}', TokenClass.CloseBrace },
                { '(', TokenClass.OpenParen },
                { ')', TokenClass.CloseParen },
                { '[', TokenClass.OpenBracket },
                { ']', TokenClass.CloseBracket },
                { ';', TokenClass.Semicolon },
                { ':', TokenClass.Colon },
                { ',', TokenClass.Comma }
            };

            // Test for those characters
            if (dict.ContainsKey (c)) {

                // Skip if advancing is possible
                if (source.See ())
                    source.Skip ();

                // Add the lexeme
                lexemes.Add (new Lexeme (dict[c], source, c));
                return;
            }

            // Test for other stuff
            switch (c) {
                case '#':

                    // Skip a character
                    source.Skip ();

                    // Read a comment line
                    var comment = source.ReadLine ();

                    // Test if the line cotnains source analysis hints
                    if (comment.TrimStart ().StartsWith ("analysis", StringComparison.Ordinal)) {

                        // Parse the source analysis hint
                        var parts = comment.Trim ().Split (' ');
                        if (parts.Length != 3) break;
                        var action = parts[1].Trim ();
                        var name = parts[2].Trim ();

                        // Add the source analysis hint
                        lexemes.Add (new Lexeme (TokenClass.SourceAnalysisHint, source, $"{action}:{name}"));
                    }
                    break;
                case '\'':
                case '"':
                case '`':

                    // Read a string
                    ReadString ();
                    break;
                default:

                    // Test for binary string
                    if (source.See () && c == 'b' && (source.Peek (1) == '\"' || source.Peek (1) == '\'')) {

                        // Read a binary string
                        ReadBinaryString ();
                        break;
                    }

                    // Test for identifier
                    if (char.IsLetter (c) || c == '_') {

                        // Read an identifier
                        ReadIdentifier ();
                        break;
                    }

                    // The token was not handled by any of the above code; Throw an exception
                    throw new Exception ($"Unexpected token: '{c}' at {source.Location}.");
            }
        }

        string JustReadString (out char delimiter) {
            var accum = new StringBuilder ();
            delimiter = source.Read ();
            var c = source.Peek ();
            while (source.See () && c != delimiter) {
                c = source.Read ();
                if (c == '\\') {
                    var next = source.Peek ();
                    var dict = new Dictionary<char, char> {
                        [ '\"' ] = '\"',
                        [ '\'' ] = '\'',
                        [ 'n'  ] = '\n',
                        [ 'r'  ] = '\r',
                        [ 'b'  ] = '\b',
                        [ 't'  ] = '\t',
                        [ 'f'  ] = '\f'
                    };
                    if (!dict.ContainsKey (next))
                        throw new Exception ($"Unrecognized escape sequence: '\\{next}'.");
                    c = dict[next];
                    source.Skip ();
                }
                /*
                else if (delimiter == '"' && c == '#' && source.Peek () == '{') {
                    source.Skip ();
                    var balance = source.BraceBalance;
                    while (lexemes.Last ().Value != "}" && source.BraceBalance - balance == 0)
                        ScanToken ();
                    source.Skip ();
                    source.CloseBrace ();
                    continue;
                }
                */
                accum.Append (c);
                c = source.Peek ();
            }
            if (c != delimiter)
                throw new Exception ($"Unterminated string literal at {source.Location}.");
            if (source.See ())
                source.Skip ();
            return accum.ToString ();
        }

        void ReadString () {
            char delimiter;
            var str = JustReadString (out delimiter);
            var tokenclass = delimiter == '`' ? TokenClass.TemplateStringLiteral : TokenClass.StringLiteral;
            lexemes.Add (new Lexeme (tokenclass, source, str));
        }

        void ReadBinaryString () {
            char _;
            var str = JustReadString (out _);
            lexemes.Add (new Lexeme (TokenClass.BinaryStringLiteral, source, str));
        }

        void ReadNumber () {
            var accum = new StringBuilder ();
            var c = source.Peek ();
            var isfloat = false;
            while (source.See () && (char.IsDigit (c) || c == '.')) {
                if (c == '.' && !isfloat)
                    isfloat = true;
                else if (c == '.')
                    throw new Exception ("Floating point numbers can only have one decimal point.");
                accum.Append (c);
                source.Skip ();
                c = source.Peek ();
            }
            if (isfloat) {
                lexemes.Add (new Lexeme (TokenClass.FloatLiteral, source, accum));
                return;
            }
            lexemes.Add (new Lexeme (TokenClass.IntLiteral, source, accum));
        }

        void ReadHexNumber () {
            var accum = new StringBuilder ();
            source.Skip (2);
            var c = source.Peek ();
            while (source.See () && (char.IsDigit (c) || "abcdefABCDEF".Contains (c.ToString ()))) {
                accum.Append (c);
                source.Skip ();
                c = source.Peek ();
            }
            lexemes.Add (new Lexeme (TokenClass.IntLiteral, source, BigInteger.Parse ("0" + accum, NumberStyles.HexNumber).ToString ()));
        }

        void ReadOperator () {
            var op = source.Peek ();
            string op2 = source.Peeks (2);
            string op3 = source.Peeks (3);
            switch (op3) {
                case "<<=":
                case ">>=":
                case "...":
                    source.Skip (3);
                    lexemes.Add (new Lexeme (TokenClass.Operator, source, op3));
                    break;
            }
            switch (op2) {
                case ">>":
                case "<<":
                case "==":
                case "!=":
                case "=>":
                case "<=":
                case ">=":
                case "+=":
                case "-=":
                case "*=":
                case "/=":
                case "%=":
                case "&=":
                case "^=":
                case "|=":
                case "??":
                case "..":
                    source.Skip (2);
                    lexemes.Add (new Lexeme (TokenClass.Operator, source, op2));
                    break;
                case ".?":
                    source.Skip (2);
                    lexemes.Add (new Lexeme (TokenClass.MemberDefaultAccess, source, op2));
                    break;
                case "/*":
                    source.Skip (2);
                    var nn = source.Peeks (2);
                    while (nn != "*/") {
                        source.Skip ();
                        nn = source.Peeks (2);
                    }
                    break;
            }
            source.Skip ();
            if (op == '.') {
                lexemes.Add (new Lexeme (TokenClass.MemberAccess, source, op));
                return;
            }
            lexemes.Add (new Lexeme (TokenClass.Operator, source, op.ToString ()));
        }

        void ReadIdentifier () {
            var accum = new StringBuilder ();
            var c = source.Peek ();
            while (char.IsLetterOrDigit (c) || c == '_') {
                accum.Append (c);
                if (!source.See ())
                    break;
                source.Skip ();
                c = source.Peek ();
            }
            var str = accum.ToString ();
            if (Keywords.Contains (str)) {
                lexemes.Add (new Lexeme (TokenClass.Keyword, source, str));
                return;
            }
            if (OperatorStrings.Contains (str)) {
                lexemes.Add (new Lexeme (TokenClass.Operator, source, str));
                return;
            }
            lexemes.Add (new Lexeme (TokenClass.Identifier, source, str));
        }
    }
}

