using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Compiler
{
    public class JackTokenizer
    {
        private readonly TextReader reader;
        private string? nextToken;
        public string FileName;
        public int currentRow = 1, currentCol = 1;
        public int nextRow = 1, nextCol = 1;
        public int readRow = 1, readCol = 1;
        private string? currentToken;
        public JackTokenizer(string fileName, TextReader reader)
        {
            FileName = fileName;
            this.reader = reader;
        }

        private char[] symbols = new char[] {
            '{', '}', '(', ')', '[', ']', '.', ',', ';', '+',
            '-', '*', '/', '&', '|', '<', '>', '=', '~'
        };

        private string[] keywords = new string[] {
            "class",
            "method",
            "function",
            "constructor",
            "int",
            "boolean",
            "char",
            "void",
            "var",
            "static",
            "field",
            "let",
            "do",
            "if",
            "else",
            "while",
            "return",
            "true",
            "false",
            "null",
            "this"
        };

        public bool HasMoreTokens()
        {
            do
            {
                nextToken = ReadToken();
            }
            while (nextToken?.Length == 0);

            return nextToken != null;
        }

        private string? ReadToken()
        {
            nextRow = readRow;
            nextCol = readCol;
            int raw = Read();
            if (raw == -1)
            {
                return null;
            }

            char firstChar = (char)raw;
            if (firstChar == '/' && reader.Peek() == (int)'/')
            {
                // Comment
                ReadUntil(firstChar, (l, c) => l == '\n');
                return string.Empty;
            }
            else if (firstChar == '/' && reader.Peek() == (int)'*')
            {
                // Comment
                ReadUntil(firstChar, (l, c) => l == '*' && c == '/');
                Read(); // Consume slash
                return string.Empty;
            }
            else if (char.IsWhiteSpace(firstChar))
            {
                // White Space
                ReadUntil(firstChar, (l, c) => !char.IsWhiteSpace(c));
                return string.Empty;
            }
            else if (symbols.Contains(firstChar))
            {
                return new string(new char[] { firstChar });
            }
            else if (firstChar == '"')
            {
                return ReadUntil(firstChar, (l, c) => l == '"');
            }
            else
            {
                return ReadUntil(firstChar, (l, c) => char.IsWhiteSpace(c) || symbols.Contains(c));
            }
        }

        private char? ReadSymbol()
        {
            int raw = reader.Peek();
            if (raw == -1)
            {
                return null;
            }

            char c = (char)raw;
            if (symbols.Contains(c))
            {
                return c;
            }

            return null;
        }

        private int Read()
        {
            int raw = reader.Read();

            if (raw != -1)
            {
                char c = (char)raw;

                if (c == '\n')
                {
                    readRow++;
                    readCol = 1;
                }
                else
                {
                    if (c != '\r')
                    {
                        readCol++;
                    }
                }
            }

            return raw;
        }

        private string? ReadUntil(char firstChar, Func<char?, char, bool> predicate)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(firstChar);
            char? last = null;
            while (true)
            {
                int raw = reader.Peek();
                if (raw == -1)
                {
                    break;
                }

                char c = (char)raw;
                if (predicate(last, c))
                {
                    break;
                }

                builder.Append(c);
                last = (char)Read();
            }

            return builder.ToString();
        }

        public void Advance()
        {
            currentToken = nextToken;
            nextToken = null;
            currentRow = nextRow;
            currentCol = nextCol;
        }

        public TokenType GetTokenType()
        {
            if (symbols.Contains(currentToken.FirstOrDefault()))
            {
                return TokenType.SYMBOL;
            }
            else if (currentToken.All(x => char.IsDigit(x)))
            {
                return TokenType.INT_CONST;
            }
            else if (keywords.Contains(currentToken))
            {
                return TokenType.KEYWORD;
            }
            else if (currentToken?.StartsWith('"') ?? false)
            {
                return TokenType.STRING_CONST;
            }

            return TokenType.IDENTIFIER;
        }

        public string GetKeyword()
        {
            return currentToken ?? string.Empty;
        }

        public char GetSymbol()
        {
            return currentToken.First();
        }

        public string? GetIdentifier()
        {
            return currentToken;
        }

        public int GetIntVal()
        {
            return int.Parse(currentToken ?? string.Empty);
        }

        public string GetStringVal()
        {
            return currentToken?.Trim('"') ?? string.Empty;
        }
    }
}
