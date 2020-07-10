using System;
using System.IO;

namespace Assembler.Core
{
    public class Parser
    {
        private readonly TextReader reader;
        private string? currentLine;
        private string? nextLine;
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

            if (currentLine.StartsWith("@"))
            {
                return CommandType.A_COMMAND;
            }
            else if (currentLine.StartsWith("("))
            {
                return CommandType.L_COMMAND;
            }
            else
            {
                return CommandType.C_COMMAND;
            }
        }

        public string GetSymbol()
        {
            if (currentLine == null)
            {
                return string.Empty;
            }

            return currentLine.TrimStart('@', '(').TrimEnd(')');
        }

        public string GetDest()
        {
            if (currentLine == null)
            {
                return string.Empty;
            }

            int indexOfEquals = currentLine.IndexOf("=");
            if (indexOfEquals == -1)
            {
                return string.Empty;
            }

            return currentLine.Substring(0, indexOfEquals);
        }

        public string GetComp()
        {
            if (currentLine == null)
            {
                return string.Empty;
            }

            int startIndex = currentLine.IndexOf("=") + 1;
            int endIndex = currentLine.IndexOf(";");

            if (endIndex == -1)
            {
                return currentLine.Substring(startIndex);
            }
            else
            {
                return currentLine.Substring(startIndex, endIndex - startIndex);
            }
        }

        public string GetJump()
        {
            if (currentLine == null)
            {
                return string.Empty;
            }

            int indexOfSemicolon = currentLine.IndexOf(";");
            if (indexOfSemicolon == -1)
            {
                return string.Empty;
            }

            return currentLine.Substring(indexOfSemicolon + 1);
        }
    }
}
