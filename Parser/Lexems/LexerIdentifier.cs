using System;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents an identifier in TSQL.
    /// </summary>
    public class LexerIdentifier: Lexem
    {
        string _identifier;

        public LexerIdentifier(string name)
        {
            _identifier = name;          
        }

        public string Name => _identifier;

        public override string Token { get { return Name; } }

        public bool HasAtSymbol()
        {
            return _identifier[0] == '@';
        }

        public bool HasDoubleAtSymbols()
        {
            return _identifier[0] == '@' && _identifier.Length > 1 && _identifier[1] == '@';
        }

        public override string ToString()
        {
            return $"Id {_identifier}";
        }
    }
}
