using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor
{
    public interface ITableSourceVisitor
    {
        void Visit(SimpleTable table);
        void Visit(TableJoin join);

        void Visit(SubTableSource subTable);

        void Visit(DerivedTable derivedTable);

        void Visit(OpenXMLTable openXMLTable);
    }
}
