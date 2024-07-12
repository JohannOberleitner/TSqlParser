using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class CreateStatisticsStatement : Statement
    {
        private readonly Identifier _statisticsName;
        private readonly Expression _target;

        public CreateStatisticsStatement(Identifier statisticsName, Expression target)
        {
            _statisticsName = statisticsName;
            _target = target;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("CREATE STATISTICS ");
            sb.Append(_statisticsName);
            sb.Append(" ON ");
            sb.Append(_target);
            return sb.ToString();
        }
    }
}
