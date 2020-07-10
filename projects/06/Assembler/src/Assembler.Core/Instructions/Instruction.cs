namespace Assembler.Core.Instructions
{
    public class Instruction : IInstruction
    {
        public static IInstruction Empty { get; } = new Instruction();

        private Instruction()
        {

        }

        public string AsString()
        {
            return string.Empty;
        }
    }
}
