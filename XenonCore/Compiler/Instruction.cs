using System;
namespace XenonCore {

    /// <summary>
    /// Instruction.
    /// </summary>
    public class Instruction {

        /// <summary>
        /// The opcode.
        /// </summary>
        public readonly Opcode Opcode;

        /// <summary>
        /// The argument.
        /// </summary>
        public readonly int Argument;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.Instruction"/> class.
        /// </summary>
        /// <param name="opcode">Opcode.</param>
        public Instruction (Opcode opcode, int arg = 0) {
            Opcode = opcode;
            Argument = arg;
        }
    }
}

