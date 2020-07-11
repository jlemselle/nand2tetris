using System;
using System.IO;

namespace VirtualMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            string directory = args[0];

            using TextWriter writer = File.CreateText(Path.Combine(directory, Path.GetFileNameWithoutExtension(directory) + ".asm"));

            CodeWriter codeWriter = new CodeWriter(writer);

            codeWriter.WriteInit();

            foreach (var file in Directory.GetFiles(directory, "*.vm"))
            {
                using TextReader reader = File.OpenText(file);
                Parser parser = new Parser(reader);

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
                    else if (parser.GetCommandType() == CommandType.LABEL)
                    {
                        codeWriter.WriteLabel(parser.GetArg1());
                    }
                    else if (parser.GetCommandType() == CommandType.GOTO)
                    {
                        codeWriter.WriteGoto(parser.GetArg1());
                    }
                    else if (parser.GetCommandType() == CommandType.IF)
                    {
                        codeWriter.WriteIf(parser.GetArg1());
                    }
                    else if (parser.GetCommandType() == CommandType.FUNCTION)
                    {
                        codeWriter.WriteFunction(parser.GetArg1(), parser.GetArg2());
                    }
                    else if (parser.GetCommandType() == CommandType.RETURN)
                    {
                        codeWriter.WriteReturn();
                    }
                    else if (parser.GetCommandType() == CommandType.CALL)
                    {
                        codeWriter.WriteCall(parser.GetArg1(), parser.GetArg2());
                    }
                }
            }

            codeWriter.Close();
        }
    }
}
