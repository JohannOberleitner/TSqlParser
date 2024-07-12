using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    /// <summary>
    /// Represents a FETCH cursor INTO statement.
    /// </summary>
    public class FetchCursorStatement : Statement
    {
        private readonly Identifier _cursorName;
        private readonly IList<Expression> _variables;

        public FetchCursorStatement(Identifier cursorName, IList<Expression> variables)
        {
            _cursorName = cursorName;
            _variables = variables;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("FETCH ");
            sb.Append(_cursorName);
            if (_variables.Count > 0)
            {
                sb.Append(" INTO ");
                sb.Append(_variables[0]);
                for (int i = 1; i < _variables.Count; ++i)
                {
                    sb.Append(", ");
                    sb.Append(_variables[i]);
                }
            }
            return sb.ToString();
        }
    }
}
