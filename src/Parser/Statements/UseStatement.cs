using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class UseStatement : Statement
    {
        private readonly Identifier _schema;

        public UseStatement(Identifier schema)
        {
            _schema = schema;
        }

        public string DatabaseName => _schema.Name;

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"USE {_schema.Name}";
        }
    }
}
