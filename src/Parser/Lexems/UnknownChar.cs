namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a character that was not recognized by the Lexer.
    /// </summary>
    public class UnknownChar : Lexem
    {
        private readonly char _c;

        /// <summary>
        /// Creates an instance of Unknown Char with the not identified character.
        /// </summary>
        /// <param name="c"></param>
        public UnknownChar(char c)
        {
            this._c = c;
        }

        /// <summary>
        /// The token is returned as '** Unkwown **' and the character that was used
        /// in the constructor.
        /// </summary>
        public override string Token => "** Unknown **" + _c;

        /// <summary>
        /// Returns a string starting with ' Unknown: ' and the unidentified character.
        /// Afterwards a long list of * is appended.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $" Unknown: {_c} *************************************************";
        }
    }
}
