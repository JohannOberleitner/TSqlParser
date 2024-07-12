using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor
{
    public class BaseTableSourceVisitor : ITableSourceVisitor, IJoinVisitor
    {
        private readonly IExpressionVisitor? _expressionVisitor;

        public BaseTableSourceVisitor()
        {
            _expressionVisitor = null;
        }


        public BaseTableSourceVisitor(IExpressionVisitor expressionVisitor)
        {
            _expressionVisitor = expressionVisitor;
        }

        public virtual void Visit(SimpleTable table)
        {
        }
        public virtual void Visit(TableJoin tableJoin)
        {
            tableJoin.First.Accept(this);
            foreach (var join in tableJoin.Joins)
                join.Accept(this);
        }

        public virtual void Visit(SubTableSource subTable)
        {
            subTable.Inner.Accept(this);
        }

        public virtual void Visit(DerivedTable derivedTable)
        {
            if (_expressionVisitor == null)
                throw new ArgumentException("BaseTableSourceVisitor has 'null' _expressionVisitor");

            derivedTable.Query.Accept(_expressionVisitor);
        }


        public virtual void Visit(OpenXMLTable openXMLTable)
        { }

        public virtual void Visit(InnerJoin innerJoin)
        {
            innerJoin.JoinSource.Accept(this);
        }
        public virtual void Visit(OuterJoin outerJoin)
        {
            outerJoin.JoinSource.Accept(this);
        }

        public virtual void Visit(CrossApply crossApply)
        {
            crossApply.Right.Accept(this);
        }
        public virtual void Visit(CrossJoin crossJoin)
        {
            crossJoin.Right.Accept(this);
        }
    }

}
