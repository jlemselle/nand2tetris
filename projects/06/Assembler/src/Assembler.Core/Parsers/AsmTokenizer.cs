using Superpower;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace Assembler.Core.Parsers
{
    public static class AsmTokenizer
    {
        public static TextParser<char[]> Identifier { get; } =
            Character.LetterOrDigit
                .Or(Character.In('_', '.', '$', ':'))
                .AtLeastOnce();

        public static TextParser<char[]> Number { get; } =
            Character.Digit
                .AtLeastOnce();

        public static TextParser<Unit> WhiteSpace { get; } =
            Character.WhiteSpace
                .Where(x => x != '\n')
                .AtLeastOnce()
                .Value(Unit.Value);

        public static Tokenizer<AsmToken> Instance { get; } =
            new TokenizerBuilder<AsmToken>()
                .Ignore(WhiteSpace)
                .Match(Comment.CPlusPlusStyle, AsmToken.Comment)
                .Match(Character.EqualTo('('), AsmToken.LBracket)
                .Match(Character.EqualTo(')'), AsmToken.RBracket)
                .Match(Character.EqualTo('@'), AsmToken.At)
                .Match(Character.EqualTo('='), AsmToken.Equal)
                .Match(Character.EqualTo(';'), AsmToken.Semicolon)
                .Match(Character.EqualTo('-'), AsmToken.Dash)
                .Match(Character.EqualTo('!'), AsmToken.ExclamationMark)
                .Match(Character.EqualTo('+'), AsmToken.Add)
                .Match(Character.EqualTo('&'), AsmToken.Ampersand)
                .Match(Character.EqualTo('|'), AsmToken.Pipe)
                .Match(Character.EqualTo('D'), AsmToken.D)
                .Match(Character.EqualTo('A'), AsmToken.A)
                .Match(Character.EqualTo('M'), AsmToken.M)
                .Match(Character.EqualTo('\n'), AsmToken.NewLine)
                .Match(Number, AsmToken.Number, requireDelimiters: true)
                .Match(Identifier, AsmToken.Identifier, requireDelimiters: true)
                .Build();
    }
}
