using System;
namespace XenonCore {

    /// <summary>
    /// Parser.
    /// </summary>
    public class Parser {

        /// <summary>
        /// The parsing unit.
        /// </summary>
        readonly ParsingUnit unit;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.Parser"/> class.
        /// </summary>
        /// <param name="unit">Unit.</param>
        public Parser (ParsingUnit unit) {
            this.unit = unit;
        }

        public AstRoot Visit () {

            // Create the root node
            var root = new AstRoot (unit.Location);

            // Return the root node
            return root;
        }
    }
}

