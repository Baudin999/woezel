
using Compiler;
using Compiler.Lexer;
using Compiler.Parser;
using System.Linq;
using Xunit;

namespace Woezel.Tests
{
    public class BasicTests
    {


        [Fact]
        public void EntryPoint()
        {
            var _code = new SourceCode(@"

open System importing ( Console );

func main () => 
    let x = 2 + 3;
    Console.log x;

");
            var lexer = new Lexer();
            var tokens = lexer.LexFile(_code).ToArray();
            var parser = new Parser(lexer.ErrorSink);
            var ast = parser.ParseFile(_code, tokens, ParserOptions.OptionalSemicolons);


            Assert.NotNull(ast);
            Assert.Equal(2, ast.Children.Count());
        }


        [Fact]
        public void TestSomething()
        {
            var _code = new SourceCode(@"
# Header 1

Dit is nog meer informatie,
waar wij dingen mee kunnen uitvoeren.
En nog wat meer info

@ The Person type
@ This defines the person
type Person


type Address =
    Street: String;
    PostCode: String
        & min 12
        & max 32
    ;



@ The building itself
type Building =
    Street: Address.Street;

");
            var lexer = new Lexer();
            var tokens = lexer.LexFile(_code).ToArray();
            var parser = new Parser(lexer.ErrorSink);
            var ast = parser.ParseFile(_code, tokens, ParserOptions.OptionalSemicolons);


            Assert.NotNull(ast);
            Assert.Equal(5, ast.Children.Count());
        }

       
    }
}
