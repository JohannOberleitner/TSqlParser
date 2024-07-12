using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class SetLocalVariableStatement : Statement
    {
        private readonly Identifier _variable;
        private readonly Expression _expr;

        public SetLocalVariableStatement(Identifier variable, Expression expr)
        {
            _variable = variable;
            _expr = expr;
        }

        public Identifier Variable => _variable;
        public Expression Value => _expr;

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            return $"SET {_variable.Name} = {_expr}";
        }
    }
}
