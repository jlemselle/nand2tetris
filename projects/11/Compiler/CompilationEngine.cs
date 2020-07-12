using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Compiler
{
    public class CompilationEngine
    {
        private readonly VMWriter writer;
        private int labelUniqueIdentifier = 0;
        private readonly JackTokenizer tokenizer;
        private readonly SymbolTable symbolTable;
        private readonly Parse Parse;
        private readonly Is Is;
        public CompilationEngine(JackTokenizer tokenizer, TextWriter writer)
        {
            this.writer = new VMWriter(writer);
            this.tokenizer = tokenizer;
            Parse = new Parse(tokenizer);
            Is = new Is(tokenizer);
            symbolTable = new SymbolTable();
        }

        private string? className;
        public void CompileClass()
        {
            Parse.Keyword("class");
            Parse.Identifier("class name", out className);
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

            if (constructorFunctionOrMethod == "method")
            {
                symbolTable.Define("this", className ?? string.Empty, Kind.ARG);
            }

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

            writer.WriteFunction($"{className}.{subroutineName}", symbolTable.VarCount(Kind.VAR));

            switch (constructorFunctionOrMethod)
            {
                case "constructor":
                    writer.WritePush(Segment.CONSTANT, symbolTable.VarCount(Kind.FIELD));
                    writer.WriteCall("Memory.alloc", 1);
                    writer.WritePop(Segment.POINTER, 0);
                    break;

                case "method":
                    writer.WritePush(Segment.ARGUMENT, 0);
                    writer.WritePop(Segment.POINTER, 0);
                    break;
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
            writer.WritePop(Segment.TEMP, 0);
        }

        public void CompileLet()
        {
            Parse.Keyword("let");
            Parse.Identifier("var name", out string varName);
            if (Is.Symbol('['))
            {
                switch (symbolTable.KindOf(varName))
                {
                    case Kind.VAR:
                        writer.WritePush(Segment.LOCAL, symbolTable.IndexOf(varName));
                        break;
                    case Kind.ARG:
                        writer.WritePush(Segment.ARGUMENT, symbolTable.IndexOf(varName));
                        break;
                    case Kind.FIELD:
                        writer.WritePush(Segment.THIS, symbolTable.IndexOf(varName));
                        break;
                    case Kind.STATIC:
                        writer.WritePush(Segment.STATIC, symbolTable.IndexOf(varName));
                        break;
                }

                Parse.Symbol('[');
                CompileExpression();
                Parse.Symbol(']');

                writer.WriteArithmetic(Command.ADD);

                Parse.Symbol('=');
                CompileExpression();
                Parse.Symbol(';');

                writer.WritePop(Segment.TEMP, 0);
                writer.WritePop(Segment.POINTER, 1);
                writer.WritePush(Segment.TEMP, 0);
                writer.WritePop(Segment.THAT, 0);
            }
            else
            {
                Parse.Symbol('=');
                CompileExpression();
                Parse.Symbol(';');

                switch (symbolTable.KindOf(varName))
                {
                    case Kind.VAR:
                        writer.WritePop(Segment.LOCAL, symbolTable.IndexOf(varName));
                        break;
                    case Kind.ARG:
                        writer.WritePop(Segment.ARGUMENT, symbolTable.IndexOf(varName));
                        break;
                    case Kind.FIELD:
                        writer.WritePop(Segment.THIS, symbolTable.IndexOf(varName));
                        break;
                    case Kind.STATIC:
                        writer.WritePop(Segment.STATIC, symbolTable.IndexOf(varName));
                        break;
                }
            }
        }

        public void CompileWhile()
        {
            string whileLabel = $"WHILE{labelUniqueIdentifier}";
            string endWhileLabel = $"WHILE_END{labelUniqueIdentifier++}";

            Parse.Keyword("while");
            Parse.Symbol('(');
            writer.WriteLabel(whileLabel);
            CompileExpression();
            writer.WriteArithmetic(Command.NOT);
            writer.WriteIf(endWhileLabel);
            Parse.Symbol(')');
            Parse.Symbol('{');
            CompileStatements();
            writer.WriteGoto(whileLabel);
            Parse.Symbol('}');
            writer.WriteLabel(endWhileLabel);
        }

        public void CompileReturn()
        {
            Parse.Keyword("return");
            if (Is.ExprTerm())
            {
                CompileExpression();
            }
            else
            {
                writer.WritePush(Segment.CONSTANT, 0);
            }
            Parse.Symbol(';');

            writer.WriteReturn();
        }

        public void CompileIf()
        {
            string endIfLabel = $"IF_END{labelUniqueIdentifier}";
            string elseLabel = $"ELSE{labelUniqueIdentifier++}";
            Parse.Keyword("if");
            Parse.Symbol('(');
            CompileExpression();
            Parse.Symbol(')');
            writer.WriteArithmetic(Command.NOT);
            writer.WriteIf(elseLabel);
            Parse.Symbol('{');
            CompileStatements();
            Parse.Symbol('}');
            writer.WriteGoto(endIfLabel);

            writer.WriteLabel(elseLabel);
            if (Is.Else())
            {
                Parse.Keyword("else");
                Parse.Symbol('{');
                CompileStatements();
                Parse.Symbol('}');
            }
            writer.WriteLabel(endIfLabel);
        }

        public void CompileExpression()
        {
            if (Is.ExprTerm())
            {
                CompileTerm();
            }

            while (Is.ExprOp())
            {
                Parse.ExprOp(out char symbol);
                CompileTerm();

                switch (symbol)
                {
                    case '+':
                        writer.WriteArithmetic(Command.ADD);
                        break;
                    case '-':
                        writer.WriteArithmetic(Command.SUB);
                        break;
                    case '*':
                        writer.WriteCall("Math.multiply", 2);
                        break;
                    case '/':
                        writer.WriteCall("Math.divide", 2);
                        break;
                    case '&':
                        writer.WriteArithmetic(Command.AND);
                        break;
                    case '|':
                        writer.WriteArithmetic(Command.OR);
                        break;
                    case '<':
                        writer.WriteArithmetic(Command.LT);
                        break;
                    case '>':
                        writer.WriteArithmetic(Command.GT);
                        break;
                    case '=':
                        writer.WriteArithmetic(Command.EQ);
                        break;
                    default:
                        break;
                }
            }
        }

        public void CompileTerm()
        {
            if (Is.Integer(out int _))
            {
                Parse.Integer(out int value);
                writer.WritePush(Segment.CONSTANT, value);
            }
            else if (Is.String())
            {
                Parse.String(out string value);
                writer.WritePush(Segment.CONSTANT, value.Length);
                writer.WriteCall("String.new", 1);
                writer.WritePop(Segment.TEMP, 1);

                foreach (char c in value)
                {
                    writer.WritePush(Segment.TEMP, 1);
                    writer.WritePush(Segment.CONSTANT, c);
                    writer.WriteCall("String.appendChar", 2);
                    writer.WritePop(Segment.TEMP, 0);
                }

                writer.WritePush(Segment.TEMP, 1);
            }
            else if (Is.ExprKeyword())
            {
                Parse.ExprKeyword(out string keyword);
                switch (keyword)
                {
                    case "true":
                        writer.WritePush(Segment.CONSTANT, 0);
                        writer.WriteArithmetic(Command.NOT);
                        break;
                    case "false":
                    case "null":
                        writer.WritePush(Segment.CONSTANT, 0);
                        break;
                    case "this":
                        writer.WritePush(Segment.POINTER, 0);
                        break;
                    default:
                        break;
                }
            }
            else if (Is.ExprUnaryOp())
            {
                Parse.ExprUnaryOp(out char unaryOp);
                CompileTerm();

                if (unaryOp == '-')
                {
                    writer.WriteArithmetic(Command.NEG);
                }
                else if (unaryOp == '~')
                {
                    writer.WriteArithmetic(Command.NOT);
                }
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
                Parse.Identifier("var name", out string varName);

                switch (symbolTable.KindOf(varName))
                {
                    case Kind.VAR:
                        writer.WritePush(Segment.LOCAL, symbolTable.IndexOf(varName));
                        break;
                    case Kind.ARG:
                        writer.WritePush(Segment.ARGUMENT, symbolTable.IndexOf(varName));
                        break;
                    case Kind.FIELD:
                        writer.WritePush(Segment.THIS, symbolTable.IndexOf(varName));
                        break;
                    case Kind.STATIC:
                        writer.WritePush(Segment.STATIC, symbolTable.IndexOf(varName));
                        break;
                }

                if (Is.Symbol('['))
                {
                    Parse.Symbol('[');
                    CompileExpression();
                    Parse.Symbol(']');

                    writer.WriteArithmetic(Command.ADD);
                    writer.WritePop(Segment.POINTER, 1);
                    writer.WritePush(Segment.THAT, 0);
                }
            }
        }

        public void CompileExpressionList(out int count)
        {
            count = 0;
            if (Is.ExprTerm())
            {
                count++;
                CompileExpression();
            }

            while (Is.Symbol(','))
            {
                count++;
                Parse.Symbol(',');
                CompileExpression();
            }
        }

        public void CompileSubroutineCall()
        {
            Parse.Identifier("class, var or subroutine name", out string firstIdentifier);
            string? varOrClassName = null;
            string subroutineName;
            string? className = null;
            int methodArgs = 0;
            if (Is.Symbol('.'))
            {
                Parse.Symbol('.');
                Parse.Identifier("subroutine name", out string secondIdentifier);
                varOrClassName = firstIdentifier;
                subroutineName = secondIdentifier;

                string varName = varOrClassName ?? string.Empty;
                switch (symbolTable.KindOf(varName))
                {
                    case Kind.ARG:
                        writer.WritePush(Segment.ARGUMENT, symbolTable.IndexOf(varName));
                        className = symbolTable.TypeOf(varName);
                        methodArgs = 1;
                        break;
                    case Kind.FIELD:
                        writer.WritePush(Segment.THIS, symbolTable.IndexOf(varName));
                        className = symbolTable.TypeOf(varName);
                        methodArgs = 1;
                        break;
                    case Kind.STATIC:
                        writer.WritePush(Segment.STATIC, symbolTable.IndexOf(varName));
                        className = symbolTable.TypeOf(varName);
                        methodArgs = 1;
                        break;
                    case Kind.VAR:
                        writer.WritePush(Segment.LOCAL, symbolTable.IndexOf(varName));
                        className = symbolTable.TypeOf(varName);
                        methodArgs = 1;
                        break;
                    case Kind.NONE:
                        className = varOrClassName;
                        break;
                }
            }
            else
            {
                subroutineName = firstIdentifier;
                className = this.className;
                writer.WritePush(Segment.POINTER, 0);
                methodArgs = 1;
            }
            Parse.Symbol('(');
            CompileExpressionList(out int args);
            Parse.Symbol(')');

            writer.WriteCall($"{className}.{subroutineName}", args + methodArgs);
        }
    }
}
