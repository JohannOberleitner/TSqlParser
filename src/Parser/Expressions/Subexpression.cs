using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class SubExpression : Expression
    {
        private readonly Expression _inner;
        public SubExpression(Expression inner)
        {
            _inner = inner;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Expression Inner => _inner;
        public override string ToString()
        {
            return $"({_inner})";
        }
    }
}
