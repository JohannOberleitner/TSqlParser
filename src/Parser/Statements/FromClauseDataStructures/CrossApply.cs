using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures
{
    /// <summary>
    /// Represents CROSS APPLY
    /// </summary>
    public class CrossApply : Join
    {
        private readonly TableSource _right;

        public CrossApply(TableSource right)
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
            sb.Append("CROSS APPLY ");
            sb.Append(_right);
            return sb.ToString();
        }
    }
}
