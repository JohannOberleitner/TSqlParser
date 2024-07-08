namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a TSQL String literal. Distinguishes if unicode format was used.
    /// </summary>
    public class StringLiteral: Lexem
    {
        private string _string;
        private bool _isUnicode;

        public StringLiteral(string s, bool isUnicode)
        {
            _string = s;
            _isUnicode = isUnicode;
        }

        public override string Token
        {
            get {
                    if (_isUnicode)
                        return "N\'"+_string+'\'';
                    else
                    return '\'' + _string + '\'';
            }
        }

        public override string ToString()
        {
            return $"STRING {_string}";
        }

    }
}
