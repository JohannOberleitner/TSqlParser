using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class CursorExpression : Expression
    {
        private readonly Expression _query;
        public CursorExpression(Expression query)
        {
            _query = query;
        }

        public Expression Query => _query;

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            return $"CURSOR FOR {_query}";
        }
    }
}
