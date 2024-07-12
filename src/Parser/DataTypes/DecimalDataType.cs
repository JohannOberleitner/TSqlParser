using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes
{
    public class DecimalDataType : SqlDataType
    {
        private readonly Identifier _identifier;
        private readonly int? _precision;
        private readonly int? _scale;

        public DecimalDataType(Identifier identifier, int? precision, int? scale)
        {
            _identifier = identifier;
            _scale = scale;
            _precision = precision;
        }

        public override string ToString()
        {
            if (_precision.HasValue && _scale.HasValue)
                return $"{_identifier.Name}({_precision},{_scale})";
            else if (!_scale.HasValue)
                return $"{_identifier.Name}({_precision})";
            else
                return $"{_identifier.Name}";
        }
    }
}
