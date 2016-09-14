using System;
namespace IodineCore {

    /// <summary>
    /// Ternary expression.
    /// </summary>
    public class TernaryExpression : AstNode {

        /// <summary>
        /// The condition.
        /// </summary>
        public readonly AstNode Condition;

        /// <summary>
        /// The left expression.
        /// </summary>
        public readonly AstNode Left;

        /// <summary>
        /// The right expression.
        /// </summary>
        public readonly AstNode Right;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.TernaryExpression"/> class.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <param name="condition">Condition.</param>
        /// <param name="left">Left.</param>
        /// <param name="right">Right.</param>
        public TernaryExpression (
            SourceLocation location,
            AstNode condition,
            AstNode left,
            AstNode right)
            : base (location) {
            Condition = condition;
            Left = left;
            Right = right;
        }

        public override void Visit (AstVisitor visitor) {
            visitor.Accept (this);
        }

        public override void VisitChildren (AstVisitor visitor) {
            Condition.Visit (visitor);
            Left.Visit (visitor);
            Right.Visit (visitor);
        }
    }
}

