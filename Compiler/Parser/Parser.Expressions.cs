using Compiler.Syntax.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser
{
    public sealed partial class Parser
    {

        private Expression ParseExpression()
        {
            var span = _current.Span;
            var left = ExpressionBuilder();
            if (_current == TokenKind.Assignment)
            {
                // assignment operator
                // Example: x = 2
                var op = ParseBinaryOperator();
                var right = ParseExpression();
                return new BinaryExpression(span, left, right, op);
            }
            else if (_current == TokenKind.Dot)
            {
                // reference type
                // Example: person.Name
                return ParseReferenceExpression(left);
            }
            else if (_current.Catagory == TokenCatagory.Operator)
            {
                // binary operator 
                // Example: 2 + 4
                var op = ParseBinaryOperator();
                var right = ParseExpression();
                return new BinaryExpression(span, left, right, op);
            }
            else if (
                _current == TokenKind.Identifier || 
                (_current == TokenKind.LeftParenthesis && _next == TokenKind.RightParenthesis))
            {
                // lambda function teritory
                return ParseLambdaExpression(left);
            }
            else
            {
                return left;
            }
        }

        private Expression ExpressionBuilder()
        {
            if (_current == TokenKind.Identifier)
            {
                return ParseIdentiferExpression();
            }
            else if (_current.Catagory == TokenCatagory.Constant)
            {
                return ParseConstantExpression();
            }
            else
            {
                throw new SyntaxException("Invalid Expression syntax");
            }
        }

        private Expression SubsequentExpressionBuilder(Expression start)
        {
            return new EmptyExpression();
        }

        private Expression ParseBinaryExpression(Token start)
        {
            return new EmptyExpression();
        }

        private Expression ParseLambdaExpression(Expression root)
        {
            var parameters = new List<Expression>();
            while (_current == TokenKind.Identifier)
            {
                parameters.Add(ParseIdentiferExpression());
            }

            TryTake(TokenKind.LeftParenthesis);
            TryTake(TokenKind.RightParenthesis);

            Take(TokenKind.FatArrow);

            var body = ParseExpression();

            return new LambdaExpression(
                root.Span, 
                (root as IdentifierExpression)?.Name ?? throw new SyntaxException($"Expected an Identifier but got a '{root.Kind}'"), 
                root,
                parameters, 
                body);
        }

        private Expression ParseIdentiferExpression()
        {
            var hint = _current.Span;
            return new IdentifierExpression(hint, Take().Value);
        }


        private Expression ParseReferenceExpression(Expression root)
        {
            var hint = root.Span;
            var identifiers = new List<string> { (root as IdentifierExpression)?.Name ?? throw new SyntaxException($"Expected an Identifier but got a '{root.Kind}'") };

            while (_current == TokenKind.Dot)
            {
                Take(TokenKind.Dot);
                identifiers.Add(Take().Value);
            }

            var idSpan = new SourceSpan(hint.Start, _current.Span.Start);
            return new ReferenceExpression(idSpan, string.Join(".", identifiers), identifiers);
        }



        private Expression ParseConstantExpression()
        {
            ConstantKind kind;
            if (_current == "true" || _current == "false")
            {
                kind = ConstantKind.Boolean;
            }
            else if (_current == TokenKind.StringLiteral)
            {
                kind = ConstantKind.String;
            }
            else if (_current == TokenKind.IntegerLiteral)
            {
                kind = ConstantKind.Integer;
            }
            else if (_current == TokenKind.FloatLiteral)
            {
                kind = ConstantKind.Float;
            }
            else
            {
                throw UnexpectedToken("Constant");
            }

            var token = Take();

            return new ConstantExpression(token.Span, token.Value, kind);
        }

        private BinaryOperator ParseBinaryOperator()
        {
            var token = Take();
            switch (token.Kind)
            {
                case TokenKind.Plus: return BinaryOperator.Add;
                case TokenKind.Minus: return BinaryOperator.Sub;
                case TokenKind.Assignment: return BinaryOperator.Assign;
                case TokenKind.PlusEqual: return BinaryOperator.AddAssign;
                case TokenKind.MinusEqual: return BinaryOperator.SubAssign;
                case TokenKind.MulEqual: return BinaryOperator.MulAssign;
                case TokenKind.DivEqual: return BinaryOperator.DivAssign;
                case TokenKind.ModEqual: return BinaryOperator.ModAssign;
                case TokenKind.BitwiseAndEqual: return BinaryOperator.AndAssign;
                case TokenKind.BitwiseOrEqual: return BinaryOperator.OrAssign;
                case TokenKind.BitwiseXorEqual: return BinaryOperator.XorAssign;
                case TokenKind.BitwiseAnd: return BinaryOperator.BitwiseAnd;
                case TokenKind.BitwiseOr: return BinaryOperator.BitwiseOr;
                case TokenKind.BitwiseXor: return BinaryOperator.BitwiseXor;
                case TokenKind.Equal: return BinaryOperator.Equal;
                case TokenKind.NotEqual: return BinaryOperator.NotEqual;
                case TokenKind.BooleanOr: return BinaryOperator.LogicalOr;
                case TokenKind.BooleanAnd: return BinaryOperator.LogicalAnd;
                case TokenKind.Mul: return BinaryOperator.Mul;
                case TokenKind.Div: return BinaryOperator.Div;
                case TokenKind.Mod: return BinaryOperator.Div;
                case TokenKind.GreaterThan: return BinaryOperator.GreaterThan;
                case TokenKind.LessThan: return BinaryOperator.LessThan;
                case TokenKind.GreaterThanOrEqual: return BinaryOperator.GreaterThanOrEqual;
                case TokenKind.LessThanOrEqual: return BinaryOperator.LessThanOrEqual;
                case TokenKind.BitShiftLeft: return BinaryOperator.LeftShift;
                case TokenKind.BitShiftRight: return BinaryOperator.RightShift;
            }

            _index--;
            throw UnexpectedToken("Binary Operator");
        }

    }
}
