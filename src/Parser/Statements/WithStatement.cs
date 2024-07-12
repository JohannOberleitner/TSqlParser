using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class WithStatement : Statement
    {
        private readonly Identifier _cteName;
        private readonly QueryStatement _query;
        public WithStatement(Identifier cteName, QueryStatement query)
        {
            _cteName = cteName;
            _query = query;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("WITH ");
            sb.Append(_cteName);
            sb.Append(' ');
            sb.Append(_query);
            return sb.ToString();
        }
    }
}
