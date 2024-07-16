using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.SelectClauseDataStructures
{
    public class ColumnDescriptor
    {
        private readonly Expression _expression;
        private readonly Identifier? _alias;
        public ColumnDescriptor(Expression expression, Identifier? alias)
        {
            _expression = expression;
            _alias = alias;
        }

        public Expression Expression => _expression;
        public override string ToString()
        {
            if (_alias != null)
                return $"{_expression} AS {_alias}";
            else
                return _expression.ToString();
        }
    }
}
