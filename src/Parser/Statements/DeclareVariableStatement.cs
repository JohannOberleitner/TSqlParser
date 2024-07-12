using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class DeclareVariableStatement : Statement
    {
        private readonly Identifier _identifier;
        private readonly SqlDataType _dataType;
        private readonly Expression? _initialValue;

        public DeclareVariableStatement(Identifier identifier, SqlDataType dataType, Expression? initialValue)
        {
            _identifier = identifier;
            _dataType = dataType;
            _initialValue = initialValue;
        }

        public Identifier Name => _identifier;
        public SqlDataType DataType => _dataType;

        public Expression? InitialValue => _initialValue;

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            if (_initialValue == null)
                return $"DECLARE {_identifier} {_dataType}";
            else
                return $"DECLARE {_identifier} {_dataType} = {_initialValue}";
        }
    }
}
