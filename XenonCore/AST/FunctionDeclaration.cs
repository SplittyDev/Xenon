using System;
using System.Collections.Generic;

namespace XenonCore {

    /// <summary>
    /// Function declaration.
    /// </summary>
    public class FunctionDeclaration : AstNode {

        /// <summary>
        /// The body.
        /// </summary>
        public readonly AstNode Body;

        /// <summary>
        /// The arguments.
        /// </summary>
        public readonly List<NamedParameter> Arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.FunctionDeclaration"/> class.
        /// </summary>
        /// <param name="body">Body.</param>
        public FunctionDeclaration (SourceLocation location, AstNode body) : base (location) {
            this.Body = body;
            Arguments = new List<NamedParameter> ();
        }

        public override void Visit (AstVisitor visitor) {
            visitor.Accept (this);
        }
    }
}

