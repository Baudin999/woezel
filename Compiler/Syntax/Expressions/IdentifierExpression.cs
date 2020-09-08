namespace Compiler.Syntax.Expressions
{
    public class IdentifierExpression : Expression
    {
        public string Name { get; }

        public override SyntaxKind Kind => SyntaxKind.IdentifierExpression;

        public IdentifierExpression(SourceSpan span, string identifier) : base(span)
        {
            Name = identifier;
        }

        public override string ToString()
        {
            return $"id_exp: {Name}";
        }
    }
}
