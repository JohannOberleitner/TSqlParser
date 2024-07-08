using System;

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
        public virtual string Token
            => this.ToString()!; 
    }
}
