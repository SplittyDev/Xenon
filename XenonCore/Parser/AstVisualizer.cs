using System;
namespace XenonCore {

    /// <summary>
    /// AST visualizer.
    /// </summary>
    public static class AstVisualizer {

        /// <summary>
        /// Gets the padding.
        /// </summary>
        /// <returns>The padding.</returns>
        /// <param name="depth">Depth.</param>
        static string GetPadding (int depth) => string.Empty.PadLeft (depth * 2);

        /// <summary>
        /// Visualizes the AST.
        /// </summary>
        /// <param name="root">Root.</param>
        /// <param name="depth">Depth.</param>
        /// <param name="suppressNewline">If set to <c>true</c> suppress newline.</param>
        public static void Visualize (AstNode root, int depth = 0, bool suppressNewline = false) {
            if (depth > 0 && !suppressNewline) {
                Console.WriteLine ();
            }
            Console.Write ($"{GetPadding (depth)}");
            if (root is AstRoot) {
                Console.Write ("* Root");
                foreach (var child in ((AstRoot) root).Children)
                    Visualize (child, depth + 1);
            } else {
                Console.Write ($"* {root}");
            }
            if (depth == 0 && !suppressNewline) {
                Console.WriteLine ();
            }
        }
    }
}

