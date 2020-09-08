using Compiler.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser
{
    public sealed partial class Parser
    {
        private bool _error;
        private ErrorSink _errorSink;
        private int _index;
        private ParserOptions _options;
        private SourceCode _sourceCode;
        private IEnumerable<Token> _tokens;

        private Token Peek(int ahead) => _tokens.ElementAtOrDefault(_index + ahead) ?? _tokens.Last();

        private Token _current => _tokens.ElementAtOrDefault(_index) ?? _tokens.Last();

        private Token _previous => Peek(-1);

        private Token _next => Peek(1);

        private Token _peekAt(int n) => Peek(n);

        public bool HasErrors => _errorSink.HasErrors;
        public IEnumerable<ErrorEntry> Errors => _errorSink.Errors;

        public Parser()
            : this(new ErrorSink())
        {
        }

        public Parser(ErrorSink errorSink)
        {
            _errorSink = errorSink;
            _options = ParserOptions.Default;
            _sourceCode = new SourceCode("");
            _tokens = Enumerable.Empty<Token>();
        }

        private void AddError(Severity severity, string message, SourceSpan? span = null)
        {
            _errorSink.AddError(message, _sourceCode, severity, span ?? CreateSpan(_current));
        }

        private void Advance()
        {
            _index++;
        }

        private SourceSpan CreateSpan(SourceLocation start, SourceLocation end)
        {
            return new SourceSpan(start, end);
        }

        private SourceSpan CreateSpan(Token start)
        {
            return CreateSpan(start.Span.Start, _current.Span.End);
        }

        private SourceSpan CreateSpan(SyntaxNode start)
        {
            return CreateSpan(start.Span.Start, _current.Span.End);
        }

        private SourceSpan CreateSpan(SourceLocation start)
        {
            return CreateSpan(start, _current.Span.End);
        }

        private void InitializeParser(SourceCode sourceCode, IEnumerable<Token> tokens, ParserOptions options)
        {
            _sourceCode = sourceCode;
            _tokens = tokens.Where(g => !g.IsTrivia()).ToArray();
            _options = options;
            _index = 0;
        }

        private void MakeBlock(Action action, TokenKind openKind = TokenKind.LeftBracket, TokenKind closeKind = TokenKind.RightBracket)
        {
            Take(openKind);

            MakeStatement(action, closeKind);
        }

        private void MakeStatement(Action action, TokenKind closeKind = TokenKind.Semicolon)
        {
            try
            {
                while (_current != closeKind && _current != TokenKind.EndOfFile)
                {
                    action();
                }
            }
            catch (SyntaxException)
            {
                while (_current != closeKind && _current != TokenKind.EndOfFile)
                {
                    Take();
                }
            }
            finally
            {
                if (_error)
                {
                    if (_previous == closeKind)
                    {
                        _index--;
                    }
                    if (_current != closeKind)
                    {
                        while (_current != closeKind && _current != TokenKind.EndOfFile)
                        {
                            Take();
                        }
                    }
                    _error = false;
                }
                if (closeKind == TokenKind.Semicolon)
                {
                    TakeSemicolon();
                }
                else
                {
                    Take(closeKind);
                }
            }
        }

        private SyntaxException SyntaxError(Severity severity, string message, SourceSpan? span = null)
        {
            _error = true;
            AddError(severity, message, span);
            return new SyntaxException(message);
        }

        private Token Take()
        {
            var token = _current;
            Advance();
            return token;
        }

        private Token Take(TokenKind kind)
        {
            if (_current != kind)
            {
                throw UnexpectedToken(kind);
            }
            return Take();
        }

        private (SourceLocation, Token) TakeWith(TokenKind kind)
        {
            while (_current == TokenKind.Indent) Take();
            if (_current != kind)
            {
                throw UnexpectedToken(kind);
            }
            return (_current.Span.Start, Take());
        }

        private (SourceLocation, Token) TakeWith(string value)
        {
            while (_current == TokenKind.Indent) Take();
            if (_current != value)
            {
                throw UnexpectedToken(value);
            }
            return (_current.Span.Start, Take());
        }

        /// <summary>
        /// Checks if the next token is the kind we're looking for. We can include annotations which are 
        /// not included by default.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="includeAnnotations"></param>
        /// <returns></returns>
        private bool IsNext(TokenKind kind, bool includeAnnotations = false)
        {
            var index = 0;
            while (Peek(index) == TokenKind.Indent || (includeAnnotations && Peek(index) == TokenKind.Annotation)) index++;

            return Peek(index) == kind;
        }
        private bool IsNext(string value, bool includeAnnotations = false)
        {
            var index = 0;
            while (Peek(index) == TokenKind.Indent || (includeAnnotations && Peek(index) == TokenKind.Annotation)) index++;

            return Peek(index) == value;
        }

        private Token Take(string contextualKeyword)
        {
            if (_current != TokenKind.Identifier && _current != contextualKeyword)
            {
                throw UnexpectedToken(contextualKeyword);
            }
            return Take();
        }

        private Token TakeKeyword(string keyword, bool ignoreIndentation = false)
        {
            if (ignoreIndentation)
            {
                while (_current == TokenKind.Indent) Take();
            }

            if (_current != TokenKind.Keyword && _current != keyword)
            {
                throw UnexpectedToken(keyword);
            }
            return Take();
        }

        private Token TakeSemicolon()
        {
            if (_options.EnforceSemicolons || _current == TokenKind.Semicolon)
            {
                return Take(TokenKind.Semicolon);
            }
            return _current;
        }

        private Token? TryTake(TokenKind kind)
        {
            if (_current == kind) return Take(kind);
            else return null;
        }

        private SyntaxException UnexpectedToken(TokenKind expected)
        {
            return UnexpectedToken(expected.ToString());
        }

        private SyntaxException UnexpectedToken(string expected)
        {
            Advance();
            var value = string.IsNullOrEmpty(_previous?.Value) ? _previous?.Kind.ToString() : _previous?.Value;
            string message = $"Unexpected '{value}'.  Expected '{expected}'";

            return SyntaxError(Severity.Error, message, _previous?.Span);
        }

        private bool IsIdentifier()
        {
            return _current.Kind == TokenKind.Identifier;
        }

        private bool IsConstant()
        {
            return _current.Value == "true" || 
                   _current.Value == "false" || 
                   _current.Kind == TokenKind.StringLiteral || 
                   _current.Kind == TokenKind.IntegerLiteral || 
                   _current.Kind == TokenKind.FloatLiteral;
        }
    }
}
