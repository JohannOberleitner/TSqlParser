using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures
{
    /// <summary>
    /// Represents an TSQL CROSS Join.
    /// </summary>
    public class CrossJoin : Join
    {
        private readonly TableSource _right;

        public CrossJoin(TableSource right)
        {
            _right = right;
        }

        public override void Accept(IJoinVisitor visitor)
        {
            visitor.Visit(this);
        }

        public TableSource Right => _right;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("CROSS JOIN ");
            sb.Append(_right);
            return sb.ToString();
        }
    }
}
