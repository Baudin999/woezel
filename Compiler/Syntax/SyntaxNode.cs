using Compiler.Syntax.Statements;

namespace Compiler.Syntax
{
    public abstract class SyntaxNode
    {

        public abstract SyntaxCatagory Catagory { get; }

        public abstract SyntaxKind Kind { get; }

        public SourceSpan Span { get; }

        protected SyntaxNode(SourceSpan span)
        {
            Span = span;
        }

        public override string ToString()
        {
            return $"{Catagory} {Kind} {Span}";
        }
    }

    //public abstract class AnnotatedSyntaxNode : SyntaxNode
    //{
    //    public string Name { get; }

    //    public AnnotationStatement Annotation { get; }

    //    protected AnnotatedSyntaxNode(SourceSpan span, string name, AnnotationStatement annotation) : base(span)
    //    {
    //        this.Name = name;
    //        this.Annotation = annotation;
    //    }

    //    public override string ToString()
    //    {
    //        return $"{Catagory} {Kind} {Span}";
    //    }
    //}
}
