namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a label in an TSQL script that can be the target of 
    /// a goto statement.
    /// </summary>
    public class Label : Lexem
    {
        private readonly string _label;

        /// <summary>
        /// Initializes this label with the string value of the label.
        /// </summary>
        /// <param name="label"></param>
        public Label(string label)
        {
            _label = label;
        }

        /// <summary>
        /// Returns the value of the label that was provided in the constructor. 
        /// </summary>
        public string Name => _label;

        /// <summary>
        /// Returns as token the string value of the label.
        /// </summary>
        public override string Token => Name;

        /// <summary>
        /// Returns LABEL and the label value that was provided to the constructor.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"LABEL {_label}";
        }
    }
}
