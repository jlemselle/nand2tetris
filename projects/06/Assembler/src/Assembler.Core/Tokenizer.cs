using Assembler.Core.Instructions;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace Assembler.Core
{
    public static class AsmTokenizer
    {
        public static Tokenizer<AsmToken> Instance { get; } =
            new TokenizerBuilder<AsmToken>()
                .Ignore(AsmParsers.WhiteSpace)
                .Match(AsmParsers.Comment, AsmToken.Comment)
                .Match(Character.EqualTo('('), AsmToken.LBracket)
                .Match(Character.EqualTo(')'), AsmToken.RBracket)
                .Match(Character.EqualTo('@'), AsmToken.At)
                .Match(Character.EqualTo('='), AsmToken.Equal)
                .Match(Character.EqualTo(';'), AsmToken.Semicolon)
                .Match(Character.EqualTo('-'), AsmToken.Minus)
                .Match(Character.EqualTo('!'), AsmToken.ExclamationMark)
                .Match(Character.EqualTo('+'), AsmToken.Plus)
                .Match(Character.EqualTo('&'), AsmToken.And)
                .Match(Character.EqualTo('|'), AsmToken.Or)
                .Match(Character.EqualTo('D'), AsmToken.D)
                .Match(Character.EqualTo('A'), AsmToken.A)
                .Match(Character.EqualTo('M'), AsmToken.M)
                .Match(Span.EqualTo("JGT"), AsmToken.JGT, requireDelimiters: true)
                .Match(Span.EqualTo("JEQ"), AsmToken.JEQ, requireDelimiters: true)
                .Match(Span.EqualTo("JGE"), AsmToken.JGE, requireDelimiters: true)
                .Match(Span.EqualTo("JLT"), AsmToken.JLT, requireDelimiters: true)
                .Match(Span.EqualTo("JNE"), AsmToken.JNE, requireDelimiters: true)
                .Match(Span.EqualTo("JLE"), AsmToken.JLE, requireDelimiters: true)
                .Match(Span.EqualTo("JMP"), AsmToken.JMP, requireDelimiters: true)
                .Match(Character.EqualTo('\n'), AsmToken.NewLine)
                .Match(Character.Digit.IgnoreMany(), AsmToken.Number, requireDelimiters: true)
                .Match(Character.LetterOrDigit.Or(Character.In('_', '.', '$', ':')).IgnoreMany(), AsmToken.Identifier, requireDelimiters: true)
                .Build();
    }

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

    static class AsmParser
    {
        public static TokenListParser<AsmToken, IInstruction> AInstructionParser { get; } =
            from _ in Token.EqualTo(AsmToken.At)
            from number in Token.EqualTo(AsmToken.Number)
            select (IInstruction)new AInstruction(short.Parse(number.ToStringValue()));

        // `TryParse` is just a helper method. It's useful to write one of these, where
        // the tokenization and parsing phases remain distinct, because it's often very
        // handy to place a breakpoint between the two steps to check out what the
        // token list looks like.
        public static bool TryParse(string json, out object? value, out string? error, out Position errorPosition)
        {
            var tokens = AsmTokenizer.Instance.TryTokenize(json);
            if (!tokens.HasValue)
            {
                value = null;
                error = tokens.ToString();
                errorPosition = tokens.ErrorPosition;
                return false;
            }

            var parsed = JsonDocument.TryParse(tokens.Value);
            if (!parsed.HasValue)
            {
                value = null;
                error = parsed.ToString();
                errorPosition = parsed.ErrorPosition;
                return false;
            }

            value = parsed.Value;
            error = null;
            errorPosition = Position.Empty;
            return true;
        }
    }

    public class Tokenizer
    {
    }
}
