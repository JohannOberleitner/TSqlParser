namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a literal TSQL integer number. The maximum domain for the
    /// literal value is limited to the values for the .NET long datatype.
    /// </summary>
    public class IntegerLiteral: Lexem
    {
        string _string;
        long? _value;

        public IntegerLiteral(string s)
        {
            _string = s;
            long l;
            if (long.TryParse(s, out l))
            {
                _value = l;
            }
        }

        public long? Number => _value;

        public override string ToString()
        {
            return $"INTEGER {_string}";
        }
    }
}
