using System.Collections.Generic;

namespace Compiler.Syntax.Statements
{
    public class AnnotationStatement
    {
        public string Value => string.Join(" ", Parts);
        private List<string> Parts { get; } = new List<string>();

        public AnnotationStatement() {  }
        public AnnotationStatement(string value)
        {
            Parts.Add(Clean(value));
        }

        public void Add(string value)
        {
            Parts.Add(Clean(value));
        }

        public void Add(Token token)
        {
            Parts.Add(Clean(token.Value));
        }

        private string Clean(string value)
        {
            return value.Replace("@", "").Trim();
        }

        public override string ToString()
        {
            return this.Value;
        }

    }
}
