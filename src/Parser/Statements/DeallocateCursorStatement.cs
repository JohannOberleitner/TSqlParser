using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class DeallocateCursorStatement : Statement
    {
        private readonly Identifier _cursorName;

        public DeallocateCursorStatement(Identifier cursorName)
        {
            _cursorName = cursorName;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"DEALLOCATE {_cursorName}";
        }
    }
}
