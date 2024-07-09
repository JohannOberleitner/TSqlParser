namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a float literal.
    /// </summary>
    /// <remarks>
    /// There might be additional representations in TSQL that are currently not
    /// supported.
    /// </remarks>
    public class FloatLiteral : Lexem
    {
        private readonly string _string;
        private readonly double? _value;

        /// <summary>
        /// Creates an instance of a FloatLiteral.
        /// </summary>
        /// <param name="s">Initialization value that is parsed as double.</param>
        public FloatLiteral(string s)
        {
            _string = s;
            if (double.TryParse(s, out double l))
            {
                _value = l;
            }
        }

        /// <summary>
        /// Returns the parsed double value from the constructor.
        /// </summary>
        public double? Number => _value;

        /// <summary>
        /// Returns as token the value that was provided to the construcot.
        /// </summary>
        public override string Token => _string;

        /// <summary>
        /// Return FLOAT and the value that was provided to the constructor.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"FLOAT {_string}";
        }
    }
}
