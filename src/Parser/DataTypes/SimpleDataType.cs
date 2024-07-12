using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes
{
    public class SimpleDataType : SqlDataType
    {
        private readonly Identifier _identifier;
        public SimpleDataType(Identifier identifier)
        {
            _identifier = identifier;
        }

        public override string ToString()
        {
            return $"{_identifier.Name}";
        }
    }
}
