using Assembler.Core.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assembler.Core
{
    class LineParser : ILineParser
    {
        public IEnumerable<IInstruction> Parse(string line)
        {
            // todo
            return Enumerable.Empty<IInstruction>();
        }
    }
}
