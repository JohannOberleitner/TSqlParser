using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.SetStatementRules;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    /// <summary>
    /// Represents the SQL SET statement 
    /// </summary>
    public class SetStatement : Statement
    {
        private readonly SetStatementRule _rule;
        private readonly SetStatementMatchValues _matchValues;

        /// <summary>
        /// Initializes the Set Statement.
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="matchValues"></param>
        public SetStatement(SetStatementRule rule, SetStatementMatchValues matchValues)
        {
            _rule = rule;
            _matchValues = matchValues;
        }

        public SetStatementRule Rule => _rule;
        public SetStatementMatchValues MatchValues => _matchValues;


        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            return $"SET {_rule.Keyword} {_matchValues}";
        }
    }
}
