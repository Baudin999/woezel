using Compiler.Syntax.Declarations;

namespace Compiler.Syntax.Statements
{
    internal class MarkdownStatement : Statement
    {
        private readonly string Value;

        public MarkdownStatement(SourceSpan span, string value) : base(span)
        {
            this.Value = value;
        }

        public override SyntaxKind Kind => SyntaxKind.Markdown;
    }
}