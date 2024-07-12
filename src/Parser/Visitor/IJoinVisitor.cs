using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor
{
    public interface IJoinVisitor
    {
        void Visit(InnerJoin innerJoin);
        void Visit(OuterJoin outerJoin);

        void Visit(CrossApply crossApply);
        void Visit(CrossJoin crossJoin);
    }
}
