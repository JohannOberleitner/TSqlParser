using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Globalization;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class ReturnStatement : Statement
    {
        private readonly Expression? _returnValueExpression;

        public ReturnStatement(Expression? returnValueExpression)
        {
            _returnValueExpression = returnValueExpression;
        }

        public Expression? ReturnValue => _returnValueExpression;

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (_returnValueExpression != null)
                sb.AppendLine(CultureInfo.InvariantCulture, $"RETURN {_returnValueExpression.ToString()}");
            else
                sb.AppendLine("RETURN");
            return sb.ToString();
        }
    }
}
