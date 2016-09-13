using System;
namespace XenonCore {

    /// <summary>
    /// Label.
    /// </summary>
    public class Label {

        static int nextId;

        /// <summary>
        /// The identifier.
        /// </summary>
        public readonly int Id;

        /// <summary>
        /// The position.
        /// </summary>
        public int Position;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.Label"/> class.
        /// </summary>
        public Label () {
            Id = nextId++;
        }
    }
}

