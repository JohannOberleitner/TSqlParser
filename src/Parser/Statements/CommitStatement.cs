using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class CommitStatement : Statement
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            return "COMMIT";
        }
    }
}
