namespace Compiler
{
    public class Symbol
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public Kind Kind { get; set; }
        public int Index { get; set; }
    }
}