using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes
{
    public class DefaultValueClause
    {
        private readonly Expression _constantValue;

        public DefaultValueClause(Expression constantValue)
        {
            _constantValue = constantValue;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("DEFAULT ");
            sb.Append(_constantValue);

            return sb.ToString();
        }
    }
}
