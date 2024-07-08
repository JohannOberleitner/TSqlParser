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
        string _string;
        double? _value;

        public FloatLiteral(string s)
        {
            _string = s;
            double l;
            if (double.TryParse(s, out l))
            {
                _value = l;
            }
        }

        public double? Number => _value;

        public override string Token => _string;

        public override string ToString()
        {
            return $"FLOAT {_string}";
        }
    }
}
