using Assembler.Core.Instructions;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace Assembler.Core
{
    static class AsmParser
    {
        public static TokenListParser<AsmToken, IInstruction> AInstructionParser { get; } =
            from _ in Token.EqualTo(AsmToken.At)
            from number in Token.EqualTo(AsmToken.Number)
            select (IInstruction)new AInstruction(short.Parse(number.ToStringValue()));
    }
}
