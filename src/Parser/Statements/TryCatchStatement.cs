using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    /// <summary>
    /// Represents an SQL TRY CATCH statement
    /// </summary>
    public class TryCatchStatement : Statement
    {
        private readonly Statement _tryStatement;
        private readonly Statement _catchStatement;

        public TryCatchStatement(Statement tryStatement, Statement catchStatement)
        {
            _tryStatement = tryStatement;
            _catchStatement = catchStatement;
        }

        public Statement TryStatement => _tryStatement;
        public Statement CatchStatement => _catchStatement;

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("BEGIN TRY");
            sb.AppendLine(_tryStatement.ToString());
            sb.AppendLine("END TRY");
            sb.AppendLine("BEGIN CATCH");
            sb.AppendLine(_catchStatement.ToString());
            sb.AppendLine("END CATCH");
            return sb.ToString();
        }
    }
}
