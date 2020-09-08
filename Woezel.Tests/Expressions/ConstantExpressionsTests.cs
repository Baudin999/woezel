using Compiler.Syntax.Declarations;
using Compiler.Syntax.Expressions;
using Xunit;

namespace Woezel.Tests.Expressions
{
    public class ConstantExpressionsTests
    {

        [Fact]
        public void StringExpression()
        {
            var (AST, Errors) = TestCompiler.Compile("let x = 2");
            Assert.Empty(Errors);
            Assert.Single(AST.Children);

            var exp = AST.Find<VariableDeclaration>("x");
            Assert.NotNull(exp);
            Assert.Equal("x", exp.Name);

            Assert.NotNull(exp.Id);
            Assert.IsType<IdentifierExpression>(exp.Id);
            Assert.Equal("2", ((ConstantExpression)exp.Init).Value);
        }

        [Fact]
        public void AddExpression()
        {
            var (AST, Errors) = TestCompiler.Compile("let x = 2 + 2");
            Assert.Empty(Errors);
            Assert.Single(AST.Children);

            var exp = AST.Find<VariableDeclaration>("x");
            Assert.NotNull(exp);
            Assert.Equal("x", exp.Name);
        }

        [Fact]
        public void ChainedAddExpression()
        {
            var (AST, Errors) = TestCompiler.Compile("let x = 2 + 2 + 2 + 2");
            Assert.Empty(Errors);
            Assert.Single(AST.Children);

            var exp = AST.Find<VariableDeclaration>("x");
            Assert.NotNull(exp);
            Assert.Equal("x", exp.Name);
        }

        [Fact]
        public void ReferenceExpression()
        {
            var (AST, Errors) = TestCompiler.Compile("let x = person.Name");
            Assert.Empty(Errors);
            Assert.Single(AST.Children);

            var exp = AST.Find<VariableDeclaration>("x");
            Assert.NotNull(exp);
            Assert.Equal("x", exp.Name);
        }

        [Fact]
        public void WithExpression()
        {
            var (AST, Errors) = TestCompiler.Compile(@"
let five = b + a
    with
        let a = 2
        let b = 3
");
            Assert.Empty(Errors);
            Assert.Single(AST.Children);

            var exp = AST.Find<VariableDeclaration>("five");
            Assert.NotNull(exp);
            Assert.Equal("five", exp.Name);

            Assert.Equal(2, exp.WithDeclarations.Count);
        }


    }
}
