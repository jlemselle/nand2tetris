using System;
using System.IO;

namespace VirtualMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = args[0];

            using TextReader reader = File.OpenText(file);
            using TextWriter writer = File.CreateText(Path.GetFileNameWithoutExtension(file) + ".asm");
            
            Parser parser = new Parser(reader);
            CodeWriter codeWriter = new CodeWriter(writer);

            codeWriter.SetFileName(Path.GetFileNameWithoutExtension(file));

            while (parser.HasMoreCommands())
            {
                parser.Advance();
                
                if (parser.GetCommandType() == CommandType.ARITHMETIC)
                {
                    codeWriter.WriteArithmetic(parser.GetArg1());
                }
                else if (parser.GetCommandType() == CommandType.PUSH
                    || parser.GetCommandType() == CommandType.POP)
                {
                    codeWriter.WritePushPop(parser.GetCommandType(), parser.GetArg1(), parser.GetArg2());
                }
            }
        }
    }
}
