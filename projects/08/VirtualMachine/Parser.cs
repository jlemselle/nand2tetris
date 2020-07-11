using System;
using System.IO;
using System.Linq;

namespace VirtualMachine
{
    public class Parser
    {
        private readonly TextReader reader;
        private string? currentLine;
        private string? nextLine;

        private readonly string[] arithmeticCommands = new string[] {
            "add", "sub", "neg", "eq", "gt", "lt", "and", "or", "not"
        };

        public Parser(TextReader reader)
        {
            this.reader = reader;
        }

        public bool HasMoreCommands()
        {
            do
            {
                nextLine = ReadLine();
            }
            while (nextLine?.Length == 0);

            return nextLine != null;
        }

        public void Advance()
        {
            currentLine = nextLine;
            nextLine = null;
        }

        private string? ReadLine()
        {
            string? line = reader.ReadLine();

            if (line == null)
            {
                return null;
            }

            int indexOfComment = line.IndexOf("//");
            if (indexOfComment != -1)
            {
                line = line.Substring(0, indexOfComment);
            }

            return line.Trim();
        }

        public CommandType GetCommandType()
        {
            if (currentLine == null)
            {
                return CommandType.None;
            }

            if (arithmeticCommands.Contains(currentLine))
            {
                return CommandType.ARITHMETIC;
            }
            else if (currentLine.StartsWith("push"))
            {
                return CommandType.PUSH;
            }
            else if (currentLine.StartsWith("pop"))
            {
                return CommandType.POP;
            }
            else if (currentLine.StartsWith("label"))
            {
                return CommandType.LABEL;
            }
            else if (currentLine.StartsWith("goto"))
            {
                return CommandType.GOTO;
            }
            else if (currentLine.StartsWith("if-goto"))
            {
                return CommandType.IF;
            }
            else if (currentLine.StartsWith("function"))
            {
                return CommandType.FUNCTION;
            }
            else if (currentLine.StartsWith("return"))
            {
                return CommandType.RETURN;
            }
            else if (currentLine.StartsWith("call"))
            {
                return CommandType.CALL;
            }
            else
            {
                return CommandType.None;
            }
        }

        public string GetArg1()
        {
            if (currentLine == null)
            {
                return string.Empty;
            }

            if (arithmeticCommands.Contains(currentLine))
            {
                return currentLine;
            }

            var words = currentLine.Split(' ');

            if (words.Length > 1)
            {
                return words[1];
            }

            return string.Empty;
        }

        public int GetArg2()
        {
            if (currentLine == null)
            {
                return 0;
            }

            var words = currentLine.Split(' ');

            if (words.Length > 2 && int.TryParse(words[2], out int result))
            {
                return result;
            }

            return 0;
        }
    }
}
