﻿using System;
namespace IodineCore {

    /// <summary>
    /// String expression node.
    /// </summary>
    public class StringExpression : AstNode {

        /// <summary>
        /// The value.
        /// </summary>
        string value;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value => value;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringExpression"/> class.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <param name="value">Value.</param>
        public StringExpression (SourceLocation location, string value) : base (location) {
            this.value = value;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="value">Value.</param>
        public void SetValue (string value) {
            this.value = value;
        }

        /// <summary>
        /// Visit the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor.</param>
        public override void Visit (AstVisitor visitor) {
            visitor.Accept (this);
        }

        public override string ToString () => $"[String: Value='{Value}']";
    }
}

