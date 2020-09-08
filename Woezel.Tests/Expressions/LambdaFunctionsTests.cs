using Compiler.Syntax.Declarations;
using Compiler.Syntax.Expressions;
using Xunit;

namespace Woezel.Tests.Expressions
{
    public class LambdaFunctionsTests
    {
        [Fact]
        public void LambdaExpression()
        {
            var (AST, Errors) = TestCompiler.Compile("let add x y => x + y");
            Assert.Empty(Errors);
            Assert.Single(AST.Children);

            var exp = AST.Find<VariableDeclaration>("add");
            Assert.NotNull(exp);
            Assert.Equal("add", exp.Name);
            var lambda = (LambdaExpression)exp.Init;
            Assert.Equal(2, lambda.Parameters.Count);
        }

        [Fact(DisplayName = "No parameters lambda")]
        public void NoParamsLambdaExpression()
        {
            var (AST, Errors) = TestCompiler.Compile("let returnTwo () => 2");
            Assert.Empty(Errors);
            Assert.Single(AST.Children);

            var exp = AST.Find<VariableDeclaration>("returnTwo");
            Assert.NotNull(exp);
            Assert.Equal("returnTwo", exp.Name);

            var lambda = (LambdaExpression)exp.Init;
            Assert.Empty(lambda.Parameters);
        }

        [Fact(DisplayName = "ReturnType: Number")]
        public void ReturnType_Number()
        {
            var (AST, Errors) = TestCompiler.Compile("let returnTwo () => 2");
            Assert.Empty(Errors);
            Assert.Single(AST.Children);

            var exp = AST.Find<VariableDeclaration>("returnTwo");
            Assert.NotNull(exp);
            Assert.Equal("returnTwo", exp.Name);

            var lambda = (LambdaExpression)exp.Init;
            Assert.Empty(lambda.Parameters);
        }
    }
}
