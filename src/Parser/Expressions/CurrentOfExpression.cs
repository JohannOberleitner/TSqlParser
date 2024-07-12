using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class CurrentOfExpression : Expression
    {
        private readonly Expression _cursorName;
        public CurrentOfExpression(Expression cursorName)
        {
            _cursorName = cursorName;
        }

        public Expression CursorName => _cursorName;

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"CURSOR OF {_cursorName}";
        }
    }
}
