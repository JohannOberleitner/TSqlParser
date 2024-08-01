using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public enum ParameterDeclarationNullValue
    {
        None,
        Null = 1,
        NotNull = 2
    }

    /// <summary>
    /// Represents a parameter declaration for a stored procedure.
    /// </summary>
    public class ParameterDeclaration
    {
        private readonly Identifier _identifier;
        private readonly SqlDataType _parameterType;

        private readonly ParameterDeclarationNullValue _nullValue;
        private readonly IList<Identifier> _additionalFlags;
        private readonly Expression? _defaultValue;

        private readonly string _additionalFlagsRepresentation;

        public ParameterDeclaration(Identifier identifier, SqlDataType parameterType, ParameterDeclarationNullValue nullValue, IList<Identifier> additionalFlags, Expression? defaultValue)
        {
            _identifier = identifier;
            _parameterType = parameterType;
            _nullValue = nullValue;
            _additionalFlags = additionalFlags;
            _defaultValue = defaultValue;
            _additionalFlagsRepresentation = MakeAdditionalFlagsRepresentation();
        }

        private string MakeAdditionalFlagsRepresentation()
        {
            if (_additionalFlags.Count == 0)
                return "";

            var sb = new StringBuilder();
            sb.Append(_additionalFlags[0].Name);
            for (int i = 1; i < _additionalFlags.Count; ++i)
            {
                sb.Append(' ');
                sb.Append(_additionalFlags[i].Name);
            }
            return sb.ToString();
        }

        private string NullValue
        {
            get
            {
                switch (_nullValue)
                {
                    case ParameterDeclarationNullValue.None:
                        return "";
                    case ParameterDeclarationNullValue.Null:
                        return " NULL";
                    case ParameterDeclarationNullValue.NotNull:
                        return " NOT NULL";
                    default:
                        throw new NotImplementedException($"Invalid NullValue Type:{_nullValue}");
                }
            }
        }

        public override string ToString()
        {
            if (_defaultValue == null)
                return $"{_identifier.Name} {_parameterType}{NullValue}{_additionalFlagsRepresentation}";
            else
                return $"{_identifier.Name} {_parameterType}{NullValue}{_additionalFlagsRepresentation} = {_defaultValue}";
        }
    }
}
