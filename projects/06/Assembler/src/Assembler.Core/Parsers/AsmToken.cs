using Superpower.Display;

namespace Assembler.Core.Parsers
{
    public enum AsmToken
    {
        [Token(Example = "(")]
        LBracket,

        [Token(Example = ")")]
        RBracket,

        [Token(Example = "@")]
        At,

        [Token(Example = "=")]
        Equal,

        [Token(Example = ";")]
        Semicolon,

        [Token(Example = "-")]
        Dash,

        [Token(Example = "!")]
        ExclamationMark,

        [Token(Example = "+")]
        Add,

        [Token(Example = "&")]
        Ampersand,

        [Token(Example = "|")]
        Pipe,

        D,

        A,

        M,

        Number,

        Identifier,

        NewLine,

        Comment
    }
}
