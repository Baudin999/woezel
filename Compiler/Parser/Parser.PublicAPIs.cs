using Compiler.Syntax;
using System;
using System.Collections.Generic;

namespace Compiler.Parser
{
    public sealed partial class Parser
    {
     

        public SourceDocument? ParseFile(SourceCode sourceCode, IEnumerable<Token> tokens)
        {
            return ParseFile(sourceCode, tokens, ParserOptions.Default);
        }

        public SourceDocument? ParseFile(SourceCode sourceCode, IEnumerable<Token> tokens, ParserOptions options)
        {
            InitializeParser(sourceCode, tokens, options);
            try
            {
                return ParseDocument();
            }
            catch (SyntaxException sex)
            {
                Console.WriteLine(sex.Message);
                return null;
            }
        }

    }
}
