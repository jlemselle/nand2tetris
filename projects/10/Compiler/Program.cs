using System.IO;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = args[0];
            using TextReader reader = File.OpenText(file);
            using TextWriter writer = File.CreateText(Path.GetFileNameWithoutExtension(file) + ".vm");
            JackTokenizer tokenizer = new JackTokenizer(file, reader);

            CompilationEngine engine = new CompilationEngine(tokenizer, writer);

            engine.CompileClass();
        }
    }
}
