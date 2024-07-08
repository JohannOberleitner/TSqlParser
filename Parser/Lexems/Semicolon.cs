namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a semicolon (";") that occurred in the SQL script.
    /// </summary>
    public class Semicolon: Lexem
    {
        public Semicolon()
        {
        }

        public override string ToString()
        {
            return "SEMICOLON";
        }
    }
}