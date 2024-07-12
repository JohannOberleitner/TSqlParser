using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class WaitForStatement : Statement
    {
        private readonly Expression _delay;

        public WaitForStatement(Expression delay)
        {
            _delay = delay;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"WAITFOR {_delay}";
        }
    }
}
