using System.Globalization;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class IntegerExpression : Expression
    {
        private readonly IntegerLiteral _integer;

        public IntegerExpression(IntegerLiteral literal)
        {
            _integer = literal;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            return _integer.Number.ToString(CultureInfo.InvariantCulture);
        }
    }
}
