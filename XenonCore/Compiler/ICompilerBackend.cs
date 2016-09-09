using System;
namespace XenonCore {

    /// <summary>
    /// Compiler backend.
    /// </summary>
    public interface ICompilerBackend {

        void Compile (AstNode unit);
    }
}

