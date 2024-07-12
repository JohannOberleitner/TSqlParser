using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.InsertDataStructures
{
    public class OutputClause
    {
        /* private readonly IList<Expression> _columnNames; */
        private readonly string _columnNameRepresentation;
        private readonly Identifier? _targetTableName;
        /* private readonly IList<Identifier>? _targetTableColumns;*/
        private readonly string? _targetTableColumnNameRepresentation;

        public OutputClause(IList<Expression> columnNames)
        {
            // _columnNames = columnNames;
            _columnNameRepresentation = MakeColumnNameRepresentation(columnNames);
        }

        public OutputClause(IList<Expression> columnNames, Identifier targetTableName, IList<Identifier> targetTableColumns)
        {
            // _columnNames = columnNames;
            _columnNameRepresentation = MakeColumnNameRepresentation(columnNames);
            _targetTableName = targetTableName;
            _targetTableColumnNameRepresentation = MakeColumnNameRepresentation(targetTableColumns);
        }

        private static string MakeColumnNameRepresentation(IList<Expression> columns)
        {
            var sb = new StringBuilder();
            if (columns.Count > 0)
                sb.Append(columns[0]);
            for (int i = 1; i < columns.Count; ++i)
            {
                sb.Append(", ");
                sb.Append(columns[i]);
            }
            sb.AppendLine();
            return sb.ToString();
        }

        private static string MakeColumnNameRepresentation(IList<Identifier> columns)
        {
            var sb = new StringBuilder();
            if (columns.Count > 0)
                sb.Append(columns[0].Name);
            for (int i = 1; i < columns.Count; ++i)
            {
                sb.Append(", ");
                sb.Append(columns[i].Name);
            }
            sb.AppendLine();
            return sb.ToString();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("OUTPUT");
            sb.AppendLine(_columnNameRepresentation);
            if (_targetTableName != null)
            {
                sb.Append("INTO ");
                sb.Append(_targetTableName);
                sb.Append('(');
                sb.AppendLine(_targetTableColumnNameRepresentation);
                sb.Append(')');
            }
            return sb.ToString();

        }

    }
}
