using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Globalization;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class MultiDeclareStatement : Statement
    {
        private readonly IList<DeclareVariableStatement> _declareStatements;
        private readonly string _representation;

        public MultiDeclareStatement(IList<DeclareVariableStatement> declareStatements)
        {
            _declareStatements = declareStatements;
            _representation = MakeRepresentation();
        }

        public IList<DeclareVariableStatement> Declarations => _declareStatements;

        private string MakeRepresentation()
        {
            var sb = new StringBuilder();
            sb.Append("DECLARE ");
            sb.Append(CultureInfo.InvariantCulture, $"{_declareStatements[0].Name} {_declareStatements[0].DataType.ToString()}");
            if (_declareStatements[0].InitialValue != null)
            {
                sb.Append(CultureInfo.InvariantCulture, $" = {_declareStatements[0].InitialValue!.ToString()}");
            }

            for (int i = 1; i < _declareStatements.Count; ++i)
            {
                sb.Append(',');
                sb.Append(CultureInfo.InvariantCulture, $"{_declareStatements[i].Name} {_declareStatements[i].DataType.ToString()}");
                if (_declareStatements[i].InitialValue != null)
                {
                    sb.Append(CultureInfo.InvariantCulture, $" = {_declareStatements[i].InitialValue!.ToString()}");
                }
            }
            return sb.ToString();
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return _representation;
        }
    }
}
