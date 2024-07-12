using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class TopClauseExpression
    {
        private readonly Expression _topClause;
        private readonly bool _braces;
        private readonly bool _withTies;

        public TopClauseExpression(Expression topClause, bool braces, bool withTies)
        {
            _topClause = topClause;
            _braces = braces;
            _withTies = withTies;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("TOP ");
            if (_braces)
            {
                sb.Append('(');
                sb.Append(_topClause);
                sb.Append(')');
            }
            else
            {
                sb.Append(_topClause);
            }
            sb.Append(_withTies);
            return sb.ToString();
        }
    }
}
