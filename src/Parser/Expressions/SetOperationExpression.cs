using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class SetOperationExpression : Expression
    {
        private readonly TSqlKeyword _setOperator;
        private readonly QueryStatement _query;

        public SetOperationExpression(TSqlKeyword setOperator, QueryStatement query)
        {
            _setOperator = setOperator;
            _query = query;
        }

        public QueryStatement Query => _query;

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_setOperator.Token);
            sb.Append(_query.ToString());
            return sb.ToString();
        }
    }
}
