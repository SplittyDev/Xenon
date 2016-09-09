using System;
namespace XenonCore {

    /// <summary>
    /// Float expression node.
    /// </summary>
    public class FloatExpression : AstNode {

        /// <summary>
        /// The unsigned value.
        /// </summary>
        float value;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public float Value => value;

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatExpression"/> class.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <param name="fval">Fval.</param>
        public FloatExpression (SourceLocation location, float fval) : base (location) {
            value = fval;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="fval">Value.</param>
        public void SetValue (float fval) {
            value = fval;
        }

        /// <summary>
        /// Visit the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor.</param>
        public override void Visit (AstVisitor visitor) {
            visitor.Accept (this);
        }

        public override string ToString () => $"[Float: Value='{value}']";
    }
}

