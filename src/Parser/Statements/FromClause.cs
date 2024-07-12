using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    /// <summary>
    /// Represents the SQL From clause.
    /// </summary>
    public class FromClause
    {
        private readonly IList<TableSource> _tableSources;
        private readonly string _tableSourcesRepresentation;

        /// <summary>
        /// Initializes the FromClause with a list of tables that is occurring in the clause.
        /// </summary>
        /// <param name="tableSources"></param>
        public FromClause(IList<TableSource> tableSources)
        {
            _tableSources = tableSources;
            _tableSourcesRepresentation = MakeTableSourcesRepresentation();
        }

        private string MakeTableSourcesRepresentation()
        {
            var sb = new StringBuilder();
            sb.Append(_tableSources[0]);
            for (int i = 1; i < _tableSources.Count; ++i)
            {
                sb.Append(',');
                sb.Append(_tableSources[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the table sources that were provided in the constructor.
        /// </summary>
        public IList<TableSource> TableSources => _tableSources;

        /// <summary>
        /// Renders the FromClause by providing FROM and a list of the tables that
        /// were used in the constructor.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"FROM {_tableSourcesRepresentation}";
        }
    }
}
