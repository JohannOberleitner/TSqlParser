using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes
{
    /// <summary>
    /// Represents the SQL cursor type.
    /// </summary>
    public class CursorType : SqlDataType
    {
        private readonly QueryExpression? _query;

        /// <summary>
        /// Initializes the CursorType with a query expression for
        /// the CURSOR FOR statement.
        /// </summary>
        /// <param name="query"></param>
        public CursorType(QueryExpression query)
        {
            _query = query;
        }

        /// <summary>
        /// Initializes the CursorType with en empty query.
        /// </summary>
        public CursorType()
        {
        }

        /// <summary>
        /// Represents a string that renders this Cursor type.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_query == null)
                return "CURSOR";
            else
                return $"CURSOR FOR {_query}";
        }
    }
}
