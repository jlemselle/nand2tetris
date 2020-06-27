using Superpower.Display;
using Superpower.Parsers;

namespace Assembler.Core
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
        Minus,

        [Token(Example = "!")]
        ExclamationMark,

        [Token(Example = "+")]
        Plus,

        [Token(Example = "&")]
        And,

        [Token(Example = "|")]
        Or,

        D,

        A,

        M,

        JGT,

        JEQ,

        JGE,

        JLT,

        JNE,

        JLE,

        JMP,

        Number,

        Identifier,

        NewLine,

        Comment
    }
}
