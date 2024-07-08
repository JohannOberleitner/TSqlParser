using System;
using System.Collections.Generic;
using System.Text;


namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a TSQL Keyword.
    /// </summary>
    public class TSqlKeyword:Lexem
    {
        private string _keyword;

        public TSqlKeyword(string keyword)
        {
            _keyword = keyword;
        }

        public string Keyword => _keyword;

        public override string Token { get { return Keyword; } }

        public override string ToString()
        {
            return $"Keyword {_keyword.ToUpper()}";
        }
    }
}
