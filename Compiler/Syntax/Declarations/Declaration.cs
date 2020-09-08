using Compiler.Syntax.Statements;

namespace Compiler.Syntax.Declarations
{
    public abstract class Declaration : SyntaxNode
    {
        public override SyntaxCatagory Catagory => SyntaxCatagory.Declaration;

        public string Name { get; }

        public AnnotationStatement Annotation { get; }


        protected Declaration(SourceSpan span, string name, AnnotationStatement annotation) : base(span)
        {
            this.Name = name;
            this.Annotation = annotation;
        }
    }
}
