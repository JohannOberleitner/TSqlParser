using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures
{
    public class OuterJoin : Join
    {
        private readonly string _joinType;
        private readonly TableSource _joinSource;
        private readonly Expression _joinCondition;
        private readonly string _joinHint;

        public OuterJoin(TableSource joinSource, string joinType, string joinHint, Expression joinCondition)
        {
            _joinSource = joinSource;
            _joinType = joinType;
            _joinHint = joinHint;
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
            if (_joinHint != null)
                sb.Append(_joinHint);
            sb.Append(" JOIN ");
            sb.Append(_joinSource);
            sb.Append(" ON ");
            sb.Append(_joinCondition.ToString());
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
