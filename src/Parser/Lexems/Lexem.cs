namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Base class for Lexems
    /// </summary>
    /// <remarks>In the base class is a single Token proeprty that returns 
    /// a string representation.
    /// </remarks>
    public abstract class Lexem
    {
        /// <summary>
        /// If not overwritten returns the value of ToString() for the token.
        /// </summary>
        public virtual string Token => this.ToString()!;

        /// <summary>
        /// Compares the Token with a given set of strings.
        /// The comparison happens case insensitive.
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public bool TokenEquals(params string[] strings)
        {
            foreach (var s in strings)
            {
                if (Token.Equals(s, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if this lexem is a Keyword and equals one of the provided
        /// strings.
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public virtual bool KeywordEquals(params string[] strings) => false;

        /// <summary>
        /// Checks if this this lexem is an operation symbol and 
        /// if the symbol fits one of the contained operatino symbol. 
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public bool CheckWithOperationSymbols(params string[] strings)
        {
            foreach (var s in strings)
            {
                if (CheckWithOperationSymbol(s))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if this this lexem is an operation symbol and 
        /// if the symbol equals the contained operatino symbol. 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public virtual bool CheckWithOperationSymbol(string s) => false;

        /// <summary>
        /// Checks if the operator consists of a multiplicative operator, i.e.
        /// multiplication *, division /, and remainder %.
        /// </summary>
        //public bool IsMultiplicationSymbol => _symbol == "*" || _symbol == "/" || _symbol == "%";
        public virtual bool IsMultiplicationSymbol => false;

        /// <summary>
        /// Checks if the operator consist of an additional operator, i.e.
        /// additiona +, subtraction -, concatenation &amp;, ^, pipe |
        /// </summary>
        public virtual bool IsAdditionSymbol => false;

        /// <summary>
        /// Returns if the operator consist of a comparision  operator, i.e.
        /// equals =, greater than >, less than &lt; greater than >=, ...
        /// </summary>
        public virtual bool IsComparisonSymbol => false;

        /// <summary>
        /// Returns if the operator is an assignment symbol
        /// </summary>
        public virtual bool IsAssignmentSymbol => false;

    }
}
