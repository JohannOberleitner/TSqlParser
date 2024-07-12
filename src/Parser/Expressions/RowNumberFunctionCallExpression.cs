using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class RowNumberFunctionCallExpression : Expression
    {
        private readonly IList<Expression> _partitionByValueExpresions;
        private readonly OrderByClause _orderByClause;

        public RowNumberFunctionCallExpression(IList<Expression> partitionByValueExpresions, OrderByClause orderByClause)
        {
            _partitionByValueExpresions = partitionByValueExpresions;
            _orderByClause = orderByClause;
        }

        public IList<Expression> PartitionByValueExpressions => _partitionByValueExpresions;

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("ROW_NUMBER() OVER");
            sb.Append('(');
            if (_partitionByValueExpresions.Count > 0)
            {
                sb.Append("PARTITION BY ");
                sb.Append(_partitionByValueExpresions[0]);
                for (int i = 1; i < _partitionByValueExpresions.Count; ++i)
                {
                    sb.Append(", ");
                    sb.Append(_partitionByValueExpresions[i]);
                }
            }
            sb.Append(' ');
            sb.Append(_orderByClause);
            sb.AppendLine(")");
            return sb.ToString();
        }
    }
}
