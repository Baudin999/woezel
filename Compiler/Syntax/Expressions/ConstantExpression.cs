namespace Compiler.Syntax.Expressions
{
    public class ConstantExpression : Expression
    {
        public ConstantKind ConstantKind { get; }

        public override SyntaxKind Kind => SyntaxKind.ConstantExpression;

        public override ExpressionType Type { get; }

        public string Value { get; }

        public ConstantExpression(SourceSpan span, string value, ConstantKind kind)
            : base(span)
        {
            Value = value;
            ConstantKind = kind;
            Type = kind switch
            {
                ConstantKind.Integer => ExpressionType.Number,
                ConstantKind.String => ExpressionType.String,
                ConstantKind.Boolean => ExpressionType.Boolean,
                ConstantKind.Float => ExpressionType.Float,
                _ => ExpressionType.Indetermined

            };
        }
    }

    public enum ConstantKind
    {
        Invalid,
        Integer,
        Float,
        String,
        Boolean
    }
}
