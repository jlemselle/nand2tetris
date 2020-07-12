using System.Linq;

namespace Compiler
{
    public class Is
    {
        private readonly JackTokenizer tokenizer;
        public Is(JackTokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
        }

        public bool Keyword(string value)
        {
            return tokenizer.GetTokenType() == TokenType.KEYWORD
                && tokenizer.GetKeyword() == value;
        }

        public bool Symbol(char symbol)
        {
            return tokenizer.GetTokenType() == TokenType.SYMBOL
                && tokenizer.GetSymbol() == symbol;
        }

        public bool Type(out string typeIdentifier)
        {
            if (tokenizer.GetTokenType() == TokenType.KEYWORD
                && new string[] { "int", "char", "boolean" }.Contains(tokenizer.GetKeyword()))
            {
                typeIdentifier = tokenizer.GetKeyword() ?? string.Empty;
                return true;
            }
            if (tokenizer.GetTokenType() == TokenType.IDENTIFIER)
            {
                typeIdentifier = tokenizer.GetIdentifier() ?? string.Empty;
                return true;
            }

            typeIdentifier = string.Empty;
            return false;
        }

        public bool Identifier(out string identifier)
        {
            identifier = tokenizer.GetIdentifier() ?? string.Empty;
            return tokenizer.GetTokenType() == TokenType.IDENTIFIER;
        }


        public bool ReturnType(out string returnType)
        {
            if (Type(out returnType))
            {
                return true;
            }

            if (tokenizer.GetTokenType() == TokenType.KEYWORD
                && tokenizer.GetKeyword() == "void")
            {
                returnType = tokenizer.GetKeyword();
                return true;
            }

            return false;
        }

        public bool ClassVarDec()
        {
            return tokenizer.GetTokenType() == TokenType.KEYWORD
                && (tokenizer.GetKeyword() == "static" || tokenizer.GetKeyword() == "field");
        }

        public bool Subroutine()
        {
            return tokenizer.GetTokenType() == TokenType.KEYWORD
                && (tokenizer.GetKeyword() == "constructor" || tokenizer.GetKeyword() == "function"
                    || tokenizer.GetKeyword() == "method");
        }

        public bool VarDec()
        {
            return Keyword("var");
        }

        public bool LetStatement()
        {
            return Keyword("let");
        }

        public bool IfStatement()
        {
            return Keyword("if");
        }

        public bool Else()
        {
            return Keyword("else");
        }

        public bool WhileStatement()
        {
            return Keyword("while");
        }

        public bool DoStatement()
        {
            return Keyword("do");
        }

        public bool ReturnStatement()
        {
            return Keyword("return");
        }

        public bool Statement()
        {
            return LetStatement()
                || IfStatement()
                || WhileStatement()
                || DoStatement()
                || ReturnStatement();
        }

        public bool ExprKeyword()
        {
            return Keyword("true")
                || Keyword("false")
                || Keyword("null")
                || Keyword("this");
        }

        public bool ExprUnaryOp()
        {
            return Symbol('-')
                || Symbol('~');
        }

        public bool ExprOp()
        {
            return Symbol('+')
                || Symbol('-')
                || Symbol('*')
                || Symbol('/')
                || Symbol('&')
                || Symbol('|')
                || Symbol('<')
                || Symbol('>')
                || Symbol('=');
        }

        public bool ExprSubroutineCall()
        {
            return Identifier(out string _)
                && (tokenizer.nextToken == "(" || tokenizer.nextToken == ".");
        }

        public bool Integer(out int value)
        {
            if (tokenizer.GetTokenType() == TokenType.INT_CONST)
            {
                value = tokenizer.GetIntVal();
                return true;
            }

            value = 0;
            return false;
        }

        public bool String()
        {
            return tokenizer.GetTokenType() == TokenType.STRING_CONST;
        }

        public bool ExprTerm()
        {
            return Integer(out int _)
            || String()
            || ExprKeyword()
            || Identifier(out string _)
            || Symbol('(')
            || ExprUnaryOp();
        }
    }
}