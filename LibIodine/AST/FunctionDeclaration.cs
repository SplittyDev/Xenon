using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IodineCore {

    /// <summary>
    /// Function declaration.
    /// </summary>
    public class FunctionDeclaration : AstNode {

        /// <summary>
        /// The body.
        /// </summary>
        public AstNode Body;

        /// <summary>
        /// The name.
        /// </summary>
        public NameExpression Name;

        /// <summary>
        /// The arguments.
        /// </summary>
        public readonly List<NamedParameter> Arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.FunctionDeclaration"/> class.
        /// </summary>
        /// <param name="body">Body.</param>
        public FunctionDeclaration (SourceLocation location) : base (location) {
            Arguments = new List<NamedParameter> ();
        }

        public override void Visit (AstVisitor visitor) {
            visitor.Accept (this);
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString () {
            var accum = new StringBuilder ();
            accum.Append ($"[Function: Name='{Name.Value}'");
            if (Arguments.Count > 0) {
                var args = string.Join (", ", Arguments.Select (a => a.Name.Value));
                accum.Append ($" Arguments=[{args}]");
            }
            accum.Append ("]");
            return accum.ToString ();
        }
    }
}

