using Assembler.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assembler.Runner
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            string file = args[0];

            var symbols = LoadSymbolTable(file);
            int nextRam = 16;

            using TextReader reader = File.OpenText(file);
            using TextWriter writer = File.CreateText(Path.GetFileNameWithoutExtension(file) + ".hack");

            Parser parser = new Parser(reader);

            while (parser.HasMoreCommands())
            {
                parser.Advance();

                switch (parser.GetCommandType())
                {
                    case CommandType.A_COMMAND:
                        string symbol = parser.GetSymbol();

                        if (symbol.All(char.IsDigit))
                        {
                            WriteBytes(writer, Code.Literal(symbol));
                        }
                        else
                        {
                            if (!symbols.ContainsKey(symbol))
                            {
                                symbols.Add(symbol, nextRam);
                                nextRam++;
                            }

                            WriteBytes(writer, Code.Literal(symbols[symbol]));
                        }
                        break;
                    case CommandType.C_COMMAND:
                        WriteBytes(writer, new bool[] { true, true, true }
                            .Concat(Code.Comp(parser.GetComp()))
                            .Concat(Code.Dest(parser.GetDest()))
                            .Concat(Code.Jump(parser.GetJump()))
                            .ToArray());
                        break;
                    default:
                        break;
                }
            }

            reader.Close();
            writer.Close();
        }

        private static void WriteBytes(TextWriter writer, bool[] bytes)
        {
            writer.WriteLine(bytes.Select(x => x ? '1' : '0').ToArray());
        }

        public static Dictionary<string, int> LoadSymbolTable(string file)
        {
            Dictionary<string, int> symbols = new Dictionary<string, int>() {
                { "SP", 0 },
                { "LCL", 1 },
                { "ARG", 2 },
                { "THIS", 3 },
                { "THAT", 4 },
                { "R0", 0 },
                { "R1", 1 },
                { "R2", 2 },
                { "R3", 3 },
                { "R4", 4 },
                { "R5", 5 },
                { "R6", 6 },
                { "R7", 7 },
                { "R8", 8 },
                { "R9", 9 },
                { "R10", 10 },
                { "R11", 11 },
                { "R12", 12 },
                { "R13", 13 },
                { "R14", 14 },
                { "R15", 15 },
                { "SCREEN", 16384 },
                { "KBD", 24576 }
            };
            using TextReader reader = File.OpenText(file);

            Parser parser = new Parser(reader);

            int instructionIndex = 0;
            while (parser.HasMoreCommands())
            {
                parser.Advance();

                switch (parser.GetCommandType())
                {
                    case CommandType.A_COMMAND:
                    case CommandType.C_COMMAND:
                        instructionIndex = instructionIndex + 1;
                        break;
                    case CommandType.L_COMMAND:
                        symbols.Add(parser.GetSymbol(), instructionIndex);
                        break;
                    default:
                        break;
                }
            }

            reader.Close();

            return symbols;
        }
    }
}
