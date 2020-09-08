using Compiler.Syntax.Declarations;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Syntax
{
    public class SourceDocument : SyntaxNode
    {
        public override SyntaxCatagory Catagory => SyntaxCatagory.Document;

        public IEnumerable<SyntaxNode> Children { get; }

        public override SyntaxKind Kind => SyntaxKind.SourceDocument;

        public SourceCode SourceCode { get; }

        public SourceDocument(SourceSpan span, SourceCode sourceCode, IEnumerable<SyntaxNode> children)
            : base(span)
        {
            SourceCode = sourceCode;
            Children = children;
        }

        public T? Child<T>(int index) where T: SyntaxNode
        {
            var list = Children.ToList();
            if (index > list.Count - 1)
            {
                return null; 
            }
            return list[index] as T;
        }

        public T? Find<T>(string name) where T: Declaration
        {
            return (T)Children
                .OfType<Declaration>()
                .FirstOrDefault(n => n.Name == name) ?? null;
        }

    }
}
