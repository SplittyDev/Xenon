using System;
using System.Collections.Generic;

namespace XenonCore {

    /// <summary>
    /// List node.
    /// </summary>
    public class ListExpression : AstNode {

        /// <summary>
        /// The arguments.
        /// </summary>
        public readonly List<AstNode> Arguments;

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count => Arguments.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListExpression"/> class.
        /// </summary>
        /// <param name="location">Location.</param>
        public ListExpression (SourceLocation location) : base (location) {
            Arguments = new List<AstNode> ();
        }

        /// <summary>
        /// Add the specified node.
        /// </summary>
        /// <param name="node">Node.</param>
        public void Add (AstNode node) => Arguments.Add (node);

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
            Arguments.ForEach (node => node.Visit (visitor));
        }

        public override string ToString () => $"[List: Count={Arguments.Count}]";
    }
}

