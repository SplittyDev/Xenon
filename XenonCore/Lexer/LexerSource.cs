using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace XenonCore {

    /// <summary>
    /// Lexer source.
    /// </summary>
    public class LexerSource {

        /// <summary>
        /// The position.
        /// </summary>
        int pos;

        /// <summary>
        /// The line.
        /// </summary>
        int line;

        /// <summary>
        /// The position on the line.
        /// </summary>
        int linepos;

        /// <summary>
        /// The brace balance.
        /// </summary>
        int bracebalance;

        /// <summary>
        /// The source.
        /// </summary>
        string source;

        /// <summary>
        /// Gets the line.
        /// </summary>
        /// <value>The line.</value>
        public int Line => line;

        /// <summary>
        /// Gets the position on the line.
        /// </summary>
        /// <value>The position on the line.</value>
        public int Linepos => linepos;

        /// <summary>
        /// Gets the brace balance.
        /// </summary>
        /// <value>The brace balance.</value>
        public int BraceBalance => bracebalance;

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>The location.</value>
        public string Location => $"{line}:{linepos}";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.LexerSource"/> class.
        /// </summary>
        /// <param name="source">Source.</param>
        public LexerSource (string source) {
            line = 1;
            this.source = source;
        }

        /// <summary>
        /// Increments the brace balance.
        /// </summary>
        public void OpenBrace () => bracebalance++;

        /// <summary>
        /// Decrements the brace balance.
        /// </summary>
        public void CloseBrace () => bracebalance--;

        /// <summary>
        /// Skips <paramref name="n"/> characters.
        /// </summary>
        /// <param name="n">The amount of characters to skip.</param>
        public void Skip (int n = 1) {
            Contract.Assert (See (n));
            for (var i = 0; i < n; i++) {
                var c = Peek (i);
                if (c == '\n') {
                    line++;
                    linepos = 0;
                } else
                    linepos++;
            }
            pos += n;
        }

        /// <summary>
        /// Skips whitespace.
        /// </summary>
        public void SkipWhitespace () {
            while (See (1) && char.IsWhiteSpace (Peek ()))
                Skip ();
        }

        /// <summary>
        /// Skips a line.
        /// </summary>
        public void SkipLine () {
            while (Peek () != '\n')
                Skip ();
        }

        /// <summary>
        /// Tests if the specified relative position can be advanced to.
        /// </summary>
        /// <param name="lookahead">Lookahead.</param>
        public bool See (int lookahead = 1) => pos + lookahead < source.Length;

        /// <summary>
        /// Peeks at the specified relative position.
        /// </summary>
        /// <param name="lookahead">Lookahead.</param>
        public char Peek (int lookahead = 0) {
            Contract.Assert (See (lookahead));
            return source[pos + lookahead];
        }

        /// <summary>
        /// Peeks at <paramref name="count"/> characters starting at the specified relative position.
        /// </summary>
        /// <param name="count">Count.</param>
        /// <param name="lookahead">Lookahead.</param>
        public string Peeks (int count, int lookahead = 0) {
            Contract.Assert (See (count + lookahead));
            return source.Substring (pos + lookahead, count);
        }

        /// <summary>
        /// Reads the character at the specified position.
        /// </summary>
        /// <param name="lookahead">Lookahead.</param>
        public char Read (int lookahead = 0) {
            var c = Peek (lookahead);
            Skip (1 + lookahead);
            return c;
        }

        /// <summary>
        /// Reads <paramref name="count"/> characters starting at the specified position.
        /// </summary>
        /// <param name="count">Count.</param>
        /// <param name="lookahead">Lookahead.</param>
        public string Reads (int count, int lookahead = 0) {
            var str = Peeks (count, lookahead);
            Skip (lookahead + count);
            return str;
        }

        /// <summary>
        /// Reads a line.
        /// </summary>
        /// <returns>The line.</returns>
        public string ReadLine () {
            var accum = new StringBuilder ();
            while (Peek () != '\n')
                accum.Append (Read ());
            return accum.ToString ();
        }
    }
}

