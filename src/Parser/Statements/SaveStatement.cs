using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class SaveStatement : Statement
    {
        private readonly Expression _savePoint;

        public SaveStatement(Expression savePoint)
        {
            _savePoint = savePoint;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override string ToString()
        {
            return $"SAVE TRANSACTION {_savePoint}";
        }
    }
}
