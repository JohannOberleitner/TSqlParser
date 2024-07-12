using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures
{
    public class DerivedTable : TableSource
    {
        private readonly QueryExpression _subQuery;
        private readonly Identifier? _aliasName;
        private readonly IList<TableHint> _tableHintsAfterAlias;

        public DerivedTable(QueryExpression subQuery, Identifier? aliasName, IList<TableHint> tableHintsAfterAlias)
        {
            _subQuery = subQuery;
            _aliasName = aliasName;
            _tableHintsAfterAlias = tableHintsAfterAlias;
        }

        public override void Accept(ITableSourceVisitor visitor)
        {
            visitor.Visit(this);
        }


        public QueryExpression Query => _subQuery;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('(');
            sb.Append(_subQuery);
            sb.Append(')');
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
