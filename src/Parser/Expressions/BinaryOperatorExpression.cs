using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class BinaryOperatorExpression : Expression
    {
        private readonly Expression _first;
        private readonly Expression _second;
        private readonly OperatorSymbol _operator;

        public BinaryOperatorExpression(Expression first, Expression second, OperatorSymbol symbol)
        {
            _first = first;
            _second = second;
            _operator = symbol;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public OperatorSymbol Operator => _operator;

        public Expression First => _first;
        public Expression Second => _second;

        public override string ToString()
        {
            return $"{_first} {_operator.Symbol} {_second}";
        }
    }
}
