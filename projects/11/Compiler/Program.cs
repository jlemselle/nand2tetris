using System.IO;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string directory = args[0];
            foreach (var file in Directory.GetFiles(directory, "*.jack"))
            {
                using TextReader reader = File.OpenText(file);
                using TextWriter writer = File.CreateText(Path.Combine(directory, Path.GetFileNameWithoutExtension(file) + ".vm"));
                JackTokenizer tokenizer = new JackTokenizer(file, reader);

                CompilationEngine engine = new CompilationEngine(tokenizer, writer);

                engine.CompileClass();
            }
        }
    }
}
