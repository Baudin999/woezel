using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Syntax.Expressions
{
    public class BinaryExpression : Expression
    {
        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;

        public Expression Left { get; }

        public BinaryOperator Operator { get; }

        public Expression Right { get; }

        public override ExpressionType Type { get; } = ExpressionType.Indetermined;

        public BinaryExpression(SourceSpan span, Expression left, Expression right, BinaryOperator op) : base(span)
        {
            Left = left;
            Right = right;
            Operator = op;

            var _t = (op, left.Type, right.Type);
            this.Type = _t switch
            {
                // Number operations
                (BinaryOperator.Add, ExpressionType.Number, ExpressionType.Number) => ExpressionType.Number,
                (BinaryOperator.Sub, ExpressionType.Number, ExpressionType.Number) => ExpressionType.Number,
                (BinaryOperator.Mul, ExpressionType.Number, ExpressionType.Number) => ExpressionType.Number,
                (BinaryOperator.Div, ExpressionType.Number, ExpressionType.Number) => ExpressionType.Number,
                
                // Float operations
                (BinaryOperator.Add, ExpressionType.Float, ExpressionType.Float) => ExpressionType.Float,
                (BinaryOperator.Sub, ExpressionType.Float, ExpressionType.Float) => ExpressionType.Float,
                (BinaryOperator.Mul, ExpressionType.Float, ExpressionType.Float) => ExpressionType.Float,
                (BinaryOperator.Div, ExpressionType.Float, ExpressionType.Float) => ExpressionType.Float,
                
                _ => ExpressionType.Indetermined
            };
        }
    }

    public enum BinaryOperator
    {
        #region Assignment

        Assign,
        AddAssign,
        SubAssign,
        MulAssign,
        DivAssign,
        ModAssign,
        AndAssign,
        XorAssign,
        OrAssign,

        #endregion Assignment

        #region Logical

        LogicalOr,
        LogicalAnd,

        #endregion Logical

        #region Equality

        Equal,
        NotEqual,

        #endregion Equality

        #region Relational

        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,

        #endregion Relational

        #region Bitwise

        BitwiseAnd,
        BitwiseOr,
        BitwiseXor,

        #endregion Bitwise

        #region Shift

        LeftShift,
        RightShift,

        #endregion Shift

        #region Additive

        Add,
        Sub,

        #endregion Additive

        #region Multiplicative

        Mul,
        Div,
        Mod,

        #endregion Multiplicative
    }
}
