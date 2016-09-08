using System;
using System.Runtime.CompilerServices;

namespace XenonCore {
    
    /// <summary>
    /// Source cursor.
    /// </summary>
    public class SourceCursor {

        /// <summary>
        /// The current cursor position.
        /// </summary>
        internal int pos;

        /// <summary>
        /// The highest possible cursor position.
        /// </summary>
        readonly int maxPos;

        /// <summary>
        /// Gets the current cursor position.
        /// </summary>
        /// <value>The position.</value>
        public int Position => pos;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:LexDotNet.SourceCursor"/> class.
        /// </summary>
        /// <param name="maxPos">The highest possible cursor position.</param>
        public SourceCursor (int maxPos) {
            this.maxPos = maxPos;
            pos = 0;
        }

        /// <summary>
        /// Advances the cursor by one unit.
        /// </summary>
        /// <returns>The next.</returns>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void MoveNext () => pos = Math.Min (pos + 1, maxPos);

        /// <summary>
        /// Advances the cursor by <paramref name="n"/> units.
        /// </summary>
        /// <param name="n">The number of units.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Move (int n = 1) => pos = Math.Min (pos + n, maxPos);
    }
}

