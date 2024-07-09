namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a semicolon (";") that occurred in the SQL script.
    /// </summary>
    public class Semicolon : Lexem
    {
        /// <summary>
        /// Constructs a semicolon lexem.
        /// </summary>
        public Semicolon()
        {
        }

        /// <summary>
        /// Returns SEMICOLON.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "SEMICOLON";
        }
    }
}