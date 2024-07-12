using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes
{
    public class ColumnDeclaration
    {
        private readonly Identifier _identifier;
        private readonly SqlDataType _parameterType;
        private readonly IdentityClause? _identityClause;
        private readonly DefaultValueClause? _defaultValueClause;
        private readonly IList<ColumnConstraint> _columnConstraints;

        private readonly string _columnConstraintsRepresentation;

        public ColumnDeclaration(Identifier identifier, SqlDataType parameterType, IdentityClause? identityClause, DefaultValueClause? defaultValueClause, IList<ColumnConstraint> columnConstraints)
        {
            _identifier = identifier;
            _parameterType = parameterType;
            _columnConstraints = columnConstraints;
            _identityClause = identityClause;
            _defaultValueClause = defaultValueClause;
            _columnConstraintsRepresentation = MakeColumnConstraintRepresentation();
        }

        private string MakeColumnConstraintRepresentation()
        {
            if (_columnConstraints.Count == 0)
                return "";

            var sb = new StringBuilder();
            sb.Append(_columnConstraints[0].ToString());
            for (int i = 1; i < _columnConstraints.Count; ++i)
            {
                sb.Append(' ');
                sb.Append(_columnConstraints[i].ToString());
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(_identifier.Name);
            sb.Append(' ');
            sb.Append(_parameterType.ToString());
            if (_identityClause != null)
            {
                sb.Append(' ');
                sb.Append(_identityClause);
            }
            if (_defaultValueClause != null)
            {
                sb.Append(' ');
                sb.Append(_defaultValueClause);
            }
            if (_columnConstraints != null)
            {
                sb.Append(' ');
                sb.Append(_columnConstraintsRepresentation);
            }
            return sb.ToString();
        }
    }
}
