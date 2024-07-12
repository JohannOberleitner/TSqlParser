using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class CreateTableStatement : Statement
    {
        private readonly CreationType _creationType;
        private readonly Identifier _tableName;
        private readonly TableType _tableType;

        public CreateTableStatement(CreationType creationType, Identifier tableName, TableType tableType)
        {
            _creationType = creationType;
            _tableName = tableName;
            _tableType = tableType;
        }

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
            sb.Append(" TABLE ");
            sb.Append(_tableName);
            sb.AppendLine();
            sb.Append(_tableType);
            return sb.ToString();
        }
    }
}
