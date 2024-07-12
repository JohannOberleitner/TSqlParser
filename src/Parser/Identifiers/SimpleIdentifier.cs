using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers
{
    /// <summary>
    /// A simple identifier that consists of only 1 part.
    /// </summary>
    public class SimpleIdentifier : Identifier
    {
        private readonly Lexem _identifier;

        /// <summary>
        /// Used to initialize this identifier based on an Lexem identifier.
        /// </summary>
        /// <param name="identifier"></param>
        public SimpleIdentifier(Lexem identifier)
        {
            _identifier = identifier;
        }

        /// <summary>
        /// Returns the name from either the identifier.
        /// </summary>
        public override string Name => _identifier.Token;

        /// <summary>
        /// Returns just the name.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
