using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class LikeExpression : Expression
    {
        private readonly Expression _left;
        private readonly Expression _right;
        private readonly bool _isNot;

        public LikeExpression(Expression left, Expression right, bool isNot)
        {
            _left = left;
            _right = right;
            _isNot = isNot;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Expression Left => _left;
        public Expression Right => _right;

        public override string ToString()
        {
            if (_isNot)
                return $"{_left} NOT LIKE {_right}";
            else
                return $"{_left} LIKE {_right}";
        }

    }
}
