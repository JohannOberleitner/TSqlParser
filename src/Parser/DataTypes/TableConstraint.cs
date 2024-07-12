using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes
{
    public class TableConstraint : Constraint
    {
        private readonly string _constraintType;
        private readonly IList<string> _conditions;
        private readonly string _conditionRepresentation;
        private readonly string _clusteredType;

        public TableConstraint(string constraintType, string clusteredType, IList<string> conditions)
        {
            _constraintType = constraintType;
            _conditions = conditions;
            _clusteredType = clusteredType;
            _conditionRepresentation = MakeConditionRepresentation();
        }

        private string MakeConditionRepresentation()
        {
            var sb = new StringBuilder();
            sb.Append(_conditions[0]);
            for (int i = 1; i < _conditions.Count; ++i)
            {
                sb.Append(',');
                sb.Append(_conditions[i]);
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            return $"{_constraintType} ${_clusteredType} ({_conditionRepresentation})";
        }
    }
}
