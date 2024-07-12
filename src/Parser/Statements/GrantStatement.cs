using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class GrantStatement : Statement
    {
        private readonly string _permission;
        private readonly Expression _principal;
        public GrantStatement(string permission, Expression principal)
        {
            _principal = principal;
            _permission = permission;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"GRANT {_permission} TO {_principal}";
        }
    }
}
