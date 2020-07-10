using System;

namespace Assembler.Core.Instructions
{
    public class AInstruction
        : IInstruction
    {
        public short Value { get; }

        public AInstruction(short value)
        {
            if (value < 0b0000_0000_0000_0000 || value > 0b0111_1111_1111_1111)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            Value = value;
        }

        public string AsString()
        {
            return Convert.ToString(Value, 2).PadLeft(16, '0');
        }
    }
}
