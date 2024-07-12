using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class AlterIndexStatement : Statement
    {
        private readonly Identifier _indexName;
        private readonly Expression _target;

        public AlterIndexStatement(Identifier indexName, Expression target)
        {
            _indexName = indexName;
            _target = target;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("ALTER INDEX ");
            sb.Append(_indexName);
            sb.Append(" ON ");
            sb.Append(_target);


            return sb.ToString();
        }
    }
}
