using System;

namespace Compiler
{
    public class ParseException : Exception
    {
        public ParseException(JackTokenizer tokenizer, string message)
            : base($"{tokenizer.FileName}({tokenizer.currentRow},{tokenizer.currentCol}): error: {message}")
        {
        }
    }
}