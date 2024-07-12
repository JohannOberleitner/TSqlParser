using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class ThrowStatement : Statement
    {
        private readonly Expression? _errorNumber;
        private readonly Expression? _message;
        private readonly Expression? _state;

        public ThrowStatement()
        {
        }
        public ThrowStatement(Expression errorNumber, Expression message, Expression state)
        {
            _errorNumber = errorNumber;
            _message = message;
            _state = state;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            if (_errorNumber == null)
                return "THROW";
            else
                return $"THROW {_errorNumber}, {_message}, {_state}";
        }
    }
}
