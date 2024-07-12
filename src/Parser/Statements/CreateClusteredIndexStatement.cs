using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class CreateIndexStatement : Statement
    {
        private readonly Identifier _indexName;
        private readonly bool _unique;
        private readonly string _clusteredType;
        private readonly Expression _onExpression;

        public CreateIndexStatement(Identifier indexName, bool unique, string clusteredType, Expression onExpression)
        {
            _indexName = indexName;
            _unique = unique;
            _clusteredType = clusteredType;
            _onExpression = onExpression;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("CREATE ");
            if (_unique)
            {
                sb.Append("UNIQUE ");
            }
            sb.Append(_clusteredType);
            sb.Append(' ');
            sb.Append(_indexName);
            sb.Append(" ON ");
            sb.Append(_onExpression);
            return sb.ToString();
        }
    }
}
