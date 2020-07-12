using System.IO;

namespace Compiler
{
    public class VMWriter
    {
        private readonly TextWriter writer;
        public VMWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        public void WritePush(Segment segment, int index)
        {
            writer.WriteLine($"push {segment.ToString().ToLowerInvariant()} {index}");
        }

        public void WritePop(Segment segment, int index)
        {
            writer.WriteLine($"pop {segment.ToString().ToLowerInvariant()} {index}");
        }

        public void WriteArithmetic(Command command)
        {
            writer.WriteLine(command.ToString().ToLowerInvariant());
        }

        public void WriteLabel(string label)
        {
            writer.WriteLine($"label {label}");
        }

        public void WriteGoto(string label)
        {
            writer.WriteLine($"goto {label}");
        }

        public void WriteIf(string label)
        {
            writer.WriteLine($"if-goto {label}");
        }

        public void WriteCall(string function, int args)
        {
            writer.WriteLine($"call {function} {args}");
        }

        public void WriteFunction(string function, int locals)
        {
            writer.WriteLine($"function {function} {locals}");
        }

        public void WriteReturn()
        {
            writer.WriteLine($"return");
        }

        public void Close()
        {
            writer.Close();
        }
    }
}