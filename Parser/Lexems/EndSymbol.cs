using System;
using System.Collections.Generic;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents the end of a sequence of lexems.
    /// This artificial symbol is added by the Lexer to signal the parser
    /// that this is the last symbol in a sequence of lexems/tokens.
    /// </summary>
    /// <remarks>This is not the same as the END symbol in TSQL </remarks>
    public class EndSymbol: Lexem
    {

        public override string ToString()
        {
            return "END";
        }
    }
}
