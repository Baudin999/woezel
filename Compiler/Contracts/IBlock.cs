using Compiler.Syntax;

namespace Compiler.Contracts
{
    public interface IBlock
    {
        string Name { get; }
        string Keyword { get; }

        SyntaxNode BlockParser(Compiler.Parser.Parser parser);
    }
}
