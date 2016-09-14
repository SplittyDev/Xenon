﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace IodineCore {
    
    /// <summary>
    /// Parsing unit.
    /// </summary>
    public class ParsingUnit {

        /// <summary>
        /// The cursor.
        /// </summary>
        readonly SourceCursor cursor;

        /// <summary>
        /// The tokens.
        /// </summary>
        readonly List<Lexeme> tokens;

        /// <summary>
        /// Gets the current lexeme.
        /// </summary>
        /// <value>The current lexeme.</value>
        public Lexeme Current => See (0) ? Peek () : null;

        /// <summary>
        /// Gets the source location of the current lexeme.
        /// </summary>
        /// <value>The source location of the current lexeme.</value>
        public SourceLocation Location => Peek ().Location;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingUnit"/> class.
        /// </summary>
        /// <param name="lexemes">Lexemes.</param>
        ParsingUnit (List<Lexeme> lexemes) {
            this.tokens = lexemes;
            cursor = new SourceCursor (this.tokens.Count);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ParsingUnit"/> class.
        /// </summary>
        /// <param name="lexemes">Lexemes</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static ParsingUnit Create (List<Lexeme> lexemes) => new ParsingUnit (lexemes);

        /// <summary>
        /// Determines whether the specified number of characters can be safely processed.
        /// </summary>
        /// <param name="lookahead">Lookahead.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool See (int lookahead = 1) => cursor.Position + lookahead < tokens.Count;

        /// <summary>
        /// Peeks at the lexeme located at <paramref name="pos"/>
        /// relative to the cursor position.
        /// </summary>
        /// <param name="pos">Position.</param>
        public Lexeme Peek (int pos = 0) {
            Contract.Assert (See (pos));
            return tokens[cursor.Position + pos];
        }

        /// <summary>
        /// Skips <paramref name="n"/> lexemes.
        /// </summary>
        /// <param name="n">The number of lexemes to skip.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Skip (int n = 1) => cursor.Move (n);

        /// <summary>
        /// Match the specified token.
        /// </summary>
        /// <param name="token">Token.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool Match (TokenClass token) => Current != null && Current.Type == token;

        /// <summary>
        /// Match the specified string.
        /// </summary>
        /// <param name="strval">Strval.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool Match (string strval) => Current != null && Current.Value == strval;

        /// <summary>
        /// Match the specified token and string.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <param name="strval">Strval.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public bool Match (TokenClass token, string strval) => Current != null && Match (token) && Match (strval);

        /// <summary>
        /// Read the current token.
        /// </summary>
        public Lexeme Read () {
            var lex = Current;
            Skip ();
            return lex;
        }

        /// <summary>
        /// Accept the specified token.
        /// </summary>
        /// <param name="token">Token.</param>
        public bool Accept (TokenClass token) {
            var match = Match (token);
            Skip (match ? 1 : 0);
            return match;
        }

        /// <summary>
        /// Accept the specified string.
        /// </summary>
        /// <param name="strval">Strval.</param>
        public bool Accept (string strval) {
            var match = Match (strval);
            Skip (match ? 1 : 0);
            return match;
        }

        /// <summary>
        /// Accept the specified token and value.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <param name="strval">Strval.</param>
        public bool Accept (TokenClass token, string strval) {
            var match = Match (token) && Match (strval);
            Skip (match ? 1 : 0);
            return match;
        }

        /// <summary>
        /// Expect the specified token.
        /// </summary>
        /// <param name="token">Token.</param>
        public Lexeme Expect (TokenClass token) {
            var current = Current;
            if (Accept (token)) {
                return current;
            }
            if (!See ()) {
                throw ParserException.Create (Location).Describe ("Unexpected end of file.");
            }
            var next = Read ();
            throw ParserException.Create (Location).Describe ($"Unexpected token: '{next.Value}' ({next.Type})");
        }

        /// <summary>
        /// Expect the specified string.
        /// </summary>
        /// <param name="strval">Strval.</param>
        public Lexeme Expect (string strval) {
            var current = Current;
            if (Accept (strval)) {
                return current;
            }
            if (!See ()) {
                throw ParserException.Create (Location).Describe ("Unexpected end of file.");
            }
            var next = Read ();
            throw ParserException.Create (Location).Describe ($"Unexpected token: '{next.Value}' ({next.Type})");
        }

        /// <summary>
        /// Expect the specified token and string.
        /// </summary>
        /// <param name="token">Token.</param>
        /// <param name="strval">Strval.</param>
        public Lexeme Expect (TokenClass token, string strval) {
            var current = Current;
            if (Accept (token, strval)) {
                return current;
            }
            if (!See ()) {
                throw ParserException.Create (Location).Describe ("Unexpected end of file.");
            }
            var next = Read ();
            throw ParserException.Create (Location).Describe ($"Unexpected token: '{next.Value}' ({next.Type})");
        }
    }
}

