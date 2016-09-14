using System;
using System.Collections.Generic;

namespace XenonCore {

    /// <summary>
    /// Emitter.
    /// </summary>
    public class Emitter : XenonBytecode {

        /// <summary>
        /// The instructions.
        /// </summary>
        public readonly List<Instruction> Instructions;

        /// <summary>
        /// The label references.
        /// </summary>
        public readonly Dictionary<int, Label> LabelReferences;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XenonCore.Emitter"/> class.
        /// </summary>
        public Emitter () {
            Instructions = new List<Instruction> ();
            LabelReferences = new Dictionary<int, Label> ();
        }

        public void Emit (Opcode opcode) {
            Instructions.Add (new Instruction (opcode));
        }

        public void Emit (Opcode opcode, int arg) {
            Instructions.Add (new Instruction (opcode, arg));
        }

        public void Emit (Opcode opcode, Label label) {
            LabelReferences[Instructions.Count] = label;
            Instructions.Add (new Instruction (opcode));
        }

        public void SetLabelPosition (Label label) {
            label.Position = Instructions.Count;
        }

        public void Resolve () {
            foreach (var labelPos in LabelReferences.Keys) {
                Instructions[labelPos] = new Instruction (
                    opcode: Instructions [labelPos].Opcode,
                    arg: LabelReferences [labelPos].Position
                );
            }
        }
    }
}

