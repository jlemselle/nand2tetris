using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler
{
    public class Parse
    {
        private readonly JackTokenizer tokenizer;
        private readonly Is Is;
        public Parse(JackTokenizer tokenizer)
        {
            this.tokenizer = tokenizer;

            if (tokenizer.HasMoreTokens())
            {
                tokenizer.Advance();
            }
            else
            {
                throw new ParseException(tokenizer, "empty file");
            }

            Is = new Is(tokenizer);
        }

        public void VarNameList(out List<string> varNames)
        {
            varNames = new List<string>();
            Identifier("var name", out string varName);
            varNames.Add(varName);

            while (Is.Symbol(','))
            {
                Symbol(',');
                Identifier("var name", out string additionalVarName);
                varNames.Add(additionalVarName);
            }
        }

        public void Parameter(out (string type, string name) parameter)
        {
            Type("parameter", out string type);
            Identifier("parameter name", out string paramName);
            parameter = (type, paramName);
        }

        public void ParameterList(out List<(string type, string name)> parameters)
        {
            parameters = new List<(string type, string name)>();
            if (Is.Type(out string _))
            {
                Parameter(out (string type, string name) parameter);
                parameters.Add(parameter);

                while (Is.Symbol(','))
                {
                    Symbol(',');
                    Parameter(out (string type, string name) additionalParameters);
                    parameters.Add(additionalParameters);
                }
            }
        }

        public void Type(string name, out string type)
        {
            if (!Is.Type(out type))
            {
                throw new ParseException(tokenizer, $"Expected {name} type");
            }
            ConsumeToken();
        }

        public void Identifier(string name, out string identifier)
        {
            if (!Is.Identifier(out identifier))
            {
                throw new ParseException(tokenizer, $"Expected {name} identifier");
            }
            ConsumeToken();
        }

        public void Keyword(string keyword)
        {
            if (!Is.Keyword(keyword))
            {
                throw new ParseException(tokenizer, $"Expected `{keyword}` keyword");
            }
            ConsumeToken();
        }

        public void OneOfKeywords(out string keyword, params string[] keywords)
        {
            if (!keywords.Any(x => Is.Keyword(x)))
            {
                throw new ParseException(tokenizer, $"Expected one of {PrettyPrintKeywords(keywords)} keywords");
            }

            keyword = tokenizer.GetKeyword();

            ConsumeToken();
        }

        public void OneOfSymbols(out char symbol, params char[] symbols)
        {
            if (!symbols.Any(x => Is.Symbol(x)))
            {
                throw new ParseException(tokenizer, $"Expected one of {PrettyPrintKeywords(symbols.Select(x => x.ToString()))} symbols");
            }

            symbol = tokenizer.GetSymbol();

            ConsumeToken();
        }

        public void ReturnType(out string returnType)
        {
            if (!Is.ReturnType(out returnType))
            {
                throw new ParseException(tokenizer, $"Expected return type");
            }

            ConsumeToken();
        }

        public void Symbol(char symbol)
        {
            if (!Is.Symbol(symbol))
            {
                throw new ParseException(tokenizer, $"Expected `{symbol}` symbol");
            }

            ConsumeToken();
        }

        internal void ExprOp(out char symbol)
        {
            OneOfSymbols(out symbol, '+', '-', '*', '/', '&', '|', '<', '>', '=');
        }

        internal void Integer(out int value)
        {
            if (!Is.Integer(out value))
            {
                throw new ParseException(tokenizer, $"Expected integer");
            }

            ConsumeToken();
        }

        internal void String(out string value)
        {
            if (!Is.String())
            {
                throw new ParseException(tokenizer, $"Expected string");
            }
            value = tokenizer.GetStringVal();
            ConsumeToken();
        }

        internal void ExprKeyword(out string keyword)
        {
            OneOfKeywords(out keyword, "true", "false", "null", "this");
        }

        internal void ExprUnaryOp(out char unaryOp)
        {
            OneOfSymbols(out unaryOp, '-', '~');
        }

        public void EndOfFile()
        {
            if (!eof)
            {
                throw new ParseException(tokenizer, "Expected end of file");
            }
        }

        private bool eof = false;
        private void ConsumeToken()
        {
            if (eof)
            {
                throw new ParseException(tokenizer, "Unexpected end of file");
            }
            if (tokenizer.HasMoreTokens())
            {
                tokenizer.Advance();
            }
            else
            {
                eof = true;
            }
        }

        private string PrettyPrintKeywords(IEnumerable<string> keywords)
        {
            string last = keywords.Last();
            string[] commaSepWords = keywords.TakeWhile(x => x != last).ToArray();

            return string.Join(", ", "`" + commaSepWords + "`") + " or `" + last + "`";
        }
    }
}