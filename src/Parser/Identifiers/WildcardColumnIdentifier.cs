namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers
{
    /// <summary>
    /// Represents an identifier that is just the wildcard.
    /// </summary>
    /// <remarks>
    /// This is a pseudo identifier, used to support * for identifiers.
    /// </remarks>
    public class WildcardColumnIdentifier : Identifier
    {
        public WildcardColumnIdentifier()
        {
        }

        /// <summary>
        /// The name is always "*".
        /// </summary>
        public override string Name => "*";

        /// <summary>
        /// Returns just "*".
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "*";
        }
    }
}
