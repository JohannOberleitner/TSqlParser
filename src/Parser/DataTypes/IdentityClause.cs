using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes
{
    public class IdentityClause
    {
        private readonly Expression? _seed;
        private readonly Expression? _increment;

        public IdentityClause()
        { }

        public IdentityClause(Expression seed, Expression increment)
        {
            _seed = seed;
            _increment = increment;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("IDENTITY");
            if (_seed != null)
            {
                sb.Append('(');
                sb.Append(_seed);
                sb.Append(", ");
                sb.Append(_increment);
            }

            return sb.ToString();
        }
    }
}
