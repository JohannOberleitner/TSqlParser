using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.InsertDataStructures;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class InsertStatement : Statement
    {
        private readonly Identifier _tableName;
        private readonly IList<Identifier> _columnNames;
        private readonly OutputClause? _outputClause;
        private readonly string _columnNameRepresentation;
        private readonly IList<Expression>? _valueExpressions;
        private readonly QueryStatement? _derivedTableQuery;
        private readonly bool? _defaultValues;


        private readonly string? _valueExpressionRepresentation;

        public InsertStatement(Identifier tableName, IList<Identifier> columnNames, OutputClause? outputClause, bool defaultValues)
        {
            _tableName = tableName;
            _columnNames = columnNames;
            _outputClause = outputClause;
            _defaultValues = defaultValues;

            _columnNameRepresentation = MakeColumnNameRepresentation();
        }

        public InsertStatement(Identifier tableName, IList<Identifier> columnNames, OutputClause? outputClause, IList<Expression> valueExpressions)
        {
            _tableName = tableName;
            _columnNames = columnNames;
            _outputClause = outputClause;
            _valueExpressions = valueExpressions;

            _columnNameRepresentation = MakeColumnNameRepresentation();
            _valueExpressionRepresentation = MakeValueExpressionListRepresentation();
        }

        public InsertStatement(Identifier tableName, IList<Identifier> columnNames, OutputClause? outputClause, QueryStatement derivedTableQuery)
        {
            _tableName = tableName;
            _columnNames = columnNames;
            _outputClause = outputClause;

            _columnNameRepresentation = MakeColumnNameRepresentation();
            _derivedTableQuery = derivedTableQuery;
        }

        private string MakeColumnNameRepresentation()
        {
            var sb = new StringBuilder();
            sb.Append('(');
            if (_columnNames.Count > 0)
                sb.Append(_columnNames[0].Name);
            for (int i = 1; i < _columnNames.Count; ++i)
            {
                sb.Append(", ");
                sb.Append(_columnNames[i].Name);
            }
            sb.Append(')');
            sb.AppendLine();
            return sb.ToString();
        }

        private string MakeValueExpressionListRepresentation()
        {
            var sb = new StringBuilder();
            sb.Append("VALUES(");
            if (_valueExpressions!.Count > 0)
                sb.AppendLine(_valueExpressions![0].ToString());
            for (int i = 1; i < _valueExpressions.Count; ++i)
            {
                sb.AppendLine(",");
                sb.Append(_valueExpressions![i].ToString());
            }
            sb.Append(')');
            sb.AppendLine();
            return sb.ToString();
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Identifier TableName => _tableName;
        public QueryStatement? QueryStatement => _derivedTableQuery;
        public IList<Expression>? ValueExpressions => _valueExpressions;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("INSERT INTO");
            sb.Append(_tableName);
            sb.AppendLine();
            sb.Append('(');
            sb.Append(_columnNameRepresentation);
            sb.Append(')');
            sb.AppendLine();
            if (_outputClause != null)
            {
                sb.Append(_outputClause);
                sb.AppendLine();
            }
            if (_defaultValues != null && _defaultValues! == true)
                sb.Append(" DEFAULT VALUES ");
            else if (_valueExpressions != null)
                sb.Append(_valueExpressionRepresentation);
            else if (_derivedTableQuery != null)
                sb.Append(_derivedTableQuery);
            else
                sb.Append("Unknown clause!");
            return sb.ToString();
        }
    }
}
