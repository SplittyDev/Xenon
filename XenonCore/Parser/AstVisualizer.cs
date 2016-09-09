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
            } else if (root is CodeBlock) {
                foreach (var child in ((CodeBlock) root).Children)
                    Visualize (child, depth);
            } else if (root is FunctionDeclaration) {
                Console.Write ($"* {root}");
                Visualize (((FunctionDeclaration) root).Body, depth + 1, true);
            } else if (root is NameExpression) {
                Console.Write ($"* {root}");
            } else if (root is BinaryExpression) {
                Console.Write ($"* Binary expression: {((BinaryExpression) root).Operation}");
                Visualize (((BinaryExpression) root).Left, depth + 1);
                Visualize (((BinaryExpression) root).Right, depth + 1);
            } else if (root is UnaryExpression) {
                Console.Write ($"* Unary expression: {((UnaryExpression) root).Operation}");
                Visualize (((UnaryExpression) root).Child, depth + 1);
            } else if (root is TernaryExpression) {
                Console.Write ($"* Ternary expression");
                Visualize (((TernaryExpression) root).Condition, depth + 1);
                Visualize (((TernaryExpression) root).Left, depth + 1);
                Visualize (((TernaryExpression) root).Right, depth + 1);
            } else if (root is CallExpression) {
                Console.Write ($"* Call");
                Visualize (((CallExpression) root).Arguments, depth + 1);
                Visualize (((CallExpression) root).Target, depth + 1);
            } else {
                Console.Write ($"* {root}");
            }
            if (depth == 0 && !suppressNewline) {
                Console.WriteLine ();
            }
        }
    }
}

