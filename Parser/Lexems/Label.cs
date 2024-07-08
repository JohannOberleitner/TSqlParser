using System;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents a label in an TSQL script that can be the target of 
    /// a goto statement.
    /// </summary>
    public class Label: Lexem
    {
        string _label;

        public Label(string label)
        {
            _label = label;
        }

        public string Name => _label;

        public override string Token { get { return Name; } }

        public override string ToString()
        {
            return $"LABEL {_label}";
        }
    }
}
