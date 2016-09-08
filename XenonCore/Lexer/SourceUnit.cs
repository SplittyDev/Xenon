using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace XenonCore {
    
    /// <summary>
    /// Source unit.
    /// </summary>
    public class SourceUnit {

        /// <summary>
        /// The cursor.
        /// </summary>
        readonly SourceCursor cursor;

        /// <summary>
        /// The current line.
        /// </summary>
        int line;

        /// <summary>
        /// The position at the current line.
        /// </summary>
        int linepos;

        /// <summary>
        /// The source.
        /// </summary>
        string source;

        /// <summary>
        /// Gets the cursor line.
        /// </summary>
        /// <value>The current line.</value>
        public int Line => line;

        /// <summary>
        /// Gets the cursor position at the current line.
        /// </summary>
        /// <value>The position.</value>
        public int Position => linepos;

        /// <summary>
        /// Gets the absolute cursor position.
        /// </summary>
        /// <value>The cursor position.</value>
        public int CursorPosition => cursor.Position;

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <value>The source.</value>
        internal string Source => source;

        /// <summary>
        /// Gets the cursor location.
        /// </summary>
        /// <value>The cursor location.</value>
        public SourceLocation Location => new SourceLocation (line, linepos);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:LexDotNet.SourceUnit"/> class.
        /// </summary>
        /// <param name="source">Source.</param>
        SourceUnit (string source) {
            this.source = source;
            cursor = new SourceCursor (source.Length);
            line = 1;
        }

        /// <summary>
        /// Creates a new <see cref="T:LexDotNet.SourceUnit"/> from the specified source string.
        /// </summary>
        /// <returns>The source.</returns>
        /// <param name="source">Source.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SourceUnit FromSource (string source) {
            Contract.Ensures (Contract.Result<SourceUnit> () != null);
            return new SourceUnit (source);
        }

        /// <summary>
        /// Creates a new <see cref="T:LexDotNet.SourceUnit"/> from the specified source file.
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="path">Path.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SourceUnit FromFile (string path) {
            Contract.Ensures (Contract.Result<SourceUnit> () != null);
            var file = File.OpenRead (path);
            return FromFile (file, releaseHandle: true);
        }

        /// <summary>
        /// Creates a new <see cref="T:LexDotNet.SourceUnit"/> from the specified source file.
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="file">File.</param>
        /// <param name="releaseHandle">Release handle.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static SourceUnit FromFile (FileStream file, bool releaseHandle = true) {
            Contract.Ensures (Contract.Result<SourceUnit> () != null);
            var reader = new StreamReader (
                stream: file,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: true,
                bufferSize: 256,
                leaveOpen: true
            );
            string source;
            using (reader) {
                source = reader.ReadToEnd ();
            }
            if (releaseHandle)
                file.Close ();
            return FromSource (source);
        }

        /// <summary>
        /// Skips <paramref name="n"/> characters.
        /// </summary>
        /// <param name="n">The number of characters to skip.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Skip (int n = 1) {
            Contract.Assert (See (n - 1));
            for (var i = 0; i < n; i++) {
                var c = Peek (i);
                if (c == '\n') {
                    line++;
                    linepos = 0;
                } else
                    linepos++;
            }
            cursor.Move (n);
        }

        /// <summary>
        /// Advances the cursor while the cursor is pointing at a whitespace character.
        /// </summary>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void SkipWhitespace () {
            while (See (0) && char.IsWhiteSpace (Peek ()))
                Skip ();
        }

        /// <summary>
        /// Advances the cursor until the cursor points at a line break (\n) character.
        /// </summary>
        /// <returns>The line.</returns>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void SkipLine () {
            while (Peek () != '\n')
                Skip ();
        }

        /// <summary>
        /// Determines whether the specified number of characters can be safely processed.
        /// </summary>
        /// <param name="lookahead">Lookahead.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool See (int lookahead = 1) => cursor.Position + lookahead < source.Length;

        /// <summary>
        /// Peeks at the character located at <paramref name="pos"/>
        /// relative to the cursor position.
        /// </summary>
        /// <param name="pos">Position.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public char Peek (int pos = 0) {
            Contract.Assert (See (pos));
            return source[cursor.Position + pos];
        }

        /// <summary>
        /// Peeks at a string of <paramref name="count"/> characters starting
        /// at <paramref name="pos"/> relative to the cursor position.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="pos">The position.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public string Peeks (int count, int pos = 0) {
            Contract.Assert (See (count + pos));
            Contract.Ensures (Contract.Result<string> () != null);
            if (count == 1)
                return source[cursor.Position + pos].ToString ();
            return source.Substring (cursor.Position + pos, count);
        }

        /// <summary>
        /// Reads the character located at <paramref name="pos"/>
        /// relative to the cursor position.
        /// Advances the cursor by <paramref name="pos"/> units.
        /// </summary>
        /// <param name="pos">The position.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public char Read (int pos = 0) {
            var c = Peek (pos);
            Skip (1 + pos);
            return c;
        }

        /// <summary>
        /// Reads a string of <paramref name="count"/> characters starting
        /// at <paramref name="pos"/> relative to the cursor position.
        /// Advances the cursor by (<paramref name="pos"/> + <paramref name="count"/>) units.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="pos">The position.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public string Reads (int count, int pos = 0) {
            Contract.Ensures (Contract.Result<string> () != null);
            var str = Peeks (count, pos);
            Skip (pos + count);
            return str;
        }

        /// <summary>
        /// Reads a string of characters until the cursor points at
        /// a line break (\n) character.
        /// Advances the cursor by an unknown number of units.
        /// </summary>
        /// <returns>The line.</returns>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public string ReadLine () {
            Contract.Ensures (Contract.Result<string> () != null);
            var accum = new StringBuilder ();
            while (Peek () != '\n')
                accum.Append (Read ());
            return accum.ToString ();
        }
    }
}

