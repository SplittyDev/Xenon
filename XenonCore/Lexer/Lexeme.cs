using System;
using System.Text;

namespace XenonCore {

    /// <summary>
    /// Lexeme.
    /// </summary>
    public class Lexeme {

        /// <summary>
        /// The token type.
        /// </summary>
        public TokenClass Type;

        /// <summary>
        /// The value.
        /// </summary>
        public string Value;

        /// <summary>
        /// The literal value.
        /// </summary>
        public object LiteralValue;

        /// <summary>
        /// The location.
        /// </summary>
        public SourceLocation Location;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.Lexeme"/> class.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="unit">Unit.</param>
        /// <param name="value">Value.</param>
        /// <param name="literal">Literal.</param>
        public Lexeme (TokenClass type, SourceUnit unit, string value, object literal = null) {
            Type = type;
            Value = value;
            LiteralValue = literal ?? value;
            Location = unit.Location;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.Lexeme"/> class.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="unit">Unit.</param>
        /// <param name="value">Value.</param>
        /// <param name="literal">Literal.</param>
        public Lexeme (TokenClass type, SourceUnit unit, StringBuilder value, object literal = null)
            : this (type, unit, value.ToString (), literal) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.Lexeme"/> class.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="unit">Unit.</param>
        /// <param name="value">Value.</param>
        /// <param name="literal">Literal.</param>
        public Lexeme (TokenClass type, SourceUnit unit, char value, object literal = null)
            : this (type, unit, value.ToString (), literal) { }

        /// <summary>
        /// Verifies the type of the lexeme.
        /// </summary>
        /// <param name="type">Type.</param>
        public bool Is (TokenClass type) {
            return Type == type;
        }

        /// <summary>
        /// Verifies the value of the lexeme.
        /// </summary>
        /// <param name="str">String.</param>
        public bool Is (string str) {
            return Value == str;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:XenonCore.Lexeme"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:XenonCore.Lexeme"/>.</returns>
        public override string ToString () {
            return $"[Lexeme: Type={Type.ToString ()}, Value={Value}]";
        }
    }
}

