using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.SelectClauseDataStructures;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class QueryStatement : Statement
    {
        private readonly TopClauseExpression? _topClause;
        private readonly IList<ColumnDescriptor> _columnExpressionList;
        private readonly FromClause? _fromClause;
        private readonly WhereClause? _whereClause;
        private readonly GroupByClause? _groupByClause;
        private readonly OrderByClause? _orderByClause;
        private readonly SetOperationExpression? _setOperationExpression;

        private readonly string _columnExpressionListRepresentation;

        public QueryStatement(TopClauseExpression? topClause, IList<ColumnDescriptor> columnExpressionList, FromClause? fromClause, WhereClause? whereClause, GroupByClause? groupByClause, OrderByClause? orderByClause, SetOperationExpression? setOperationExpression)
        {
            _topClause = topClause;
            _columnExpressionList = columnExpressionList;
            _fromClause = fromClause;
            _whereClause = whereClause;
            _groupByClause = groupByClause;
            _orderByClause = orderByClause;
            _columnExpressionListRepresentation = MakeColumnExpressionListRepresentation();
            _setOperationExpression = setOperationExpression;
        }

        private string MakeColumnExpressionListRepresentation()
        {
            var sb = new StringBuilder();
            sb.Append(_columnExpressionList[0].ToString());
            for (int i = 1; i < _columnExpressionList.Count; ++i)
            {
                sb.AppendLine(", ");
                sb.Append(_columnExpressionList[i].ToString());
            }
            sb.AppendLine();
            return sb.ToString();
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public IList<ColumnDescriptor> ColumnExpressionList => _columnExpressionList;

        public FromClause? FromClause => _fromClause;
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("SELECT ");
            if (_topClause != null)
                sb.Append(_topClause.ToString());
            sb.Append(' ');
            sb.Append(_columnExpressionListRepresentation);
            if (_fromClause != null)
            {
                sb.Append(' ');
                sb.Append(_fromClause.ToString());
            }
            if (_whereClause != null)
            {
                sb.Append(' ');
                sb.Append(_whereClause.ToString());
            }
            if (_groupByClause != null)
            {
                sb.Append(' ');
                sb.Append(_groupByClause.ToString());
            }
            if (_orderByClause != null)
            {
                sb.Append(' ');
                sb.Append(_orderByClause.ToString());
            }
            if (_setOperationExpression != null)
            {
                sb.Append(' ');
                sb.Append(_setOperationExpression.ToString());
            }

            return sb.ToString();
        }

    }
}
