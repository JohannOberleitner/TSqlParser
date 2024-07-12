using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Globalization;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    /// <summary>
    /// Represents an SQL If Statement.
    /// </summary>
    public class IfStatement : Statement
    {
        private readonly Expression _condition;
        private readonly Statement _thenClause;
        private readonly Statement? _elseClause;

        public IfStatement(Expression condition, Statement thenClause)
        {
            _condition = condition;
            _thenClause = thenClause;
        }
        public IfStatement(Expression condition, Statement thenClause, Statement elseClause)
        {
            _condition = condition;
            _thenClause = thenClause;
            _elseClause = elseClause;
        }

        public Expression Condition => _condition;

        public Statement ThenClause => _thenClause;
        public Statement? ElseClause => _elseClause;

        public bool HasElseClause => _elseClause != null;

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(CultureInfo.InvariantCulture, $"IF {_condition} ");
            sb.AppendLine(CultureInfo.InvariantCulture, $"{_thenClause}");

            if (HasElseClause)
            {
                sb.AppendLine("ELSE");
                sb.AppendLine(CultureInfo.InvariantCulture, $"{_elseClause}");
            }
            return sb.ToString();
        }
    }
}
