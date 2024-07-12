using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.OrderByClauseDataStructures;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    /// <summary>
    /// Provides the ORDER BY clauses that are used in queries.
    /// </summary>
    public class OrderByClause
    {
        private readonly IList<OrderByClauseMember> _orderByClauses;
        private readonly string _orderByClausesRepresentation;

        public OrderByClause(IList<OrderByClauseMember> orderByClauses)
        {
            _orderByClauses = orderByClauses;
            _orderByClausesRepresentation = MakeOrderByClauseMembersRepresentation();
        }

        private string MakeOrderByClauseMembersRepresentation()
        {
            var sb = new StringBuilder();
            if (_orderByClauses.Count > 0)
                sb.Append(_orderByClauses[0].ToString());
            for (int i = 1; i < _orderByClauses.Count; ++i)
            {
                sb.Append(" ,");
                sb.Append(_orderByClauses[i].ToString());
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            return $"ORDER BY {_orderByClausesRepresentation}";
        }
    }
}
