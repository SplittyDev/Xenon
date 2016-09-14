using System;
namespace IodineCore {
    
    /// <summary>
    /// Named parameter.
    /// </summary>
    public class NamedParameter {

        /// <summary>
        /// The name.
        /// </summary>
        public readonly NameExpression Name;

        /// <summary>
        /// The type.
        /// </summary>
        public NameExpression TypeHint;

        /// <summary>
        /// Gets whether the parameter has a type hint.
        /// </summary>
        /// <value>Whether the parameter has a type hint.</value>
        public bool HasTypeHint => TypeHint != null;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedParameter"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        public NamedParameter (NameExpression name, NameExpression typeHint = null) {
            Name = name;
            TypeHint = typeHint;
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString () => $"{Name}";
    }
}

