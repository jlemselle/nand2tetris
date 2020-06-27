using Superpower;
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
}
