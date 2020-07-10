using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Assembler.Core.Tests
{
    public class CodeTests
    {
        [Fact]
        public void Code_Literal()
        {
            bool[] bytes = Code.Literal("5");

            Assert.True(bytes[15]);
            Assert.False(bytes[14]);
            Assert.True(bytes[13]);
        }
    }
}
