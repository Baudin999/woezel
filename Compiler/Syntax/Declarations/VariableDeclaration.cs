using Compiler.Syntax.Expressions;
using Compiler.Syntax.Statements;
using System.Collections.Generic;

namespace Compiler.Syntax.Declarations
{
    public class VariableDeclaration : Declaration
    {
        public override SyntaxKind Kind => SyntaxKind.VariableDeclaration;
        public Expression Id { get; }
        public Expression Init { get; }
        public List<Declaration> WithDeclarations { get; }

        public VariableDeclaration(
            SourceSpan span, 
            string name, 
            Expression id,
            Expression exp, 
            List<Declaration> withDeclarations,
            AnnotationStatement annotation) : base(span, name, annotation)
        {
            this.Id = id;  
            this.Init = exp;
            this.WithDeclarations = withDeclarations;
        }
    }
}
