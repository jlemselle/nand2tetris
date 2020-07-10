using Assembler.Core.Instructions;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace Assembler.Core.Parsers
{
    public static class CInstructionParsers
    {
        public static TokenListParser<AsmToken, Token<AsmToken>> Zero { get; } = Number(0);

        public static TokenListParser<AsmToken, Token<AsmToken>> One { get; } = Number(1);

        public static TokenListParser<AsmToken, Token<AsmToken>> MinusOne { get; } = Negative(One);
        public static TokenListParser<AsmToken, Token<AsmToken>> A { get; } = Token.EqualTo(AsmToken.A);
        public static TokenListParser<AsmToken, Token<AsmToken>> M { get; } = Token.EqualTo(AsmToken.M);
        public static TokenListParser<AsmToken, Token<AsmToken>> D { get; } = Token.EqualTo(AsmToken.D);

        public static TokenListParser<AsmToken, Token<AsmToken>> Negative(TokenListParser<AsmToken, Token<AsmToken>> f) =>
            Token.EqualTo(AsmToken.Dash)
                .Then(dash => f);

        public static TokenListParser<AsmToken, Token<AsmToken>> Minus(TokenListParser<AsmToken, Token<AsmToken>> lhs, TokenListParser<AsmToken, Token<AsmToken>> rhs) =>
            Operation(AsmToken.Dash, lhs, rhs);

        public static TokenListParser<AsmToken, Token<AsmToken>> Inverse(TokenListParser<AsmToken, Token<AsmToken>> f) =>
            Token.EqualTo(AsmToken.ExclamationMark)
                .Then(dash => f);

        public static TokenListParser<AsmToken, Token<AsmToken>> Operation(AsmToken op, TokenListParser<AsmToken, Token<AsmToken>> lhs, TokenListParser<AsmToken, Token<AsmToken>> rhs) =>
            lhs.Then(l => Token.EqualTo(op).Then(o => rhs));

        public static TokenListParser<AsmToken, Token<AsmToken>> Plus(TokenListParser<AsmToken, Token<AsmToken>> lhs, TokenListParser<AsmToken, Token<AsmToken>> rhs) =>
            Operation(AsmToken.Add, lhs, rhs);

        public static TokenListParser<AsmToken, Token<AsmToken>> And(TokenListParser<AsmToken, Token<AsmToken>> lhs, TokenListParser<AsmToken, Token<AsmToken>> rhs) =>
            Operation(AsmToken.Ampersand, lhs, rhs);

        public static TokenListParser<AsmToken, Token<AsmToken>> Or(TokenListParser<AsmToken, Token<AsmToken>> lhs, TokenListParser<AsmToken, Token<AsmToken>> rhs) =>
            Operation(AsmToken.Pipe, lhs, rhs);

        public static TokenListParser<AsmToken, Token<AsmToken>> Number(int value) =>
            Token.EqualToValue(AsmToken.Number, value.ToString());

        public static TokenListParser<AsmToken, Computation> Comp { get; } =
            Plus(D, One).Value(Computation.DPlusOne).Try()
            .Or(Plus(A, One).Value(Computation.APlusOne).Try())
            .Or(Plus(M, One).Value(Computation.MPlusOne).Try())
            .Or(Minus(D, One).Value(Computation.DMinusOne).Try())
            .Or(Minus(A, One).Value(Computation.AMinusOne).Try())
            .Or(Minus(M, One).Value(Computation.MMinusOne).Try())
            .Or(Plus(D, A).Value(Computation.DPlusA).Try())
            .Or(Plus(D, M).Value(Computation.DPlusM).Try())
            .Or(Minus(D, A).Value(Computation.DMinusA).Try())
            .Or(Minus(D, M).Value(Computation.DMinusM).Try())
            .Or(Minus(A, D).Value(Computation.AMinusD).Try())
            .Or(Minus(M, D).Value(Computation.MMinusD).Try())
            .Or(And(D, A).Value(Computation.DAndA).Try())
            .Or(And(D, M).Value(Computation.DAndM).Try())
            .Or(Or(D, A).Value(Computation.DOrA).Try())
            .Or(Or(D, M).Value(Computation.DOrM).Try())
            .Or(Zero.Value(Computation.Zero).Try())
            .Or(One.Value(Computation.One).Try())
            .Or(Negative(One).Value(Computation.NegativeOne).Try())
            .Or(A.Value(Computation.A).Try())
            .Or(D.Value(Computation.D).Try())
            .Or(M.Value(Computation.M).Try())
            .Or(Inverse(D).Value(Computation.InverseOfD).Try())
            .Or(Inverse(A).Value(Computation.InverseOfA).Try())
            .Or(Inverse(M).Value(Computation.InverseOfM).Try())
            .Or(Negative(D).Value(Computation.NegativeD).Try())
            .Or(Negative(A).Value(Computation.NegativeA).Try())
            .Or(Negative(M).Value(Computation.NegativeM).Try());

        public static TokenListParser<AsmToken, Destination[]> Dest { get; } =
            from dest in Token.EqualTo(AsmToken.A).Value(Destination.A)
                .Or(Token.EqualTo(AsmToken.M).Value(Destination.M))
                .Or(Token.EqualTo(AsmToken.D).Value(Destination.D))
                .Many()
            from _ in Token.EqualTo(AsmToken.Equal)
            select dest;

        public static TokenListParser<AsmToken, JumpCondition> Jump { get; } =
            from _ in Token.EqualTo(AsmToken.Semicolon)
            from jump in
                Token.EqualToValue(AsmToken.Identifier, "JEQ").Value(JumpCondition.IsZero)
                .Or(Token.EqualToValue(AsmToken.Identifier, "JGE").Value(JumpCondition.IsGreaterThanOrEqualToZero))
                .Or(Token.EqualToValue(AsmToken.Identifier, "JGT").Value(JumpCondition.IsGreaterThanZero))
                .Or(Token.EqualToValue(AsmToken.Identifier, "JLE").Value(JumpCondition.IsLessThanOrEqualToZero))
                .Or(Token.EqualToValue(AsmToken.Identifier, "JLT").Value(JumpCondition.IsLessThanZero))
                .Or(Token.EqualToValue(AsmToken.Identifier, "JMP").Value(JumpCondition.Always))
                .Or(Token.EqualToValue(AsmToken.Identifier, "JNE").Value(JumpCondition.IsNotZero))
            select jump;
    }
}
