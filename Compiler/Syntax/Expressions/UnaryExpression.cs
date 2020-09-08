using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Syntax.Expressions
{
    public class UnaryExpression : Expression
    {
        public Expression Argument { get; }

        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;

        public UnaryOperator Operator { get; }

        public UnaryExpression(SourceSpan span, Expression argument, UnaryOperator op)
            : base(span)
        {
            Argument = argument;
            Operator = op;
        }

        public override string ToString()
        {
            return Argument.ToString();
        }
    }

    public enum UnaryOperator
    {
        Default,
        PreIncrement,
        PostIncrement,
        PreDecrement,
        PostDecrement,
        Negation,
        Not,
    }
}
