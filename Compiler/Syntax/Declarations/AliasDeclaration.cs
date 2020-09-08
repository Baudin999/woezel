using Compiler.Syntax.Expressions;
using Compiler.Syntax.Statements;
using System;
using System.Collections.Generic;

namespace Compiler.Syntax.Declarations
{
    public class AliasDeclaration : Declaration
    {
        public TypeFieldTypeDeclaration Type { get; }
        public List<TypeFieldRestriction> Restrictions => Type.Restrictions;
        public TypeFieldRestriction? DefaultExpression => Type.DefaultExpression;
        public string? Default => Type.DefaultExpression?.ValueExpression?.Value ?? null;
        public override SyntaxCatagory Catagory => throw new NotImplementedException();
        public override SyntaxKind Kind => SyntaxKind.AliasDeclaration;

        public AliasDeclaration(
            SourceSpan sourceSpan,
            string name,
            TypeFieldTypeDeclaration fieldType,
            AnnotationStatement annotation) : base(sourceSpan, name, annotation)
        {
            this.Type = fieldType;
        }
    }
}
