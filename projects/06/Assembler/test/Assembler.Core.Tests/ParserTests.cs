using System.IO;
using Xunit;

namespace Assembler.Core.Tests
{
    public class ParserTests
    {
        [Fact]
        public void Parser_Empty_String_Has_No_Commands()
        {
            // Arrange
            TextReader reader = new StringReader("");
            Parser parser = new Parser(reader);

            // Assert
            Assert.False(parser.HasMoreCommands());
        }

        [Fact]
        public void Parser_A_Command()
        {
            // Arrange
            TextReader reader = new StringReader("@123");
            Parser parser = new Parser(reader);

            // Act
            parser.HasMoreCommands();
            parser.Advance();

            // Assert
            Assert.Equal(CommandType.A_COMMAND, parser.GetCommandType());
            Assert.Equal("123", parser.GetSymbol());
            Assert.False(parser.HasMoreCommands());
        }

        [Fact]
        public void Parser_Ignores_Comment()
        {
            // Arrange
            TextReader reader = new StringReader("// Test");
            Parser parser = new Parser(reader);

            // Assert
            Assert.False(parser.HasMoreCommands());
        }

        [Fact]
        public void Parser_C_Command()
        {
            // Arrange
            TextReader reader = new StringReader("D=A;JMP");
            Parser parser = new Parser(reader);

            // Act
            parser.HasMoreCommands();
            parser.Advance();

            // Assert
            Assert.Equal(CommandType.C_COMMAND, parser.GetCommandType());
            Assert.Equal("D", parser.GetDest());
            Assert.Equal("A", parser.GetComp());
            Assert.Equal("JMP", parser.GetJump());
            Assert.False(parser.HasMoreCommands());
        }

        [Fact]
        public void Parser_C_Command_Without_Dest()
        {
            // Arrange
            TextReader reader = new StringReader("A;JMP");
            Parser parser = new Parser(reader);

            // Act
            parser.HasMoreCommands();
            parser.Advance();

            // Assert
            Assert.Equal(CommandType.C_COMMAND, parser.GetCommandType());
            Assert.Equal("A", parser.GetComp());
            Assert.Equal("JMP", parser.GetJump());
            Assert.False(parser.HasMoreCommands());
        }

        [Fact]
        public void Parser_C_Command_Without_Jump()
        {
            // Arrange
            TextReader reader = new StringReader("D=A");
            Parser parser = new Parser(reader);

            // Act
            parser.HasMoreCommands();
            parser.Advance();

            // Assert
            Assert.Equal(CommandType.C_COMMAND, parser.GetCommandType());
            Assert.Equal("D", parser.GetDest());
            Assert.Equal("A", parser.GetComp());
            Assert.False(parser.HasMoreCommands());
        }

        [Fact]
        public void Parser_L_Command()
        {
            // Arrange
            TextReader reader = new StringReader("(TEST)");
            Parser parser = new Parser(reader);

            // Act
            parser.HasMoreCommands();
            parser.Advance();

            // Assert
            Assert.Equal(CommandType.L_COMMAND, parser.GetCommandType());
            Assert.Equal("TEST", parser.GetSymbol());
            Assert.False(parser.HasMoreCommands());
        }
    }
}
