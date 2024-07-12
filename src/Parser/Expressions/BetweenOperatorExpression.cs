using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class BetweenOperatorExpression : Expression
    {
        private readonly Expression _target;
        private readonly Expression _lowerBoundary;
        private readonly Expression _upperBoundary;
        public BetweenOperatorExpression(Expression target, Expression lowerBoundary, Expression upperBoundary)
        {
            _target = target;
            _lowerBoundary = lowerBoundary;
            _upperBoundary = upperBoundary;
        }

        public Expression Target => _target;
        public Expression LowerBoundary => _lowerBoundary;
        public Expression UpperBoundary => _upperBoundary;

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(_target);
            sb.Append(" BETWEEN ");
            sb.Append(_lowerBoundary);
            sb.Append(" AND ");
            sb.Append(_upperBoundary);

            return sb.ToString();
        }
    }
}
