namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes
{
    public class ColumnConstraint
    {
        private readonly string _constraintType;
        //private IList<string> _conditions;
        //private string _conditionRepresentation;

        public ColumnConstraint(string constraintType)
        {
            _constraintType = constraintType;
            //_conditions = conditions;
            //_conditionRepresentation = MakeConditionRepresentation();
        }

        /*private string MakeConditionRepresentation()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_conditions[0]);
            for (int i = 1; i < _conditions.Count; ++i)
            {
                sb.Append(',');
                sb.Append(_conditions[i]);
            }
            return sb.ToString();
        }*/

        public override string ToString()
        {
            return $"{_constraintType}";
        }
    }
}
