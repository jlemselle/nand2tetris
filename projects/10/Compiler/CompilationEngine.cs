using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Compiler
{
    public class CompilationEngine
    {
        private readonly TextWriter writer;
        private readonly JackTokenizer tokenizer;
        private readonly SymbolTable symbolTable;
        private readonly Parse Parse;
        private readonly Is Is;
        public CompilationEngine(JackTokenizer tokenizer, TextWriter writer)
        {
            this.writer = writer;
            this.tokenizer = tokenizer;
            Parse = new Parse(tokenizer);
            Is = new Is(tokenizer);
            symbolTable = new SymbolTable();
        }

        public void CompileClass()
        {
            Parse.Keyword("class");
            Parse.Identifier("class name", out string className);
            Parse.Symbol('{');

            while (Is.ClassVarDec())
            {
                CompileClassVarDec();
            }

            while (Is.Subroutine())
            {
                CompileSubroutine();
            }

            Parse.Symbol('}');
            Parse.EndOfFile();
        }

        public void CompileClassVarDec()
        {
            Parse.OneOfKeywords(out string staticOrField, "static", "field");
            Parse.Type("class var", out string type);
            Parse.VarNameList(out List<string> varNames);
            Parse.Symbol(';');

            Kind kind = staticOrField == "static" ? Kind.STATIC : Kind.FIELD;
            foreach (string name in varNames)
            {
                symbolTable.Define(name, type, kind);
            }
        }

        public void CompileSubroutine()
        {
            symbolTable.StartSubroutine();

            Parse.OneOfKeywords(out string constructorFunctionOrMethod, "constructor", "function", "method");
            Parse.ReturnType(out string returnType);
            Parse.Identifier("subroutine name", out string subroutineName);
            Parse.Symbol('(');
            CompileParameterList();
            Parse.Symbol(')');
            Parse.Symbol('{');

            while (Is.VarDec())
            {
                CompileVarDec();
            }

            CompileStatements();

            Parse.Symbol('}');
        }

        public void CompileParameterList()
        {
            Parse.ParameterList(out List<(string type, string name)> parameterList);

            foreach ((string type, string name) parameter in parameterList)
            {
                symbolTable.Define(parameter.name, parameter.type, Kind.ARG);
            }
        }

        public void CompileVarDec()
        {
            Parse.Keyword("var");
            Parse.Type("var", out string type);
            Parse.VarNameList(out List<string> varNames);
            Parse.Symbol(';');

            foreach (string name in varNames)
            {
                symbolTable.Define(name, type, Kind.VAR);
            }
        }

        public void CompileStatements()
        {
            while (Is.Statement())
            {
                if (Is.LetStatement())
                {
                    CompileLet();
                }
                else if (Is.IfStatement())
                {
                    CompileIf();
                }
                else if (Is.WhileStatement())
                {
                    CompileWhile();
                }
                else if (Is.DoStatement())
                {
                    CompileDo();
                }
                else if (Is.ReturnStatement())
                {
                    CompileReturn();
                }
            }
        }

        public void CompileDo()
        {
            Parse.Keyword("do");
            CompileSubroutineCall();
            Parse.Symbol(';');
        }

        public void CompileLet()
        {
            Parse.Keyword("let");
            Parse.Identifier("var name", out string varName);
            if (Is.Symbol('['))
            {
                Parse.Symbol('[');
                CompileExpression();
                Parse.Symbol(']');
            }
            Parse.Symbol('=');
            CompileExpression();
            Parse.Symbol(';');
        }

        public void CompileWhile()
        {

            Parse.Keyword("while");
            Parse.Symbol('(');
            CompileExpression();
            Parse.Symbol(')');
            Parse.Symbol('{');
            CompileStatements();
            Parse.Symbol('}');
        }

        public void CompileReturn()
        {
            Parse.Keyword("return");
            if (Is.ExprTerm())
            {
                CompileExpression();
            }
            Parse.Symbol(';');
        }

        public void CompileIf()
        {
            Parse.Keyword("if");
            Parse.Symbol('(');
            CompileExpression();
            Parse.Symbol(')');
            Parse.Symbol('{');
            CompileStatements();
            Parse.Symbol('}');

            if (Is.Else())
            {
                Parse.Keyword("else");
                Parse.Symbol('{');
                CompileStatements();
                Parse.Symbol('}');
            }
        }

        public void CompileExpression()
        {
            if (Is.ExprTerm())
            {
                CompileTerm();
            }

            while (Is.ExprOp())
            {
                Parse.ExprOp();
                CompileTerm();
            }
        }

        public void CompileTerm()
        {
            if (Is.Integer())
            {
                Parse.Integer();
            }
            else if (Is.String())
            {
                Parse.String();
            }
            else if (Is.ExprKeyword())
            {
                Parse.ExprKeyword();
            }
            else if (Is.ExprUnaryOp())
            {
                Parse.ExprUnaryOp();
                CompileTerm();
            }
            else if (Is.Symbol('('))
            {
                Parse.Symbol('(');
                CompileExpression();
                Parse.Symbol(')');
            }
            else if (Is.ExprSubroutineCall())
            {
                CompileSubroutineCall();
            }
            else
            {
                Parse.Identifier("var name", out string _);
                if (Is.Symbol('['))
                {
                    Parse.Symbol('[');
                    CompileExpression();
                    Parse.Symbol(']');
                }
            }
        }

        public void CompileExpressionList()
        {
            if (Is.ExprTerm())
            {
                CompileExpression();
            }

            while (Is.Symbol(','))
            {
                Parse.Symbol(',');
                CompileExpression();
            }
        }

        public void CompileSubroutineCall()
        {
            Parse.Identifier("class, var or subroutine name", out string _);
            if (Is.Symbol('.'))
            {
                Parse.Symbol('.');
                Parse.Identifier("subroutine name", out string _);
            }
            Parse.Symbol('(');
            CompileExpressionList();
            Parse.Symbol(')');
        }
    }
}
