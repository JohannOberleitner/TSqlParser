using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions.CaseClauseDataStructures;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    /// <summary>
    /// Represents an SQL CASE expression.
    /// </summary>
    public class CaseExpression : Expression
    {
        private readonly IList<CaseClause> _clauses;
        private readonly Expression? _elseClause;

        public CaseExpression(IList<CaseClause> clauses, Expression? elseClause)
        {
            _clauses = clauses;
            _elseClause = elseClause;
        }

        public IList<CaseClause> CaseClauses => _clauses;
        public Expression? ElseClause => _elseClause;

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("CASE");
            foreach (var clause in _clauses)
            {
                sb.Append("  ");
                sb.AppendLine(clause.ToString());
            }
            if (_elseClause != null)
            {
                sb.Append("  ");
                sb.Append("ELSE ");
                sb.AppendLine(_elseClause.ToString());
            }

            sb.AppendLine("END");
            return sb.ToString();
        }
    }
}
