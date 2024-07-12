using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.SetStatementRules
{
    public class SetStatementMatchValues
    {
        private readonly IList<string> _matchedValues = new List<string>();

        public SetStatementMatchValues()
        {
        }

        public void Add(string matchedValue)
        {
            _matchedValues.Add(matchedValue);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(_matchedValues[0]);
            for (int i = 1; i < _matchedValues.Count; ++i)
            {
                sb.Append(' ');
                sb.Append(_matchedValues[i]);
            }

            return sb.ToString();
        }
    }
}
