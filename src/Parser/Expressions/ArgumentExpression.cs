using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class ArgumentExpression
    {
        private readonly Expression _expression;
        private readonly bool _isOutput;

        public ArgumentExpression(Expression expression, bool isOutput)
        {
            _expression = expression;
            _isOutput = isOutput;
        }

        public Expression Expression => _expression;

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            if (_isOutput)
                return $"{_expression} OUTPUT";
            else
                return _expression.ToString();
        }
    }
}
