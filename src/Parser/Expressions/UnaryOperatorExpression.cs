using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class UnaryOperatorExpression : Expression
    {
        private readonly Expression _inner;
        private readonly OperatorSymbol _operator;

        public UnaryOperatorExpression(Expression inner, OperatorSymbol symbol)
        {
            _inner = inner;
            _operator = symbol;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Expression Inner => _inner;

        public override string ToString()
        {
            return $"{_operator.Symbol} {_inner}";
        }
    }
}
