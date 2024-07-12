using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes
{
    public class CharacterDataType : SqlDataType
    {
        private readonly Identifier _identifier;
        private readonly long? _size;

        public CharacterDataType(Identifier identifier, long? size = null)
        {
            _identifier = identifier;
            _size = size;
        }

        public override string ToString()
        {
            if (_size == null)
            {
                return $"{_identifier.Name}";
            }
            if (_size.Value == -1)
                return $"{_identifier.Name}(max)";
            else
                return $"{_identifier.Name}({_size.Value})";
        }
    }
}
