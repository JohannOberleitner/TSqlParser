using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Globalization;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class WhileStatement : Statement
    {
        private readonly Expression _condition;
        private readonly Statement _codeBlock;

        public WhileStatement(Expression condition, Statement codeBlock)
        {
            _condition = condition;
            _codeBlock = codeBlock;
        }

        public Expression Condition => _condition;
        public Statement CodeBlock => _codeBlock;

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(CultureInfo.InvariantCulture, $"WHILE {_condition} ");
            sb.AppendLine(CultureInfo.InvariantCulture, $"{_codeBlock}");
            return sb.ToString();
        }
    }
}
