using System;
namespace IodineCore {
    
    /// <summary>
    /// Source location.
    /// </summary>
    public class SourceLocation {

        /// <summary>
        /// A source location with all fields set to zero.
        /// </summary>
        public static SourceLocation Zero;

        /// <summary>
        /// Initializes the <see cref="T:LexDotNet.SourceLocation"/> class.
        /// </summary>
        static SourceLocation () {
            Zero = new SourceLocation (0, 0);
        }

        /// <summary>
        /// Gets the line.
        /// </summary>
        /// <value>The line.</value>
        public readonly int Line;

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        public readonly int Position;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:LexDotNet.SourceLocation"/> class.
        /// </summary>
        /// <param name="line">Line.</param>
        /// <param name="pos">Position.</param>
        internal SourceLocation (int line, int pos) {
            Line = line;
            Position = pos;
        }

        /// <summary>
        /// Returns the string representation of this <see cref="T:System.Object"/> .
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString () => $"{Line}:{Position}";
    }
}

