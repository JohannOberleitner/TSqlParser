using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Globalization;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    /// <summary>
    /// Represents a SQL TRIGGER statement
    /// </summary>
    public class TriggerStatement : Statement
    {
        private readonly CreationType _creationType;
        private readonly Identifier _identifier;
        private readonly Identifier _target;
        //private IList<ParameterDeclaration> _parameterList;
        //private string _parameterListRepresentation;
        private readonly Statement _body;

        public TriggerStatement(CreationType creationType, Identifier identifier, Identifier target, Statement body)
        {
            _creationType = creationType;
            _identifier = identifier;
            _target = target;
            _body = body;
        }

        public Identifier Identifier => _identifier;

        public Identifier Target => _target;

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
            sb.AppendLine(CultureInfo.InvariantCulture, $" TRIGGER {_identifier}");
            sb.AppendLine(CultureInfo.InvariantCulture, $" ON {_target}");
            sb.AppendLine("AS");
            sb.Append(_body.ToString());
            return sb.ToString();
        }
    }
}
