using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    /// <summary>
    /// Represents a GO statement.
    /// </summary>
    public class GoStatement : Statement
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Just returns the string GO as representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"GO";
        }
    }
}
