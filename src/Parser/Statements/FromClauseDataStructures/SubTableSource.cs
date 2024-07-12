using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures
{
    public class SubTableSource : TableSource
    {
        private readonly TableSource _inner;
        public SubTableSource(TableSource inner)
        {
            _inner = inner;
        }

        public override void Accept(ITableSourceVisitor visitor)
        {
            visitor.Visit(this);
        }

        public TableSource Inner => _inner;

        public override string ToString()
        {
            return $"( {_inner} )";
        }
    }
}
