
using Compiler;
using Compiler.Lexer;
using Compiler.Parser;
using Compiler.Syntax.Declarations;
using System.Linq;
using Xunit;

namespace Woezel.Tests
{
    public class AliasTest
    {

        [Fact]
        public void SimpelAlias()
        {
            var code = @"
@ This is the address annotation
@ over multiple lines.
alias Name = String
    & min 3
    & max 20
;

";
            var (ast, errors) = TestCompiler.Compile(code);

            Assert.NotNull(ast);
            Assert.Single(ast.Children);
            Assert.Empty(errors);

            var nameAlias = ast.Find<AliasDeclaration>("Name");
            Assert.NotNull(nameAlias);
            Assert.Equal(2, nameAlias.Restrictions.Count);

        }


        [Fact]
        public void MissingEquals()
        {
            var code = @"
@ This is the address annotation
@ over multiple lines.
alias Name String
    & min 3
    & max 20
;

";
            var (AST, Errors) = TestCompiler.Compile(code);

            Assert.NotNull(AST);
            Assert.Single(Errors);
            

        }

    }
}
