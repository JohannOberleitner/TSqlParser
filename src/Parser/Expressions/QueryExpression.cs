using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class QueryExpression : Expression
    {
        private readonly QueryStatement _queryStatement;

        public QueryExpression(QueryStatement queryStatement)
        {
            _queryStatement = queryStatement;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public QueryStatement Query => _queryStatement;

        public override string ToString()
        {
            return _queryStatement.ToString();
        }
    }
}
