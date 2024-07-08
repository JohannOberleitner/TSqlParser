namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a TSQL operator.
    /// Among those could be:
    /// <list>
    /// <item>Multiplicative: * (multiplication),/ (division),% (remainder)</item>
    /// <item>Additive: + (addition),- (subtraction), & (concatenation)</item>
    /// <item>Comparison: = (equal),< (less than), > (greater than)</item>
    /// <item>Assignment: =,+=, -=</item>
    /// <item>Scope resolution: :: </item>
    /// <item>Logical: AND, OR, NOT </item>
    /// <item>SQL: IS, IN, LIKE </item>
    /// </list>
    /// </summary>
    public class OperatorSymbol: Lexem
    {
        private string _symbol;

        public OperatorSymbol(string c)
        {
            _symbol = c;
        }

        public string Symbol => _symbol;

        public bool IsMultiplicationSymbol
        {
            get { return _symbol == "*" || _symbol == "/" || _symbol == "%"; }
        }

        public bool IsAdditionSymbol
        {
            get { return _symbol == "+" || _symbol == "-" || _symbol == "&" || _symbol == "^" || _symbol == "|" || _symbol == "&"; }            
        }

        public bool IsComparisonSymbol
        {
            get {
                return _symbol == "=" || _symbol == ">" || _symbol == "<" || _symbol == ">=" || _symbol == "<=" || _symbol == "<>" || _symbol == "!>" || _symbol == "!<" || _symbol == "!=";
            }
        }

        public bool IsAssignmentSymbol
        {
            get
            {
                return _symbol == "=" || _symbol == "+=" || _symbol == "-=" || _symbol == "*=" || _symbol == "/=" || _symbol == "%=" || _symbol == "&=" || _symbol == "|=" || _symbol == "^=";
            }
        }

        public bool IsScopeResolutionSymbol
        {
            get
            {
                return _symbol == "::";
            }
        }

        public bool IsAnd => _symbol.ToUpper() == "AND";
        public bool IsOr => _symbol.ToUpper() == "OR";
        public bool IsNot => _symbol.ToUpper() == "NOT";
        public bool IsIs => _symbol.ToUpper() == "IS";
        public bool IsIn => _symbol.ToUpper() == "IN";
        public bool IsLike => _symbol.ToUpper() == "LIKE";

        public override string ToString()
        {
            return _symbol.ToString();
        }
    }
}
