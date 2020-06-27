using Assembler.Core.Instructions;
using System.Collections.Generic;

namespace Assembler.Core
{

    public interface ILineParser
    {
        public IEnumerable<IInstruction> Parse(string line);
    }
}
