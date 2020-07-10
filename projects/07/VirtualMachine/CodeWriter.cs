using System;
using System.IO;
using System.Linq;

namespace VirtualMachine
{
    public class CodeWriter
    {
        private readonly TextWriter writer;

        public CodeWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        private string? fileName;
        public void SetFileName(string fileName)
        {
            this.fileName = fileName;
        }

        private int labelUniqueIdentifier = 0;
        public void WriteArithmetic(string command)
        {
            //  "add", "sub", "neg", "eq", "gt", "lt", "and", "or", "not"

            writer.WriteLine($"// {command}");
            writer.WriteLine();
            
            switch (command)
            {
                case "add":
                case "sub":
                case "and":
                case "or":
                    writer.WriteLine("@SP");
                    writer.WriteLine("M=M-1");
                    writer.WriteLine("A=M");
                    writer.WriteLine("D=M");
                    writer.WriteLine("A=A-1");
                    writer.WriteLine($"D=M{GetOperatorSymbol(command)}D");
                    writer.WriteLine("M=D");
                    break;

                case "neg":
                case "not":
                    writer.WriteLine("@SP");
                    writer.WriteLine("A=M-1");
                    writer.WriteLine("D=M");
                    writer.WriteLine($"M={GetOperatorSymbol(command)}D");
                    break;

                case "eq":
                    writer.WriteLine("@SP");
                    writer.WriteLine("M=M-1");
                    writer.WriteLine("A=M");
                    writer.WriteLine("D=M");
                    writer.WriteLine("A=A-1");
                    writer.WriteLine("D=M-D");
                    writer.WriteLine($"@IS_ZERO.{labelUniqueIdentifier}");
                    writer.WriteLine("D;JEQ");
                    writer.WriteLine("D=-1");
                    writer.WriteLine($"(IS_ZERO.{labelUniqueIdentifier++})");
                    writer.WriteLine("@SP");
                    writer.WriteLine("A=M-1");
                    writer.WriteLine("M=!D");
                    break;

                case "gt":
                case "lt":

                    writer.WriteLine("@SP");
                    writer.WriteLine("M=M-1");
                    writer.WriteLine("A=M");
                    writer.WriteLine("D=M");
                    writer.WriteLine("A=A-1");
                    writer.WriteLine("D=M-D");
                    writer.WriteLine($"@IS_TRUE.{labelUniqueIdentifier}");
                    writer.WriteLine($"D;{GetOperatorSymbol(command)}");
                    writer.WriteLine("D=0");
                    writer.WriteLine($"@IS_FALSE.{labelUniqueIdentifier}");
                    writer.WriteLine("0;JMP");
                    writer.WriteLine($"(IS_TRUE.{labelUniqueIdentifier})");
                    writer.WriteLine("D=-1");
                    writer.WriteLine($"(IS_FALSE.{labelUniqueIdentifier++})");
                    writer.WriteLine("@SP");
                    writer.WriteLine("A=M-1");
                    writer.WriteLine("M=D");
                    break;
            }

            writer.WriteLine();
            writer.WriteLine();
        }

        private string GetOperatorSymbol(string command)
        {
            switch (command)
            {
                case "add":
                    return "+";
                case "sub":
                    return "-";
                case "and":
                    return "&";
                case "or":
                    return "|";
                case "neg":
                    return "-";
                case "not":
                    return "!";
                case "lt":
                    return "JLT";
                case "gt":
                    return "JGT";
                default:
                    return string.Empty;
            }
        }

        public void WritePushPop(CommandType command, string segment, int index)
        {
            writer.WriteLine($"// {command}");
            writer.WriteLine();

            switch (command)
            {
                case CommandType.PUSH:
                    writer.WriteLine($"@{index}");
                    writer.WriteLine("D=A");
                    writer.WriteLine("@SP");
                    writer.WriteLine("A=M");
                    writer.WriteLine("M=D");
                    writer.WriteLine("@SP");
                    writer.WriteLine("M=M+1");
                    break;
                default:
                    break;
            }

            writer.WriteLine();
            writer.WriteLine();
        }

        public void Close()
        {
            writer.Close();
        }
    }
}
