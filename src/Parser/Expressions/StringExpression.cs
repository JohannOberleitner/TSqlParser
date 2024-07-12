using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class StringExpression : Expression
    {
        private readonly StringLiteral _s;

        public StringExpression(StringLiteral s)
        {
            _s = s;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            return _s.Token.ToString();
        }
    }
}
