using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions
{
    /// <summary>
    /// Base class for all TSQL Expressions.
    /// </summary>
    public abstract class Expression
    {
        public abstract void Accept(IExpressionVisitor visitor);

        public override string ToString()
        {
            return "Expression";
        }
    }
}
