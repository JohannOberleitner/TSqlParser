using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Globalization;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class ProcedureStatement : Statement
    {
        private readonly CreationType _creationType;
        private readonly Identifier _identifier;
        private readonly IList<ParameterDeclaration> _parameterList;
        private readonly string _parameterListRepresentation;
        private readonly Statement _body;

        public ProcedureStatement(CreationType creationType, Identifier identifier, IList<ParameterDeclaration> parameterList, Statement body)
        {
            _creationType = creationType;
            _identifier = identifier;
            _parameterList = parameterList;
            _parameterListRepresentation = MakeParameterListRepresentation();
            _body = body;
        }

        public Identifier Identifier => _identifier;

        private string MakeParameterListRepresentation()
        {
            if (_parameterList.Count == 0)
                return "";

            var sb = new StringBuilder();
            sb.Append(_parameterList[0].ToString());
            for (int i = 1; i < _parameterList.Count; ++i)
            {
                sb.AppendLine(",");
                sb.Append(_parameterList[i].ToString());
            }
            return sb.ToString();
        }

        public Statement Body => _body;

        private string GetKeyword()
        {
            switch (_creationType)
            {
                case CreationType.Create:
                    return "CREATE";
                case CreationType.Alter:
                    return "ALTER";
                default: throw new NotImplementedException($"Invalid creationType {_creationType}");
            }
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(GetKeyword());
            sb.AppendLine(CultureInfo.InvariantCulture, $" PROCEDURE {_identifier}");
            sb.AppendLine(_parameterListRepresentation);
            sb.AppendLine("AS");
            sb.Append(_body.ToString());
            return sb.ToString();
        }
    }
}
