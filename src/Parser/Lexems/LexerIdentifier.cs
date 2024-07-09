namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents an identifier in TSQL.
    /// </summary>
    public class LexerIdentifier : Lexem
    {
        private readonly string _identifier;

        /// <summary>
        /// Creates an instance of a LexerIdentifier.
        /// </summary>
        /// <param name="name">The name of the identifier.</param>
        public LexerIdentifier(string name)
        {
            _identifier = name;
        }

        /// <summary>
        /// Returns the name of the identifier as provided in the constructor.
        /// </summary>
        public string Name => _identifier;

        /// <summary>
        /// Returns as token the name of the identifier.
        /// </summary>
        public override string Token => Name;

        /// <summary>
        /// Checks if the identifier starts with an @ symbol.
        /// </summary>
        /// <returns>true if the first character is an @ symbol </returns>
        public bool HasAtSymbol()
        {
            return _identifier[0] == '@';
        }

        /// <summary>
        /// Checks if the identifier starts with two @@ symbols.
        /// </summary>
        /// <returns>true if the first and second characters are @ symbols.</returns>
        public bool HasDoubleAtSymbols()
        {
            return _identifier[0] == '@' && _identifier.Length > 1 && _identifier[1] == '@';
        }

        /// <summary>
        /// Returns Id and the identifier value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Id {_identifier}";
        }
    }
}
