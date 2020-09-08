using Compiler.Syntax.Declarations;
using Compiler.Syntax.Statements;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser
{
    public sealed partial class Parser
    {

        private AliasDeclaration ParseAliasDeclaration(AnnotationStatement annotations)
        {
            var aliasKeyword = _current.Span.Start;
            Take();
            var name = ParseName();
            var fields = new List<TypeFieldDeclaration>();

            TakeAliasAssignment();
            
            var fieldType = ParseFieldTypeDefinition(new AnnotationStatement());

            var fieldRestrictions = new List<TypeFieldRestriction>();
            while (IsTypeFieldRestriction())
            {
                fieldRestrictions.Add(ParseTypeFieldRestriction());
            }

            var defaultExpression = fieldRestrictions.FirstOrDefault(f => f.Name == "default");

            TakeWith(TokenKind.Semicolon);


            return new AliasDeclaration(CreateSpan(aliasKeyword), name, fieldType, annotations);
        }

        private void TakeAliasAssignment()
        {
            if (_current != TokenKind.Assignment)
            {
                _errorSink.AddError($@"
Invalid Syntax Error,
Expected '=' but found '{_current.Value}' on line {_current.Span.Start.Line}, column {_current.Span.Start.Column}.

When defining an 'alias' we expect an '=' between the name
of the alias and the type of the alias.

Example:
alias Name = String;
", _sourceCode, Severity.Error, _current.Span
);
            } else
            {
                Take(TokenKind.Assignment);
            }
        }
    }

    
}
