using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class PostfixUnaryOperatorExpression : Expression
    {
        private readonly Expression _inner;
        private readonly Identifier _identifier;
        private readonly bool _isNot;

        public PostfixUnaryOperatorExpression(Expression inner, Identifier identifier, bool isNot)
        {
            _inner = inner;
            _identifier = identifier;
            _isNot = isNot;
        }

        public Expression Inner => _inner;

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            if (_isNot)
                return $"{_inner} IS NOT {_identifier.Name}";
            else
                return $"{_inner} IS {_identifier.Name}";
        }
    }
}
