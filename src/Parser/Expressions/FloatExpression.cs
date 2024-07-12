using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class FloatExpression : Expression
    {
        private readonly FloatLiteral _floatLiteral;

        public FloatExpression(FloatLiteral floatLiteral)
        {
            _floatLiteral = floatLiteral;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            return $"{_floatLiteral.Token}";
        }
    }
}
