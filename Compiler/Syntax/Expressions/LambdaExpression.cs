using System.Collections.Generic;

namespace Compiler.Syntax.Expressions
{
    public class LambdaExpression : Expression
    {
        public string Name {get;}
        public List<Expression> Parameters {get;}
        public Expression Id {get;}
        public Expression Body {get; }

        public override SyntaxKind Kind => SyntaxKind.LambdaExpression;


        public LambdaExpression(SourceSpan hint, string name, Expression id, List<Expression> parameters, Expression body) : base(hint)
        {
            this.Name = name;
            this.Id = id;
            this.Parameters = parameters;
            this.Body = body;
        }
    }
}
