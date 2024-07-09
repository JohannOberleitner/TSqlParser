namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a TSQL operator.
    /// Among those could be:
    /// <list type="table">
    ///   <listheader>
    ///     <term>Type</term>
    ///     <description>Description</description>
    ///   </listheader>
    ///   <item>
    ///         <term>Multiplicative</term>
    ///         <description> * (multiplication),/ (division),% (remainder)</description>
    ///   </item>
    ///   <item>
    ///         <term>Additive</term>> 
    ///         <description>+ (addition),- (subtraction), amp (concatenation)</description>
    ///   </item>
    ///   <item>
    ///         <term>Comparison</term> 
    ///         <description> = (equal),&lt; (less than), > (greater than)</description>
    ///   </item>
    ///   <item>
    ///         <term>Assignment</term> 
    ///         <description>=,+=, -=</description>
    ///   </item>
    ///   <item>
    ///         <term>Scope resolution</term>
    ///         <description>::</description>     
    ///   </item>
    ///   <item>
    ///         <term>Logical</term> 
    ///         <description>AND, OR, NOT</description> 
    ///   </item>
    ///   <item>
    ///         <term>SQL</term>
    ///         <description>IS, IN, LIKE</description> 
    ///   </item>
    /// </list>
    /// </summary>
    public class OperatorSymbol : Lexem
    {
        private readonly string _symbol;

        /// <summary>
        /// Creates an instance of an OperatorSymbol.
        /// </summary>
        /// <param name="c">Initializes the operator with this string as symbol.</param>
        public OperatorSymbol(string c)
        {
            _symbol = c;
        }

        /// <summary>
        /// Returns the symbol that was used at construction time. 
        /// </summary>
        public string Symbol => _symbol;

        /// <summary>
        /// Checks if the operator consists of a multiplicative operator, i.e.
        /// multiplication *, division /, and remainder %.
        /// </summary>
        //public bool IsMultiplicationSymbol => _symbol == "*" || _symbol == "/" || _symbol == "%";
        public bool IsMultiplicationSymbol => _symbol switch { "*" or "/" or "%" => true, _ => false };

        /// <summary>
        /// Checks if the operator consist of an additional operator, i.e.
        /// additiona +, subtraction -, concatenation &amp;, ^, pipe |
        /// </summary>
        public bool IsAdditionSymbol => _symbol == "+" || _symbol == "-" || _symbol == "&" || _symbol == "^" || _symbol == "|" || _symbol == "&";

        /// <summary>
        /// Returns if the operator consist of a comparision  operator, i.e.
        /// equals =, greater than >, less than &lt; greater than >=, ...
        /// </summary>
        public bool IsComparisonSymbol => _symbol == "=" || _symbol == ">" || _symbol == "<" || _symbol == ">=" || _symbol == "<=" || _symbol == "<>" || _symbol == "!>" || _symbol == "!<" || _symbol == "!=";

        /// <summary>
        /// Returns if the operator is an assignment symbol
        /// </summary>
        public bool IsAssignmentSymbol => _symbol == "=" || _symbol == "+=" || _symbol == "-=" || _symbol == "*=" || _symbol == "/=" || _symbol == "%=" || _symbol == "&=" || _symbol == "|=" || _symbol == "^=";

        /// <summary>
        /// Returns true if the operator is a scope resolution symbol.
        /// </summary>
        public bool IsScopeResolutionSymbol => _symbol == "::";

        /// <summary>
        /// Returns true if the operator is a TSQL AND operator.
        /// </summary>
        public bool IsAnd => _symbol.Equals("AND", StringComparison.OrdinalIgnoreCase);
        /// <summary>
        /// Returns true if the operator is a TSQL OR operator.
        /// </summary>
        public bool IsOr => _symbol.Equals("OR", StringComparison.OrdinalIgnoreCase);
        /// <summary>
        /// Returns true if the operator is a TSQL NOT operator.
        /// </summary>
        public bool IsNot => _symbol.Equals("NOT", StringComparison.OrdinalIgnoreCase);
        /// <summary>
        /// Returns true if the operator is a TSQL IS operator.
        /// </summary>
        public bool IsIs => _symbol.Equals("IS", StringComparison.OrdinalIgnoreCase);
        /// <summary>
        /// Returns true if the operator is a TSQL IN operator.
        /// </summary>
        public bool IsIn => _symbol.Equals("IN", StringComparison.OrdinalIgnoreCase);
        /// <summary>
        /// Returns true if the operator is a TSQL LIKE operator.
        /// </summary>
        public bool IsLike => _symbol.Equals("LIKE", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Returns the operator symbol.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _symbol.ToString();
        }
    }
}
