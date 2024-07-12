using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class IdentifierExpression : Expression
    {
        private readonly Identifier _identifier;

        public IdentifierExpression(Identifier identifier)
        {
            _identifier = identifier;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Identifier Identifier => _identifier;

        public override string ToString()
        {
            return _identifier.Name;
        }
    }
}
