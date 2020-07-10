using System;
using System.Collections.Generic;
using System.Linq;

namespace Assembler.Core.Model
{
    public enum Bit
    {
        Zero,
        One
    }

    public class BitSpan : List<Bit>
    {
        public static BitSpan FromInts(params int[] values)
        {
            BitSpan bits = new BitSpan();
            bits.AddRange(values.Select(FromInt));
            return bits;
        }

        public static BitSpan FromBools(params bool[] values)
        {
            BitSpan bits = new BitSpan();
            bits.AddRange(values.Select(FromBool));
            return bits;
        }

        private static Bit FromInt(int integerValue)
        {
            if (integerValue == 0)
            {
                return Bit.Zero;
            }

            if (integerValue == 1)
            {
                return Bit.One;
            }

            throw new ArgumentOutOfRangeException(nameof(integerValue));
        }

        private static Bit FromBool(bool boolValue)
        {
            return boolValue ? Bit.One : Bit.Zero;
        }
    }
}
