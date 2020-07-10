using System;
using System.Collections.Generic;
using System.Linq;
using Superpower.Model;
using Xunit;

namespace Assembler.Core.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            
            int[] a = new int[] { 0b1010_1110,
0b1000_1000,
0b1110_0000,
0b1010_1110,
0b1110_1000,
0b1100_1000,
0b1110_1100,
0b1000_1000,
0b1010_0000,
0b1110_1010,
0b1010_1000,
0b1010_0000,
0b1010_1110,
0b1110_1110,
0b1110_0000,
0b1110_1110,
0b1000_1110,
0b1100_1000 };


            TokenList<AsmToken> tokens = AsmTokenizer.Instance.Tokenize(@"
// This file is part of www.nand2tetris.org
// and the book ""The Elements of Computing Systems""
// by Nisan and Schocken, MIT Press.
// File name: projects/06/add/Add.asm

// Computes R0 = 2 + 3  (R0 refers to RAM[0])

@2
D=A
@3
D=D+A
@0
M=D

");

            List<Token<AsmToken>> tokenList = tokens.ToList();
        }
    }
}
