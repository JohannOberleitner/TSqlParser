namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a character that was not recognized by the Lexer.
    /// </summary>
    public class UnknownChar:Lexem
    {
        char _c;

        public UnknownChar(char c)
        {
            this._c = c;
        }

        public override string Token { get { return "** Unknown **" + _c; } }

        public override string ToString()
        {
            return $" Unknown: {_c} *************************************************";
        }
    }
}
