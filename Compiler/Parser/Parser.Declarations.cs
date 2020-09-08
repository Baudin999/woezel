using Compiler.Syntax;
using Compiler.Syntax.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser
{
    public sealed partial class Parser
    {
       

        private SourceDocument ParseDocument()
        {
            List<SyntaxNode> contents = new List<SyntaxNode>();

            var start = _current.Span.Start;

            while (_current != TokenKind.EndOfFile)
            {
                try
                {
                    var annotations = new AnnotationStatement();
                    if (_current == TokenKind.Annotation)
                    {
                        while (_current == TokenKind.Annotation)
                        {
                            annotations.Add(Take(TokenKind.Annotation));
                        }
                    }
                    else if (_current == TokenKind.Semicolon)
                    {
                        // ignore semicolons
                        Take();
                    }
                    else if (_current == "let")
                    {
                        contents.Add(ParseLetDeclaration(annotations));
                    }
                    else if (_current == "type")
                    {
                        contents.Add(ParseTypeDeclaration(annotations));
                    }
                    else if (_current == "alias")
                    {
                        contents.Add(ParseAliasDeclaration(annotations));
                    }
                    else if (_current == TokenKind.NewLine)
                    {
                        // ignore newlines (for now)
                        Take();
                    }
                    else
                    {
                        var paragraph = new List<string>();
                        while (_current != TokenKind.EndBlock && _current != TokenKind.EndOfFile)
                        {
                            paragraph.Add(Take().Value);
                        }
                        if (paragraph.Count > 0)
                        {
                            contents.Add(new MarkdownStatement(CreateSpan(start), string.Join(" ", paragraph)));
                        }
                        Take();
                    }
                }
                catch(Exception ex)
                {
                    // here a syntax exception happened. Parse the rest as if nothing happenend
                    // the rest of the block should be shown as Markdown!
                    Console.WriteLine("An exception has occured, please check the details and fix the syntax error.");
                    Console.WriteLine(ex.Message);
                }
            }


            if (_current != TokenKind.EndOfFile)
            {
                AddError(Severity.Error, "Top-level statements are not permitted within the current options.", CreateSpan(_current.Span.Start, _tokens.Last().Span.End));
            }

            return new SourceDocument(CreateSpan(start), _sourceCode, contents);
        }

     

        private string ParseName()
        {
            return Take(TokenKind.Identifier).Value;
        }

        private string ParseTypeAnnotation()
        {
            if (_current != TokenKind.LessThan)
            {
                throw UnexpectedToken("Type Annotation");
            }
            Take(TokenKind.LessThan);
            var identifier = ParseName();
            Take(TokenKind.GreaterThan);

            return identifier;
        }

      
    }
}
