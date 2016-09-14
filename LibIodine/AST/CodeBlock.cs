using System;
using System.Collections.Generic;
using System.Text;

namespace IodineCore {
    
    /// <summary>
    /// Abstract Syntax Tree Block.
    /// </summary>
    public sealed class CodeBlock : AstNode {

        /// <summary>
        /// The children.
        /// </summary>
        public readonly List<AstNode> Children;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeBlock"/> class.
        /// </summary>
        /// <param name="location">Location.</param>
        public CodeBlock (SourceLocation location) : base (location) {
            Children = new List<AstNode> ();
        }

        /// <summary>
        /// Adds a child to the node.
        /// </summary>
        /// <returns>The child.</returns>
        /// <param name="node">Node.</param>
        public void AddChild (AstNode node) {
            Children.Add (node);
        }

        /// <summary>
        /// Merge this code block with another one.
        /// </summary>
        /// <param name="other">The other code block.</param>
        public void Merge (CodeBlock other) {
            Children.AddRange (other.Children);
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
            Children.ForEach (child => child.Visit (visitor));
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString () {
            return "[CodeBlock]";
        }
    }
}

