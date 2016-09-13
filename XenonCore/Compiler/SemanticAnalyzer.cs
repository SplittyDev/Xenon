using System;
namespace XenonCore {

    /// <summary>
    /// Semantic analyzer.
    /// </summary>
    public class SemanticAnalyzer : AstVisitor {

        /// <summary>
        /// The symbol table.
        /// </summary>
        readonly SymbolTable symbolTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.XenonSemanticAnalyzer"/> class.
        /// </summary>
        public SemanticAnalyzer () {
            symbolTable = new SymbolTable ();
        }

        /// <summary>
        /// Analyzes the specified unit.
        /// </summary>
        /// <param name="unit">Unit.</param>
        public SymbolTable Analyze (AstNode unit) {
            unit.VisitChildren (this);
            return symbolTable;
        }

        public override void Accept (AstRoot node) {
            node.VisitChildren (this);
        }

        public override void Accept (NameExpression node) {
            node.VisitChildren (this);
        }

        public override void Accept (FunctionDeclaration node) {

            // Add the function name to the scope
            symbolTable.AddSymbol (node.Name.Value);

            // Begin a new scope
            symbolTable.InvokeInNewScope (() => {

                // Iterate over the function arguments
                foreach (var arg in node.Arguments) {

                    // Add the argument to the scope
                    symbolTable.AddSymbol (arg.Name.Value);
                }

                // Visit the children of the function declaration
                node.VisitChildren (this);
            });
        }

        public override void Accept (CodeBlock node) {

            // Begin a new scope
            symbolTable.InvokeInNewScope (() => {

                // Visit the children of the code block
                node.VisitChildren (this);
            });
        }

        public override void Accept (BinaryExpression node) {

            // Test if the node is an assignment and its left expression is a name expression
            if (true
                && node.Operation == BinaryOperation.Assign
                && node.Left is NameExpression) {

                // Get the name expression
                var name = (NameExpression) node.Left;

                // Verify that the name of the left expression is not yet in the scope
                if (!symbolTable.FindSymbol (name.Value)) {

                    // Add the name of the left expression to the scope
                    symbolTable.AddSymbol (name.Value);
                }
            }
        }

        public override void Accept (TernaryExpression node) {
            node.VisitChildren (this);
        }

        public override void Accept (UnaryExpression node) {
            node.VisitChildren (this);
        }

        public override void Accept (ArgumentList node) {
            node.VisitChildren (this);
        }

        public override void Accept (CallExpression node) {
            node.VisitChildren (this);
        }

        public override void Accept (TupleExpression node) {
            node.VisitChildren (this);
        }

        public override void Accept (IntegerExpression node) {
            node.VisitChildren (this);
        }

        public override void Accept (FloatExpression node) {
            node.VisitChildren (this);
        }

        public override void Accept (StringExpression node) {
            node.VisitChildren (this);
        }

        public override void Accept (ListExpression node) {
            node.VisitChildren (this);
        }
    }
}

