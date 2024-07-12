namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser
{
    /// <summary>
    /// Generic exception for typically unsupported stuff during the parse process.
    /// </summary>
    public class ParserException : Exception
    {
        /// <summary>
        /// Initializes the exception with the error message.
        /// </summary>
        /// <param name="message"></param>
        public ParserException(string message)
        : base(message)
        {
        }
    }
}