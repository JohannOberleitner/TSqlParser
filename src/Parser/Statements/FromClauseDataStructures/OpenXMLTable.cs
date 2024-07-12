using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures
{
    public class OpenXMLTable : TableSource
    {
        private readonly Expression _functionCall;
        private readonly Identifier? _aliasName;
        private readonly IList<TableHint> _tableHintsAfterAlias;

        public OpenXMLTable(Expression functionCall, Identifier? aliasName, IList<TableHint> tableHintsAfterAlias)
        {
            _functionCall = functionCall;
            _aliasName = aliasName;
            _tableHintsAfterAlias = tableHintsAfterAlias;
        }

        public override void Accept(ITableSourceVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(_functionCall);
            if (_aliasName != null)
            {
                sb.Append(" AS ");
                sb.Append(' ');
                sb.Append(_aliasName);
                Append(sb, _tableHintsAfterAlias);
            }
            return sb.ToString();
        }
    }
}
