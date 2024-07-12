using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class DropTableStatement : Statement
    {
        private readonly Identifier _tableName;

        public DropTableStatement(Identifier tableName)
        {
            _tableName = tableName;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            return $"DROP {_tableName}";
        }
    }
}
