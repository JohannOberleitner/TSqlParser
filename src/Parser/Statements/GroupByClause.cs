using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.GroupByClauseDataStructures;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class GroupByClause
    {
        private readonly IList<GroupByClauseMember> _groupByClauses;
        private readonly string _groupByClausesRepresentation;

        public GroupByClause(IList<GroupByClauseMember> groupByClauses)
        {
            _groupByClauses = groupByClauses;
            _groupByClausesRepresentation = MakeGroupByClauseMembersRepresentation();
        }

        private string MakeGroupByClauseMembersRepresentation()
        {
            var sb = new StringBuilder();
            if (_groupByClauses.Count > 0)
                sb.Append(_groupByClauses[0].ToString());
            for (int i = 1; i < _groupByClauses.Count; ++i)
            {
                sb.Append(" ,");
                sb.Append(_groupByClauses[i].ToString());
            }
            return sb.ToString();

        }

        public override string ToString()
        {
            return $"ORDER BY {_groupByClausesRepresentation}";
        }
    }
}
