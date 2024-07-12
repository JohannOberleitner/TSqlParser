using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class UnrecognizedStatement : Statement
    {
        private readonly Lexem _lexem;

        public UnrecognizedStatement(Lexem lexem)
        {
            _lexem = lexem;
        }

        public Lexem Lexem => _lexem;

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"Unrecognized {_lexem}";
        }
    }
}
