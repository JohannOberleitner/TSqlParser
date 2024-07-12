using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers
{
    /// <summary>
    /// A simple identifier that consists of only 1 part.
    /// </summary>
    public class ExpressionIdentifier : Identifier
    {
        private readonly Expression _expression;

        /// <summary>
        /// Used to initialize this identifier based on an expression.
        /// </summary>
        /// <remarks>Perhaps better to make an additional Identifeir class </remarks>
        /// <param name="expression"></param>
        public ExpressionIdentifier(Expression expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Returns the name from the expression.
        /// </summary>
        public override string Name => _expression.ToString();

        /// <summary>
        /// Returns just the name.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
