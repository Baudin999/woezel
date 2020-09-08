using Compiler.Syntax.Declarations;
using Compiler.Syntax.Expressions;
using Compiler.Syntax.Statements;
using System;
using System.Collections.Generic;

namespace Compiler.Parser
{
    public sealed partial class Parser
    {

        private VariableDeclaration ParseLetDeclaration(AnnotationStatement annotations)
        {
            var letKeyword = _current.Span;
            TakeKeyword("let", true);

            var expression = ParseExpression();
           
            var declarations = new List<Declaration>();
            if (IsNext("with"))
            {
                var r = TakeWith("with");
                while (IsNext("let", true))
                {
                    var letDeclaration = ParseLetDeclaration(new AnnotationStatement());
                    declarations.Add(letDeclaration);
                }
            }

            VariableDeclaration result;
            if (expression is LambdaExpression l)
            {
                result = new VariableDeclaration(CreateSpan(letKeyword.Start), l.Name, l.Id, expression, declarations, annotations);
            }
            else if (expression is BinaryExpression b)
            {
                var id = b.Left as IdentifierExpression ?? throw new SyntaxException($"Expected an Indentifier but got a {b.Kind}.");
                result = new VariableDeclaration(CreateSpan(letKeyword.Start), id.Name, b.Left, b.Right, declarations, annotations);
            }
            else
            {
                throw new SyntaxException("Invalid variable declaration.");
            }

            return result;

        }


    }


}
