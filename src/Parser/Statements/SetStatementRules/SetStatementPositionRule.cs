using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.SetStatementRules
{
    public abstract class SetStatementPositionRule
    {
        public abstract bool Matches(IList<Lexem> inputs, ref int pos, ref SetStatementMatchValues matchValues);
    }
}
