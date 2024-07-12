using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class WhereClause
    {
        private readonly Expression _searchCondition;

        public WhereClause(Expression searchCondition)
        {
            _searchCondition = searchCondition;
        }

        public override string ToString()
        {
            return $"WHERE {_searchCondition}";
        }
    }
}
