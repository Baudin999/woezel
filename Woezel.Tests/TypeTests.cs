using Compiler;
using Compiler.Lexer;
using Compiler.Parser;
using Compiler.Syntax.Declarations;
using Compiler.Syntax.Expressions;
using System.Linq;
using Xunit;

namespace Woezel.Tests
{
    public class TypeTests
    {

        [Fact]
        public void ComplexType()
        {
            var _code = new SourceCode(@"
@ This is the address annotation
@ over multiple lines.
type Address =

    @ This is the street
    @ And some more annotations
    Street: Locations.Street 

        @ We'll really want to have a default
        @ value for the Street, because we're
        @ Testing.
        & default ""Weezenhof""
    ;

    @ Something
    Postcode: Number
        @ Example: 1111AA
        & min 6

        @ Example: 1111 AA
        & max 7
    ;

type Person =
    Address: Address;

");
            var lexer = new Lexer();
            var tokens = lexer.LexFile(_code).ToArray();
            var parser = new Parser(lexer.ErrorSink);
            var ast = parser.ParseFile(_code, tokens, ParserOptions.OptionalSemicolons);

            Assert.NotNull(ast);
            Assert.Equal(2, ast.Children.Count());

            var address = ast.Child<TypeDeclaration>(0);
            Assert.NotNull(address);
            Assert.Equal(2, address.Fields.Count);

            var street = address.Fields.First();
            Assert.NotNull(street.Annotation);
            Assert.Equal("This is the street And some more annotations", street.Annotation.Value);
            Assert.Single(street.Restrictions);
            Assert.NotNull(street.DefaultExpression);
            Assert.NotNull(street.DefaultExpression.ValueExpression);
            Assert.IsType<ConstantExpression>(street.DefaultExpression.ValueExpression);
            Assert.Equal("Weezenhof", (street.DefaultExpression.ValueExpression as ConstantExpression).Value);
            Assert.Equal("Weezenhof", street.Default);
            Assert.Equal(ConstantKind.String, (street.DefaultExpression.ValueExpression as ConstantExpression).ConstantKind);


            var postcode = address.Fields.Last();
            var minRestriction = postcode.Restrictions.First();
            Assert.NotNull(minRestriction);
            Assert.NotNull(minRestriction.Annotation);
            Assert.Equal("Example: 1111AA", minRestriction.Annotation.Value);
            Assert.IsType<ConstantExpression>(minRestriction.ValueExpression);
            Assert.Equal(6, System.Int32.Parse(minRestriction.Value));
            Assert.Equal(ConstantKind.Integer, (minRestriction.ValueExpression as ConstantExpression).ConstantKind);

            var maxRestriction = postcode.Restrictions.Last();
            Assert.NotNull(maxRestriction);
            Assert.NotNull(maxRestriction.Annotation);
            Assert.Equal("Example: 1111 AA", maxRestriction.Annotation.Value);
            Assert.IsType<ConstantExpression>(maxRestriction.ValueExpression);
            Assert.Equal(7, System.Int32.Parse(maxRestriction.Value));
            Assert.Equal(ConstantKind.Integer, (maxRestriction.ValueExpression as ConstantExpression).ConstantKind);
        }

       

        [Fact(DisplayName = "Type Sticking Together")]
        public void TypesStickingTogether()
        {
            var _code = new SourceCode(@"
type Person
type Addres =
    Street: String;
    Postcode: String;
type Buiding

");
            var lexer = new Lexer();
            var tokens = lexer.LexFile(_code).ToArray();
            var parser = new Parser(lexer.ErrorSink);
            var ast = parser.ParseFile(_code, tokens, ParserOptions.OptionalSemicolons);


            Assert.NotNull(ast);
            Assert.Equal(3, ast.Children.Count());
        }
    }
}
