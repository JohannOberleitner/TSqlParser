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
    }
}
