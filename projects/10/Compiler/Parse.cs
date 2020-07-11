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
            parameter = (paramName, type);
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
                throw new ParseException(tokenizer, $"Expected either {PrettyPrintKeywords(keywords)} keywords");
            }

            keyword = tokenizer.GetKeyword();

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

        private string PrettyPrintKeywords(string[] keywords)
        {
            string last = keywords.Last();
            string[] commaSepWords = keywords.TakeWhile(x => x != last).ToArray();

            return string.Join(", ", "`" + commaSepWords + "`") + " and `" + last + "`";
        }
    }
}