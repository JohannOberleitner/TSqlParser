using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes
{
    public class DateTime2DataType : SqlDataType
    {
        private readonly Identifier _identifier;
        private readonly long? _size;

        public DateTime2DataType(Identifier identifier, long? size = null)
        {
            _identifier = identifier;
            _size = size;
        }

        public override string ToString()
        {
            if (_size.HasValue)
                return $"{_identifier.Name}({_size.Value})";
            else
                return $"{_identifier.Name}";
        }
    }
}
