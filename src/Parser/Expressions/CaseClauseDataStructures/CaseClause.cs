using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions.CaseClauseDataStructures
{
    public class CaseClause
    {
        private readonly Expression _condition;
        private readonly Expression _clause;

        public CaseClause(Expression condition, Expression clause)
        {
            _condition = condition;
            _clause = clause;
        }

        public Expression Condition => _condition;
        public Expression Clause => _clause;

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"WHEN {_condition} THEN {_clause}";
        }

    }
}
