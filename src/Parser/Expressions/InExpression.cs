using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class InExpression : Expression
    {
        private readonly Expression _subject;
        private readonly IList<Expression> _items;
        private readonly bool _isNot;
        private readonly string _itemsRepresentation;
        public InExpression(Expression subject, IList<Expression> items, bool isNot)
        {
            _subject = subject;
            _items = items;
            _isNot = isNot;
            _itemsRepresentation = MakeItemsRepresentation();
        }

        public Expression Subject => _subject;
        public IList<Expression> Items => _items;

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        private string MakeItemsRepresentation()
        {
            var sb = new StringBuilder();
            sb.Append(_items[0]);
            for (int i = 1; i < _items.Count; ++i)
            {
                sb.Append(", ");
                sb.Append(_items[i]);
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            if (_isNot)
                return $"{_subject} NOT IN ({_itemsRepresentation})";
            else
                return $"{_subject} IN ({_itemsRepresentation})";
        }
    }
}
