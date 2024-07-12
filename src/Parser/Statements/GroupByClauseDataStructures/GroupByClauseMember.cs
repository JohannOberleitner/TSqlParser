using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.GroupByClauseDataStructures
{
    public class GroupByClauseMember
    {
        private readonly Expression _columnName;

        public GroupByClauseMember(Expression columnName)
        {
            _columnName = columnName;
        }

        public override string ToString()
        {
            return $"{_columnName}";
        }
    }
}
