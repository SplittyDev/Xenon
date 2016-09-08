using System;
namespace XenonCore {
    
    /// <summary>
    /// Ast visitor.
    /// </summary>
    public abstract class AstVisitor {

        /// <summary>
        /// The source location.
        /// </summary>
        public SourceLocation Location;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.AstVisitor"/> class.
        /// </summary>
        protected AstVisitor () {
            Location = SourceLocation.Zero;
        }

        public virtual void Accept (AstRoot node) => Update (node);
        public virtual void Accept (NameExpression node) => Update (node);
        public virtual void Accept (FunctionDeclaration node) => Update (node);
        public virtual void Accept (CodeBlock node) => Update (node);
        public virtual void Accept (BinaryExpression node) => Update (node);
        public virtual void Accept (TernaryExpression node) => Update (node);

        /// <summary>
        /// Updates the source location.
        /// </summary>
        /// <param name="node">Node.</param>
        void Update (AstNode node) => Location = node.Location;
    }
}

