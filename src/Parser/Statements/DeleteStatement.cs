using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements
{
    public class DeleteStatement : Statement
    {
        private readonly Identifier _tableName;
        private readonly WhereClause? _whereClause;

        public DeleteStatement(Identifier tableName, WhereClause? whereClause)
        {
            _tableName = tableName;
            _whereClause = whereClause;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Identifier TableName => _tableName;

        public override string ToString()
        {
            return $"DELETE FROM {_tableName} {_whereClause}";
        }
    }
}
