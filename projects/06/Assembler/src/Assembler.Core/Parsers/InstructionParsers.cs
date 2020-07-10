using Assembler.Core.Instructions;
using Superpower;
using Superpower.Parsers;
using System.Collections.Generic;
using System.Linq;

namespace Assembler.Core.Parsers
{
    public static class InstructionParsers
    {
        public static TokenListParser<AsmToken, IInstruction> AInstruction { get; } =
            from _ in Token.EqualTo(AsmToken.At)
            from number in Token.EqualTo(AsmToken.Number)
            select (IInstruction)new AInstruction(short.Parse(number.ToStringValue()));

        public static TokenListParser<AsmToken, IInstruction> CInstruction { get; } =
            from dest in CInstructionParsers.Dest.OptionalOrDefault(System.Array.Empty<Destination>())
            from comp in CInstructionParsers.Comp
            from jump in CInstructionParsers.Jump.OptionalOrDefault(JumpCondition.Never)
            select (IInstruction)new CInstruction(dest, comp, jump);

        public static TokenListParser<AsmToken, IInstruction> AnInstruction { get; } =
            AInstruction.Or(CInstruction);

        public static TokenListParser<AsmToken, IInstruction> Line { get; } =
            from instruction in AnInstruction.OptionalOrDefault(Instruction.Empty)
            from comment in Token.EqualTo(AsmToken.Comment).Optional()
            from newLine in Token.EqualTo(AsmToken.NewLine)
            select instruction;

        public static TokenListParser<AsmToken, IEnumerable<IInstruction>> Document { get; } =
            Line.Many().Select(x => x.Where(x => x != Instruction.Empty));
    }
}
