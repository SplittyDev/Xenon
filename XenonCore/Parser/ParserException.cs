using System;
using System.Text;

namespace XenonCore {
    
    /// <summary>
    /// Parser exception.
    /// </summary>
    public class ParserException : Exception {

        /// <summary>
        /// The message.
        /// </summary>
        public new string Message;

        string Header;
        string Description;
        string Solution;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserException"/> class.
        /// </summary>
        /// <param name="loc">Location.</param>
        public ParserException (SourceLocation loc) {
            Header = $"At Line {loc.Line} Pos {loc.Position}:";
            Description = string.Empty;
            Solution = string.Empty;
            BuildMessage ();
        }

        /// <summary>
        /// Creates a new Lore exception.
        /// </summary>
        /// <param name="loc">Location.</param>
        public static ParserException Create (SourceLocation loc = null)
        => new ParserException (loc ?? SourceLocation.Zero);

        public ParserException Describe (string line) {
            Description = $"{Description}\n| D | {line}";
            BuildMessage ();
            return this;
        }

        public ParserException Resolve (string line) {
            Solution = $"{Solution}\n| S | {line}";
            BuildMessage ();
            return this;
        }

        public void BuildMessage () {
            var accum = new StringBuilder ();
            accum.Append (Header);
            if (!string.IsNullOrEmpty (Description)) {
                accum.Append (Description);
            }
            if (!string.IsNullOrEmpty (Solution)) {
                accum.Append (Solution);
            }
            Message = accum.ToString ();
        }
    }
}

