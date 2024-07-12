using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures
{
    public class SimpleTable : TableSource
    {
        private readonly Expression _tableName;
        private readonly IList<TableHint> _tableHints;
        private readonly Identifier? _aliasName;
        private readonly IList<TableHint> _tableHintsAfterAlias;
        public SimpleTable(Expression tableName, IList<TableHint> tableHints, Identifier? aliasName, IList<TableHint> tableHintsAfterAlias)
        {
            _tableName = tableName;
            _tableHints = tableHints;
            _aliasName = aliasName;
            _tableHints = tableHints;
            _tableHintsAfterAlias = tableHintsAfterAlias;
        }

        public override void Accept(ITableSourceVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Expression TableName => _tableName;


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(_tableName);
            Append(sb, _tableHints);
            if (_aliasName != null)
            {
                sb.Append(" AS ");
                sb.Append(_aliasName);
                sb.Append(' ');
                Append(sb, _tableHintsAfterAlias);
            }
            return sb.ToString();
        }
    }
}
