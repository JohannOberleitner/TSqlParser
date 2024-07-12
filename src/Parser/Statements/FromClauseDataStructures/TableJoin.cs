using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures
{
    public class TableJoin : TableSource
    {
        private readonly TableSource _first;
        private readonly IList<Join> _joins;

        public TableJoin(TableSource first, IList<Join> joins)
        {
            _first = first;
            _joins = joins;
        }

        public override void Accept(ITableSourceVisitor visitor)
        {
            visitor.Visit(this);
        }

        public TableSource First => _first;
        public IList<Join> Joins => _joins;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(_first);
            foreach (var join in _joins)
            {
                sb.Append(' ');
                sb.Append(join);
            }
            return sb.ToString();
        }
    }
}
