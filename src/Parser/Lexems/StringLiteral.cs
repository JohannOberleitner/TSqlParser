namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a TSQL String literal. Distinguishes if unicode format was used.
    /// </summary>
    public class StringLiteral : Lexem
    {
        private readonly string _string;
        private readonly bool _isUnicode;

        /// <summary>
        /// Initializes a TSQL string literator
        /// </summary>
        /// <param name="s">The string that is used for initialization.</param>
        /// <param name="isUnicode">true if the string was prefixed as unicode string.</param>
        public StringLiteral(string s, bool isUnicode)
        {
            _string = s;
            _isUnicode = isUnicode;
        }

        /// <summary>
        /// Returns the string either in single quotes for non-unicode strings
        /// or in single quotes with an N prefix for unicode strings.
        /// </summary>
        public override string Token => _isUnicode ? $"N'{_string}'" : $"'{_string}'";

        /// <summary>
        /// Returns STRING and the string literator value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"STRING {_string}";
        }

    }
}
