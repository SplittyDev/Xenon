using System;
using System.Numerics;

namespace XenonCore {
    
    /// <summary>
    /// Integer expression node.
    /// </summary>
    public class IntegerExpression : AstNode {

        /// <summary>
        /// The value.
        /// </summary>
        BigInteger value;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public BigInteger Value => value;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerExpression"/> class.
        /// </summary>
        /// <param name="location">Location.</param>
        public IntegerExpression (SourceLocation location, BigInteger value) : base (location) {
            this.value = value;
        }

        /// <summary>
        /// Visit the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor.</param>
        public override void Visit (AstVisitor visitor) {
            visitor.Accept (this);
        }

        public override string ToString () => $"[Integer: Value='{value}']";
    }
}

