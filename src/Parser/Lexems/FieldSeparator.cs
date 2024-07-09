namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a separator for fields (",")
    /// </summary>
    public class FieldSeparator : Lexem
    {
        /// <summary>
        /// Returnd SEPARATOR.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "SEPARATOR";
        }
    }
}
