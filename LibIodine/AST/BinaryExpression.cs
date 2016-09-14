﻿using System;
namespace IodineCore {
    
    /// <summary>
    /// Binary expression node.
    /// </summary>
    public class BinaryExpression : AstNode {

        /// <summary>
        /// The operation.
        /// </summary>
        public readonly BinaryOperation Operation;

        /// <summary>
        /// The left node.
        /// </summary>
        public readonly AstNode Left;

        /// <summary>
        /// The right node.
        /// </summary>
        public readonly AstNode Right;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryExpression"/> class.
        /// </summary>
        /// <param name="location">Location.</param>
        public BinaryExpression (
            SourceLocation location,
            BinaryOperation op,
            AstNode left,
            AstNode right): base (location) {
            Operation = op;
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Visit the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor.</param>
        public override void Visit (AstVisitor visitor) {
            visitor.Accept (this);
        }

        /// <summary>
        /// Visits the children.
        /// </summary>
        /// <returns>The children.</returns>
        /// <param name="visitor">Visitor.</param>
        public override void VisitChildren (AstVisitor visitor) {
            Left.Visit (visitor);
            Right.Visit (visitor);
        }

        public override string ToString () => $"[Binary: Op={Operation}]";
    }
}

