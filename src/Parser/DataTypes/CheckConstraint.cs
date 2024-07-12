using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes
{
    public class CheckConstraint : Constraint
    {
        private readonly Expression _condition;
        public CheckConstraint(Expression condition)
        {
            _condition = condition;
        }

        public override string ToString()
        {
            return $"CHECK({_condition})";
        }
    }
}
