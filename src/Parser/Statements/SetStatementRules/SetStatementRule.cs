using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.SetStatementRules
{
    public class SetStatementRule
    {
        private readonly SetStatementPositionRule[] _positionRules;
        private readonly string _keyword;

        public SetStatementRule(string keyword, params SetStatementPositionRule[] positionRules)
        {
            _keyword = keyword;
            _positionRules = positionRules;
        }

        public bool Matches(IList<Lexem> inputs, ref int pos, out SetStatementMatchValues? matchValues)
        {
            matchValues = null;

            int newPos = pos;
            if (!MatchesKeywords(inputs, ref newPos))
                return false;

            matchValues = new SetStatementMatchValues();

            foreach (var rule in _positionRules)
            {
                if (rule.Matches(inputs, ref newPos, ref matchValues))
                {
                    pos = newPos;
                    return true;
                }
            }

            return false;
        }

        private bool MatchesKeywords(IList<Lexem> inputs, ref int pos)
        {
            var keywords = _keyword.Split(' ');
            int i = 0;

            while (pos < inputs.Count && i < keywords.Length)
            {
                if (!inputs[pos + i].TokenEquals(keywords[i]))
                    return false;
                i++;
            }
            pos += i;
            return true;
        }
        public string Keyword => _keyword;
    }
}
