using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class FunctionCallExpression : Expression
    {
        private readonly Identifier _identifier;
        private readonly IList<ArgumentExpression> _argumentList;
        private readonly string _argumentListRepresentation;

        public FunctionCallExpression(Identifier identifier, IList<ArgumentExpression> argumentList)
        {
            _identifier = identifier;
            _argumentList = argumentList;
            _argumentListRepresentation = MakeArgumentListRepresentation();
        }

        public IList<ArgumentExpression> ArgumentList => _argumentList;

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
        private string MakeArgumentListRepresentation()
        {
            var sb = new StringBuilder();
            if (_argumentList.Count > 0)
                sb.Append(_argumentList[0].ToString());
            for (int i = 1; i < _argumentList.Count; ++i)
            {
                sb.Append(',');
                sb.Append(_argumentList[i].ToString());
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            return $"{_identifier.Name}({_argumentListRepresentation})";
        }
    }
}
