using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public abstract class Statement
    {
        public abstract void Accept(IVisitor visitor);
    }
}
