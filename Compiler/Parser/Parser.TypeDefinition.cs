using Compiler.Syntax.Declarations;
using Compiler.Syntax.Expressions;
using Compiler.Syntax.Statements;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser
{
    public sealed partial class Parser
    {

        private TypeDeclaration ParseTypeDeclaration(AnnotationStatement annotations)
        {
            var typeKeyword = _current.Span.Start;
            Take();
            var name = ParseName();
            var fields = new List<TypeFieldDeclaration>();

            if (_current == TokenKind.Assignment)
            {
                // we're into the field definitions now...
                Take();

                while (_current == TokenKind.Indent)
                {
                    var fieldAnnotations = new AnnotationStatement();
                    while(IsNext(TokenKind.Annotation))
                    {
                        Take(TokenKind.Indent);
                        fieldAnnotations.Add(Take(TokenKind.Annotation));
                    }

                    var start = _current.Span.Start;
                    Take(TokenKind.Indent);
                    var fieldName = ParseName();
                    TakeWith(TokenKind.Colon);
                    var fieldType = ParseFieldTypeDefinition(new AnnotationStatement());

                    var fieldRestrictions = new List<TypeFieldRestriction>();
                    while (IsTypeFieldRestriction())
                    {
                        fieldRestrictions.Add(ParseTypeFieldRestriction());
                    }

                    var defaultExpression = fieldRestrictions.FirstOrDefault(f => f.Name == "default");

                    TakeWith(TokenKind.Semicolon);


                    // , fieldRestrictions, defaultExpression?.Value
                    fields.Add(new TypeFieldDeclaration(CreateSpan(start), fieldName, fieldType, fieldAnnotations));
                }

            }

            return new TypeDeclaration(CreateSpan(typeKeyword), name, annotations, fields);
        }

        private TypeFieldTypeDeclaration ParseFieldTypeDefinition(AnnotationStatement annotation)
        {
            var typeIdentifier = _current.Span.Start;
            var fieldTypes = new List<string> { ParseName() };
            while (_current == TokenKind.Dot)
            {
                Take();
                fieldTypes.Add(ParseName());
            }

            var fieldRestrictions = new List<TypeFieldRestriction>();
            while (IsTypeFieldRestriction())
            {
                fieldRestrictions.Add(ParseTypeFieldRestriction());
            }

            var defaultExpression = fieldRestrictions.FirstOrDefault(f => f.Name == "default");

            return new TypeFieldTypeDeclaration(CreateSpan(typeIdentifier), fieldTypes, fieldRestrictions, defaultExpression, annotation);
        }

        private TypeFieldRestriction ParseTypeFieldRestriction()
        {
            var annotation = new AnnotationStatement();
            while (IsNext(TokenKind.Annotation))
            {
                var (_, anno) = TakeWith(TokenKind.Annotation);
                annotation.Add(anno);
            }

            if (!IsTypeFieldRestriction()) throw new SyntaxException("Invalid type field restriction.");
            var (start, t) = TakeWith(TokenKind.BitwiseAnd);
            var name = Take().Value;
            var value = (ConstantExpression)ParseConstantExpression();
            return new TypeFieldRestriction(CreateSpan(start), name, value, annotation);
        }

        private bool IsTypeFieldRestriction()
         {
            return IsNext(TokenKind.BitwiseAnd, true);
        }
    }
}
