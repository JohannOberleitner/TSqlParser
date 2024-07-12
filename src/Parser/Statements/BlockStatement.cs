using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class BlockStatement : Statement
    {
        private readonly IList<Statement> _body;
        private readonly bool _skipBeginEndOutput;

        public BlockStatement(IList<Statement> body, bool skipBeginEndOutput = false)
        {
            _body = body;
            _skipBeginEndOutput = skipBeginEndOutput;
        }

        public IList<Statement> Body => _body;


        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (!_skipBeginEndOutput)
                sb.AppendLine("BEGIN");
            foreach (var statement in _body)
                sb.AppendLine(statement.ToString());
            if (!_skipBeginEndOutput)
                sb.AppendLine("END");
            return sb.ToString();
        }
    }
}
