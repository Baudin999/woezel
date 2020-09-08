using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Syntax.Expressions
{
    public abstract class Expression : SyntaxNode
    {
        public override SyntaxCatagory Catagory => SyntaxCatagory.Expression;

        public virtual ExpressionType Type { get; }

        protected Expression(SourceSpan span) : base(span)
        {
        }
    }
}
