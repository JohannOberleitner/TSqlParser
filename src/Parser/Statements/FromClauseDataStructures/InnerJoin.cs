using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures
{
    public class InnerJoin : Join
    {
        private readonly string _joinType;
        private readonly TableSource _joinSource;
        private readonly Expression _joinCondition;

        public InnerJoin(TableSource joinSource, string joinType, Expression joinCondition)
        {
            _joinType = joinType;
            _joinSource = joinSource;
            _joinCondition = joinCondition;
        }

        public override void Accept(IJoinVisitor visitor)
        {
            visitor.Visit(this);
        }

        public TableSource JoinSource => _joinSource;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(_joinType);
            sb.Append(" JOIN ");
            sb.Append(' ');
            sb.Append(_joinSource);
            sb.Append(" ON ");
            sb.Append(_joinCondition.ToString());
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
