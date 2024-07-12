using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    public class CastFunctionCallExpression : Expression
    {
        private readonly Expression _target;
        private readonly SqlDataType _targetType;

        public CastFunctionCallExpression(Expression target, SqlDataType targetType)
        {
            _target = target;
            _targetType = targetType;
        }

        public Expression Target => _target;

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"CAST({_target} AS {_targetType})";
        }
    }
}
