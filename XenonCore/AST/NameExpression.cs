using System;
namespace XenonCore {
    
    /// <summary>
    /// Name expression node.
    /// </summary>
    public class NameExpression : AstNode {

        /// <summary>
        /// The name.
        /// </summary>
        public readonly string Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="NameExpression"/> class.
        /// </summary>
        /// <param name="location">Location.</param>
        public NameExpression (SourceLocation location, string name) : base (location) {
            Value = name;
        }

        /// <summary>
        /// Visit the specified visitor.
        /// </summary>
        /// <param name="visitor">Visitor.</param>
        public override void Visit (AstVisitor visitor) {
            visitor.Accept (this);
        }

        public override string ToString () => $"[Name: Value='{Value}']";
    }
}

