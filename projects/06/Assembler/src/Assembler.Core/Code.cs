using System;
using System.Linq;

namespace Assembler.Core
{
    public static class Code
    {
        public static bool[] Dest(string dest)
        {
            return new bool[]
            {
                dest.Contains("A"),
                dest.Contains("D"),
                dest.Contains("M")
            };
        }

        public static bool[] Comp(string comp)
        {
            return comp switch
            {
                "0" => new bool[] { false, true, false, true, false, true, false },
                "1" => new bool[] { false, true, true, true, true, true, true },
                "-1" => new bool[] { false, true, true, true, false, true, false },
                "D" => new bool[] { false, false, false, true, true, false, false },
                "A" => new bool[] { false, true, true, false, false, false, false },
                "M" => new bool[] { true, true, true, false, false, false, false },
                "!D" => new bool[] { false, false, false, true, true, false, true },
                "!A" => new bool[] { false, true, true, false, false, false, true },
                "!M" => new bool[] { true, true, true, false, false, false, true },
                "-D" => new bool[] { false, false, false, true, true, true, true },
                "-A" => new bool[] { false, true, true, false, false, true, true },
                "-M" => new bool[] { true, true, true, false, false, true, true },
                "D+1" => new bool[] { false, false, true, true, true, true, true },
                "A+1" => new bool[] { false, true, true, false, true, true, true },
                "M+1" => new bool[] { true, true, true, false, true, true, true },
                "D-1" => new bool[] { false, false, false, true, true, true, false },
                "A-1" => new bool[] { false, true, true, false, false, true, false },
                "M-1" => new bool[] { true, true, true, false, false, true, false },
                "D+A" => new bool[] { false, false, false, false, false, true, false },
                "D+M" => new bool[] { true, false, false, false, false, true, false },
                "D-A" => new bool[] { false, false, true, false, false, true, true },
                "D-M" => new bool[] { true, false, true, false, false, true, true },
                "A-D" => new bool[] { false, false, false, false, true, true, true },
                "M-D" => new bool[] { true, false, false, false, true, true, true },
                "D&A" => new bool[] { false, false, false, false, false, false, false },
                "D&M" => new bool[] { true, false, false, false, false, false, false },
                "D|A" => new bool[] { false, false, true, false, true, false, true },
                "D|M" => new bool[] { true, false, true, false, true, false, true },
                _ => throw new ArgumentOutOfRangeException(nameof(comp))
            };
        }

        public static bool[] Jump(string dest)
        {
            return dest switch
            {
                "JGT" => new bool[] { false, false, true },
                "JEQ" => new bool[] { false, true, false },
                "JGE" => new bool[] { false, true, true },
                "JLT" => new bool[] { true, false, false },
                "JNE" => new bool[] { true, false, true },
                "JLE" => new bool[] { true, true, false },
                "JMP" => new bool[] { true, true, true },
                _ => new bool[] { false, false, false },
            };
        }

        public static bool[] Literal(int value)
        {
            bool[] result = Convert
                .ToString(value, 2)
                .PadLeft(16, '0')
                .Select(x => x == '1')
                .ToArray();

            result[0] = false;
            return result;
        }

        public static bool[] Literal(string literal)
        {
            int value = int.Parse(literal);

            return Literal(value);
        }
    }
}
