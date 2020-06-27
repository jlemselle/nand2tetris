using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace Assembler.Core
{
    public static class AsmParsers
    {
        public static TextParser<Unit> Identifier { get; } =
            from first in Character.Letter.Or(Character.In('_', '.', '$', ':'))
            from rest in Character.LetterOrDigit.Or(Character.In('_', '.', '$', ':')).IgnoreMany()
            select Unit.Value;

        public static TextParser<Unit> WhiteSpace { get; } =
            Character.WhiteSpace.Where(x => x != '\n').IgnoreMany().Value(Unit.Value);

        public static TextParser<Unit> Comment { get; } =
            from commentStart in Span.EqualTo("//")
            from comment in Character.AnyChar.Where(x => x != '\n').IgnoreMany()
            select Unit.Value;
    }
}
