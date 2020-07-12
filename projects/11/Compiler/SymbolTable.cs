using System.Collections.Generic;
using System.Linq;

namespace Compiler
{
    public class SymbolTable
    {
        Dictionary<string, Symbol> classSymbols;
        Dictionary<string, Symbol> subroutineSymbols;
        public SymbolTable()
        {
            classSymbols = new Dictionary<string, Symbol>();
            subroutineSymbols = new Dictionary<string, Symbol>();
        }

        public void StartSubroutine()
        {
            subroutineSymbols = new Dictionary<string, Symbol>();
        }

        public void Define(string name, string type, Kind kind)
        {
            if (kind == Kind.STATIC || kind == Kind.FIELD)
            {
                classSymbols.Add(name, new Symbol()
                {
                    Index = VarCount(kind),
                    Kind = kind,
                    Name = name,
                    Type = type
                });
            }
            else if (kind == Kind.ARG || kind == Kind.VAR)
            {
                subroutineSymbols.Add(name, new Symbol()
                {
                    Index = VarCount(kind),
                    Kind = kind,
                    Name = name,
                    Type = type
                });
            }
        }
        public int VarCount(Kind kind)
        {
            if (kind == Kind.STATIC || kind == Kind.FIELD)
            {
                return classSymbols.Count(x => x.Value.Kind == kind);
            }
            else if (kind == Kind.ARG || kind == Kind.VAR)
            {
                return subroutineSymbols.Count(x => x.Value.Kind == kind);
            }

            return 0;
        }

        public Kind KindOf(string name)
        {
            if (subroutineSymbols.ContainsKey(name))
            {
                return subroutineSymbols[name].Kind;
            }
            else if (classSymbols.ContainsKey(name))
            {
                return classSymbols[name].Kind;
            }

            return Kind.NONE;
        }

        public string TypeOf(string name)
        {
            if (subroutineSymbols.ContainsKey(name))
            {
                return subroutineSymbols[name].Type;
            }
            else if (classSymbols.ContainsKey(name))
            {
                return classSymbols[name].Type;
            }

            return string.Empty;
        }

        public int IndexOf(string name)
        {
            if (subroutineSymbols.ContainsKey(name))
            {
                return subroutineSymbols[name].Index;
            }
            else if (classSymbols.ContainsKey(name))
            {
                return classSymbols[name].Index;
            }

            return 0;
        }
    }
}