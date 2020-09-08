using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Syntax.Expressions
{
    public class ReferenceExpression : Expression
    {
        public string Value { get; }
        public List<string> Parts { get; }

        public override SyntaxKind Kind => SyntaxKind.ReferenceExpression;

        public ReferenceExpression(SourceSpan span, string identifier, List<string> parts) : base(span)
        {
            Value = identifier;
            this.Parts = parts;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
