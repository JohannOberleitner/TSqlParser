namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers
{
    /// <summary>
    /// Abstract base class for all identifiers
    /// </summary>
    public abstract class Identifier
    {
        /// <summary>
        /// Parent property that returns the name, i.e. the textual
        /// representation of the full identifier.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Compares the given string with this name.
        /// Ignore case sensitivity or culture.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsEqual(string other)
        {
            return Name.Equals(other, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns true if the identifier consists of multiple parts
        /// </summary>
        public virtual bool IsMultipart => false;

        /// <summary>
        /// If the name contains brackets it Returns the string inside the outermost
        /// brackets. Otherwise, this returns just the name.
        /// </summary>
        public string NameWithBrackets
        {
            get
            {
                if (Name.StartsWith('[') && Name.EndsWith(']'))
                    return Name[1..^2];
                else
                    return Name;
            }
        }
    }
}
