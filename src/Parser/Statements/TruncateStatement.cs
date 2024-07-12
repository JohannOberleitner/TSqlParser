using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class TruncateStatement : Statement
    {
        private readonly Identifier _tableName;
        public TruncateStatement(Identifier tableName)
        {
            _tableName = tableName;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("TRUNCATE ");
            sb.Append(_tableName);
            return sb.ToString();
        }
    }
}
