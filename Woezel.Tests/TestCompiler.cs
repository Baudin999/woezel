using Compiler;
using Compiler.Lexer;
using Compiler.Parser;
using Compiler.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Woezel.Tests
{
    internal class TestCompiler
    {
        internal static CompilationResult Compile(string code)
        {
            var _code = new SourceCode(code);
            var lexer = new Lexer();
            var tokens = lexer.LexFile(_code).ToArray();
            var parser = new Parser(lexer.ErrorSink);
            var ast = parser.ParseFile(_code, tokens, ParserOptions.OptionalSemicolons);

            return new CompilationResult(ast, parser.Errors);
        }
    }

    internal class CompilationResult: IEquatable<CompilationResult>
    {
        public SourceDocument AST { get; }
        public IEnumerable<ErrorEntry> Errors { get; }

        public CompilationResult(SourceDocument aST, IEnumerable<ErrorEntry> errors)
        {
            AST = aST;
            Errors = errors;
        }

        public override bool Equals(object obj)
        {
            return obj is CompilationResult other &&
                   EqualityComparer<SourceDocument>.Default.Equals(AST, other.AST) &&
                   EqualityComparer<IEnumerable<ErrorEntry>>.Default.Equals(Errors, other.Errors);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AST, Errors);
        }

        public bool Equals(CompilationResult other)
        {
            return Equals(other);
        }

        public void Deconstruct(out SourceDocument ast, out IEnumerable<ErrorEntry> errors)
        {
            ast = AST;
            errors = Errors;
        }

    }
}
