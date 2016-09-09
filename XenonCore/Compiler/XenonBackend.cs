using System;
namespace XenonCore {

    /// <summary>
    /// Xenon backend.
    /// </summary>
    public class XenonBackend : ICompilerBackend {
        
        public void Compile (AstNode unit) {
            var analyzer = new XenonSemanticAnalyzer ();
            var symbolTable = analyzer.Analyze (unit);
        }
    }
}

