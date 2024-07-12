using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers
{
    /// <summary>
    /// Represents an identifier that consists of multiple parts.
    /// </summary>
    public class CombinedIdentifier : Identifier
    {
        private readonly IList<Lexem> _identifiers = new List<Lexem>();
        private readonly string _representationCache;

        public CombinedIdentifier(IList<Lexem> identifiers)
        {
            _identifiers = identifiers;
            _representationCache = MakeStringRepresentation();
        }

        private string MakeStringRepresentation()
        {
            var sb = new StringBuilder();
            if (_identifiers.Count == 0)
            {
                // TODO: throw Exception
            }
            if (_identifiers[0] == null)
            {
                // TODO; throw Exception
            }

            sb.Append(_identifiers[0].Token);
            for (int i = 1; i < _identifiers.Count; ++i)
            {
                sb.Append('.');
                sb.Append(_identifiers[i].Token);
            }
            return sb.ToString();
        }

        public override string Name => _representationCache;

        /// <summary>
        /// Returns true for this identifier.
        /// </summary>
        public override bool IsMultipart => true;

        public string GetPart(int indexFromEnd)
        {
            return _identifiers[_identifiers.Count - indexFromEnd - 1].Token.ToString();
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
