using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    /// <summary>
    /// Represents the EXEC statement to call a stored procedure.
    /// </summary>
    public class CallStoredProcedureStatement : Statement
    {
        private readonly Expression _storedProcedureName;
        private readonly Expression? _resultValue;
        private readonly IList<ArgumentExpression> _arguments;

        public CallStoredProcedureStatement(Expression storedProcedureName, IList<ArgumentExpression> arguments, Expression? resultValue)
        {
            _storedProcedureName = storedProcedureName;
            _arguments = arguments;
            _resultValue = resultValue;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Identifier Name => ConvertExpression(_storedProcedureName);

        private Identifier ConvertExpression(Expression input)
        {
            if (input is IdentifierExpression)
                return ((IdentifierExpression)input).Identifier;
            else if (input is BinaryOperatorExpression && ((BinaryOperatorExpression)input).Operator.Symbol == "=")
                return ConvertExpression(((BinaryOperatorExpression)input).Second);
            else
                return new ExpressionIdentifier(_storedProcedureName);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("EXEC ");
            if (_resultValue != null)
            {
                sb.Append(_resultValue);
                sb.Append(" = ");
            }
            sb.Append(_storedProcedureName);
            sb.Append(' ');
            if (_arguments.Count > 0)
                sb.Append(_arguments[0]);
            for (int i = 1; i < _arguments.Count; ++i)
            {
                sb.Append(", ");
                sb.Append(_arguments[i].ToString());
            }
            return sb.ToString();
        }
    }
}
