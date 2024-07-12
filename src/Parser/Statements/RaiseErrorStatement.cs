using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class RaiseErrorStatement : Statement
    {
        private readonly Expression _msgId;
        private readonly Expression _severity;
        private readonly Expression _state;

        /*TODO REMOVE public RaiseErrorStatement()
        {
        }*/
        public RaiseErrorStatement(Expression msgId, Expression severity, Expression state)
        {
            _msgId = msgId;
            _severity = severity;
            _state = state;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"RAISERROR {_msgId}, {_severity}, {_state}";
        }
    }
}
