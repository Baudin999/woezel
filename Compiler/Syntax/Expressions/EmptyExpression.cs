namespace Compiler.Syntax.Expressions
{
    public class EmptyExpression : Expression
    {
        public override SyntaxKind Kind => SyntaxKind.EmptyExpression;

        public EmptyExpression(): base(new SourceSpan())
        {

        }
    }
}
