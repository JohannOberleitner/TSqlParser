﻿namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a TSQL Keyword.
    /// </summary>
    public class TSqlKeyword : Lexem
    {
        private readonly string _keyword;

        /// <summary>
        /// Initializes a TSqlKeyword lexem instance with the keyword as parameter.
        /// </summary>
        /// <param name="keyword"></param>
        public TSqlKeyword(string keyword)
        {
            _keyword = keyword;
        }

        /// <summary>
        /// Returns the keyword that used in the constructor for initialization.
        /// </summary>
        public string Keyword => _keyword;

        /// <summary>
        /// Checks if this lexem is a Keyword and equals one of the provided
        /// strings.
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public override bool KeywordEquals(params string[] strings)
        {
            foreach (var s in strings)
            {
                if (Keyword.Equals(s, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns as token the keyword that was provided in the constructor.
        /// </summary>
        public override string Token => Keyword;


        /// <summary>
        /// Returns Keyword and the keyword in uppercase.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Keyword {_keyword.ToUpperInvariant()}";
        }
    }
}
