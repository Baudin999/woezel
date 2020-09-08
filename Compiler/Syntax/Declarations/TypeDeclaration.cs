using Compiler.Syntax.Expressions;
using Compiler.Syntax.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Syntax.Declarations
{
    public class TypeDeclaration: Declaration
    {
        public List<TypeFieldDeclaration> Fields { get; }

        public TypeDeclaration(SourceSpan sourceSpan, string name, AnnotationStatement annotation, List<TypeFieldDeclaration> fields) : base(sourceSpan, name, annotation)
        {
            this.Fields = fields;
        }

        public override SyntaxCatagory Catagory => SyntaxCatagory.Declaration;

        public override SyntaxKind Kind => SyntaxKind.TypeDeclaration;

    }

    public class TypeFieldDeclaration : Declaration
    {
        public SourceSpan SourceSpan { get; }
        public TypeFieldTypeDeclaration FieldType { get; }
        public List<TypeFieldRestriction> Restrictions => FieldType.Restrictions;
        public TypeFieldRestriction? DefaultExpression => FieldType.DefaultExpression;
        public string? Default => FieldType.DefaultExpression?.ValueExpression?.Value ?? null;
        public List<string> Types => FieldType.FieldTypes;
        public override SyntaxKind Kind => SyntaxKind.FieldDeclaration;


        public TypeFieldDeclaration(
            SourceSpan sourceSpan, 
            string name,
            TypeFieldTypeDeclaration fieldType, 
            AnnotationStatement fieldAnnotations) : base(sourceSpan, name, fieldAnnotations)
        {
            this.SourceSpan = sourceSpan;
            this.FieldType = fieldType;
        }
    }

    public class TypeFieldTypeDeclaration : Declaration
    {
        public List<string> FieldTypes { get; }
        public List<TypeFieldRestriction> Restrictions { get; }
        public TypeFieldRestriction? DefaultExpression { get; }

        public TypeFieldTypeDeclaration(
            SourceSpan sourceSpan, 
            List<string> fieldTypes, 
            List<TypeFieldRestriction> restrictions, 
            TypeFieldRestriction? defaultExpression, 
            AnnotationStatement annotation) : base(sourceSpan, "", annotation)
        {
            this.FieldTypes = fieldTypes;
            this.Restrictions = restrictions;
            this.DefaultExpression = defaultExpression;
        }

        public override SyntaxCatagory Catagory => SyntaxCatagory.Declaration;

        public override SyntaxKind Kind => SyntaxKind.FieldTypeDeclaration;
    }

    public class TypeFieldRestriction : Declaration
    {
        public ConstantExpression? ValueExpression { get; }

        // Value unwrappers
        public string Value => ValueExpression?.Value ?? "";
        //public int IntValue => Int32.Parse(ValueExpression?.Value ?? "0");
        //public bool BoolValue => bool.Parse(ValueExpression?.Value ?? "false");

        public override SyntaxCatagory Catagory => SyntaxCatagory.Declaration;

        public override SyntaxKind Kind => SyntaxKind.TypeDeclaration;

        public TypeFieldRestriction(
            SourceSpan sourceSpan, 
            string name, 
            ConstantExpression? valueExpression, 
            AnnotationStatement annotation) : base(sourceSpan, name, annotation)
        {
            this.ValueExpression = valueExpression;
        }
    }
}
