namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a literal TSQL integer number. The maximum domain for the
    /// literal value is limited to the values for the .NET long datatype.
    /// </summary>
    public class IntegerLiteral : Lexem
    {
        private readonly string _string;
        private readonly long? _value;

        /// <summary>
        /// Creates an instance of an IntegerLiteral.
        /// </summary>
        /// <param name="s">Initialization value that is parsed as long.</param>
        public IntegerLiteral(string s)
        {
            _string = s;
            if (long.TryParse(s, out long l))
            {
                _value = l;
            }
        }

        /// <summary>
        /// Returns the parsed long value from the constructor.
        /// </summary>
        public long? Number => _value;

        /// <summary>
        /// Return INTEGER and the value that was provided to the constructor.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"INTEGER {_string}";
        }
    }
}
