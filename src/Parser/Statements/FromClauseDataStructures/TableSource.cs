using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures
{
    public abstract class TableSource
    {
        public TableSource()
        {
        }

        protected static string MakeTableHintRepresentation(IList<TableHint> tableHints)
        {
            var sb = new StringBuilder();
            if (tableHints.Count > 0)
            {
                sb.Append(tableHints[0]);
                for (int i = 1; i < tableHints.Count; ++i)
                {
                    sb.Append(", ");
                    sb.Append(tableHints[1]);
                }
            }
            return sb.ToString();
        }

        protected static void Append(StringBuilder sb, IList<TableHint> tableHints)
        {
            if (tableHints.Count > 0)
            {
                sb.Append(" WITH(");
                sb.Append(MakeTableHintRepresentation(tableHints));
                sb.Append(')');
            }
        }

        public abstract void Accept(ITableSourceVisitor visitor);
    }
}
