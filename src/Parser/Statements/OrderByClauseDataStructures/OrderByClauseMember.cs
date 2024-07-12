using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.OrderByClauseDataStructures
{
    public class OrderByClauseMember
    {
        private readonly Expression _columnName;
        private readonly OrderDirection _sortDirection;

        public OrderByClauseMember(Expression columnName, OrderDirection sortDirection)
        {
            _columnName = columnName;
            _sortDirection = sortDirection;
        }

        public override string ToString()
        {
            if (_sortDirection == OrderDirection.None)
                return $"{_columnName}";
            else if (_sortDirection == OrderDirection.Asc)
                return $"{_columnName} ASC";
            else if (_sortDirection == OrderDirection.Desc)
                return $"{_columnName} DESC";
            else
                throw new ParserException($"Invalid value for _sortDirection: {_sortDirection}");

        }
    }
}
