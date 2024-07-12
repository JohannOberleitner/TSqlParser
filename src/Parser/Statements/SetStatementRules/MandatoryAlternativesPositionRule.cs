using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.SetStatementRules
{
    /// <summary>
    /// 
    /// </summary>
    public class MandatoryAlternativesPositionRule : SetStatementPositionRule
    {
        private readonly string[] _alternatives;

        public MandatoryAlternativesPositionRule(params string[] alternatives)
        {
            _alternatives = alternatives;
        }

        public override bool Matches(IList<Lexem> inputs, ref int pos, ref SetStatementMatchValues matchValues)
        {
            foreach (var alternative in _alternatives)
            {
                if (MatchAllParts(alternative, inputs, ref pos, ref matchValues))
                    return true;
            }
            return false;
        }

        private static bool MatchAllParts(string alternative, IList<Lexem> inputs, ref int pos, ref SetStatementMatchValues matchValues)
        {
            var parts = alternative.Split(' ');
            int i = 0;
            while (pos + i < inputs.Count && i < parts.Length)
            {
                if (!inputs[pos + i].TokenEquals(parts[i]))
                    return false;
                i++;
            }
            pos += i;
            matchValues.Add(alternative);
            return true;
        }


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(_alternatives[0]);
            for (int i = 1; i < _alternatives.Length; ++i)
            {
                sb.Append('|');
                sb.Append(_alternatives[i]);
            }

            return sb.ToString();
        }

    }
}
