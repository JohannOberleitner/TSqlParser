using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    /// <summary>
    /// Represents an SQL Update Statement.
    /// </summary>
    public class UpdateStatement : Statement
    {
        private readonly Identifier _tableName;
        private readonly IList<Expression> _valueExpressions;
        private readonly WhereClause? _whereClause;
        private readonly FromClause? _fromClause;

        public UpdateStatement(Identifier tableName, IList<Expression> valueExpressions, FromClause? fromClause, WhereClause? whereClause)
        {
            _tableName = tableName;
            _valueExpressions = valueExpressions;
            _whereClause = whereClause;
            _fromClause = fromClause;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Identifier TableName => _tableName;
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("UPDATE ");
            sb.Append(_tableName);
            sb.Append(" SET ");
            sb.Append(_valueExpressions[0]);
            for (int i = 1; i < _valueExpressions.Count; ++i)
            {
                sb.Append(", ");
                sb.Append(_valueExpressions[i]);
            }
            if (_fromClause != null)
            {
                sb.Append(' ');
                sb.Append(_fromClause);
            }
            if (_whereClause != null)
            {
                sb.AppendLine();
                sb.Append(_whereClause);
            }
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
