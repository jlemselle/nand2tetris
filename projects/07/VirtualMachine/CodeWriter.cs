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

            writer.WriteLine($"// {command.ToUpper()}");
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
            writer.WriteLine($"// {command} {segment.ToUpper()} {index}");
            writer.WriteLine();

            switch (command)
            {
                case CommandType.PUSH:
                    switch (segment)
                    {
                        case "constant":
                            writer.WriteLine($"@{index}");
                            writer.WriteLine("D=A");
                            break;

                        case "local":
                        case "argument":
                        case "this":
                        case "that":
                            writer.WriteLine($"@{GetOffsetForSegment(segment, index)}");
                            if (index != 0)
                            {
                                writer.WriteLine("D=M");
                                writer.WriteLine($"@{index}");
                                writer.WriteLine("A=D+A");
                            }
                            else
                            {
                                writer.WriteLine("A=M");
                            }
                            writer.WriteLine("D=M");
                            break;

                        case "pointer":
                        case "temp":
                        case "static":
                            writer.WriteLine($"@{GetOffsetForSegment(segment, index)}");
                            writer.WriteLine("D=M");
                            break;

                        default:
                            break;
                    }

                    writer.WriteLine("@SP");
                    writer.WriteLine("A=M");
                    writer.WriteLine("M=D");
                    writer.WriteLine("@SP");
                    writer.WriteLine("M=M+1");
                    break;

                case CommandType.POP:
                    switch (segment)
                    {
                        case "local":
                        case "argument":
                        case "this":
                        case "that":
                            if (index != 0)
                            {
                                writer.WriteLine($"@{GetOffsetForSegment(segment, index)}");
                                writer.WriteLine("D=M");
                                writer.WriteLine($"@{index}");
                                writer.WriteLine("D=D+A");
                                writer.WriteLine("@R13");
                                writer.WriteLine("M=D");
                                writer.WriteLine("@SP");
                                writer.WriteLine("AM=M-1");
                                writer.WriteLine("D=M");
                                writer.WriteLine("@R13");
                                writer.WriteLine("A=M");
                                writer.WriteLine("M=D");
                            }
                            else
                            {
                                writer.WriteLine("@SP");
                                writer.WriteLine("AM=M-1");
                                writer.WriteLine("D=M");
                                writer.WriteLine($"@{GetOffsetForSegment(segment, index)}");
                                writer.WriteLine("A=M");
                                writer.WriteLine("M=D");
                            }

                            break;

                        case "pointer":
                        case "temp":
                        case "static":
                            writer.WriteLine("@SP");
                            writer.WriteLine("AM=M-1");
                            writer.WriteLine("D=M");
                            writer.WriteLine($"@{GetOffsetForSegment(segment, index)}");
                            writer.WriteLine("M=D");
                            break;

                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }

            writer.WriteLine();
            writer.WriteLine();
        }

        private string GetOffsetForSegment(string segment, int index)
        {
            switch (segment)
            {
                case "local":
                    return "LCL";
                case "argument":
                    return "ARG";
                case "this":
                    return "THIS";
                case "that":
                    return "THAT";
                case "pointer":
                    return (index + 3).ToString();
                case "temp":
                    return (index + 5).ToString();
                case "static":
                    return $"{fileName}.{index}";
                default:
                    return string.Empty;
            }
        }

        public void Close()
        {
            writer.Close();
        }
    }
}
