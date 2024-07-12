using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class AlterTableStatement : Statement
    {
        private readonly Identifier _tableName;
        private readonly string _checkOptions;

        public AlterTableStatement(Identifier tableName, string checkOptions)
        {
            _tableName = tableName;
            _checkOptions = checkOptions;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("ALTER TABLE ");
            sb.Append(_tableName);
            sb.Append(' ');
            sb.Append(_checkOptions);
            return sb.ToString();
        }

    }
}
