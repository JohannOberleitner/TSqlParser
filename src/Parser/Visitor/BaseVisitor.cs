using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions.CaseClauseDataStructures;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor
{
    /// <summary>
    /// Implements base logic for a visitor. 
    /// Follows already sequential statements, in block mode, etc.
    /// This visitor does not collect any data.
    /// </summary>
    public abstract class BaseVisitor : IVisitor, IExpressionVisitor, ITableSourceVisitor, IJoinVisitor
    {
        public virtual void Visit(IList<Statement> statements)
        {
            foreach (var statement in statements)
            {
                statement.Accept(this);
            }
        }

        public virtual void Visit(BlockStatement statement)
        {
            foreach (var childStatement in statement.Body)
            {
                childStatement.Accept(this);
            }
        }

        public virtual void Visit(IfStatement statement)
        {
            statement.Condition.Accept(this);
            statement.ThenClause.Accept(this);
            if (statement.HasElseClause)
                statement.ElseClause!.Accept(this);
        }

        public virtual void Visit(WhileStatement statement)
        {
            statement.Condition.Accept(this);
            statement.CodeBlock.Accept(this);
        }

        public virtual void Visit(TryCatchStatement statement)
        {
            statement.TryStatement.Accept(this);
            statement.CatchStatement.Accept(this);
        }

        public virtual void Visit(ReturnStatement statement)
        {
            statement.ReturnValue?.Accept(this);
        }

        public virtual void Visit(ThrowStatement statement)
        {
        }

        public virtual void Visit(BreakStatement statement)
        {
        }

        public virtual void Visit(ContinueStatement statement)
        {
        }

        public virtual void Visit(GotoStatement statement)
        {
        }

        public virtual void Visit(LabelStatement statement)
        {
        }

        public virtual void Visit(RaiseErrorStatement statement)
        {
        }

        public virtual void Visit(WaitForStatement statement)
        {
        }

        public virtual void Visit(WithStatement statement)
        {
        }

        public virtual void Visit(CallStoredProcedureStatement statement)
        {
        }

        public virtual void Visit(BeginTransactionStatement statement)
        {
        }

        public virtual void Visit(CommitStatement statement)
        {
        }

        public virtual void Visit(RollbackStatement statement)
        {
        }

        public virtual void Visit(SaveStatement statement)
        {
        }

        public virtual void Visit(PrintStatement statement)
        {
        }

        public virtual void Visit(CreateTableStatement statement)
        {
        }

        public virtual void Visit(AlterTableStatement statement)
        {
        }

        public virtual void Visit(DropTableStatement statement)
        {
        }

        public virtual void Visit(CreateStatisticsStatement statement)
        {
        }

        public virtual void Visit(CreateIndexStatement statement)
        {
        }

        public virtual void Visit(AlterIndexStatement statement)
        {
        }

        public virtual void Visit(ProcedureStatement statement)
        {
            statement.Body.Accept(this);
        }

        public virtual void Visit(TriggerStatement statement)
        {
            statement.Body.Accept(this);
        }

        public virtual void Visit(DeclareVariableStatement statement)
        {
        }

        public virtual void Visit(MultiDeclareStatement statement)
        {
        }

        public virtual void Visit(SetStatement statement)
        {
        }

        public virtual void Visit(SetLocalVariableStatement statement)
        {
            statement.Value.Accept(this);
        }

        public virtual void Visit(QueryStatement statement)
        {
            foreach (var arg in statement.ColumnExpressionList)
                arg.Expression.Accept(this);

            if (statement.FromClause != null)
            {
                foreach (var tableSource in statement.FromClause.TableSources)
                    tableSource.Accept(this);
            }
        }

        public virtual void Visit(InsertStatement statement)
        {
            statement.QueryStatement?.Accept(this);
            if (statement.ValueExpressions != null)
            {
                foreach (var valueExpression in statement.ValueExpressions)
                    valueExpression.Accept(this);
            }
        }

        public virtual void Visit(UpdateStatement statement)
        {
        }

        public virtual void Visit(DeleteStatement statement)
        {
        }

        public virtual void Visit(TruncateStatement statement)
        {
        }

        public virtual void Visit(MergeStatement statement)
        {
        }

        public virtual void Visit(OpenCursorStatement statement)
        {
        }

        public virtual void Visit(FetchCursorStatement statement)
        {
        }

        public virtual void Visit(CloseCursorStatement statement)
        {
        }

        public virtual void Visit(DeallocateCursorStatement statement)
        {
        }

        public virtual void Visit(GrantStatement statement)
        {
        }

        public virtual void Visit(GoStatement statement)
        {
        }

        public virtual void Visit(UseStatement statement)
        {
        }

        public virtual void Visit(UnrecognizedStatement statement)
        {
        }

        public virtual void Visit(StringExpression expression)
        {
        }
        public virtual void Visit(IntegerExpression expression)
        {
        }
        public virtual void Visit(FloatExpression expression)
        {
        }

        public virtual void Visit(IdentifierExpression expression)
        {
        }

        public virtual void Visit(UnaryOperatorExpression expression)
        {
            expression.Inner.Accept(this);
        }
        public virtual void Visit(PostfixUnaryOperatorExpression expression)
        {
            expression.Inner.Accept(this);
        }
        public virtual void Visit(BinaryOperatorExpression expression)
        {
            expression.First.Accept(this);
            expression.Second.Accept(this);
        }
        public virtual void Visit(InExpression expression)
        {
            expression.Subject.Accept(this);
            foreach (var item in expression.Items)
                item.Accept(this);
        }
        public virtual void Visit(LikeExpression expression)
        {
            expression.Left.Accept(this);
            expression.Right.Accept(this);
        }
        public virtual void Visit(BetweenOperatorExpression expression)
        {
            expression.Target.Accept(this);
            expression.LowerBoundary.Accept(this);
            expression.UpperBoundary.Accept(this);
        }
        public virtual void Visit(CastFunctionCallExpression expression)
        {
            expression.Target.Accept(this);
        }
        public virtual void Visit(FunctionCallExpression expression)
        {
            foreach (var argument in expression.ArgumentList)
                argument.Accept(this);
        }
        public virtual void Visit(RowNumberFunctionCallExpression expression)
        {
            foreach (var partitionExpression in expression.PartitionByValueExpressions)
                partitionExpression.Accept(this);
        }

        public virtual void Visit(SetOperationExpression expression)
        {
            expression.Query?.Accept(this);
        }

        public virtual void Visit(CaseExpression expression)
        {
            foreach (var caseClause in expression.CaseClauses)
                caseClause.Accept(this);
            expression.ElseClause?.Accept(this);
        }
        public virtual void Visit(CurrentOfExpression expression)
        {
            expression.CursorName.Accept(this);
        }
        public virtual void Visit(CursorExpression expression)
        {
            expression.Query.Accept(this);
        }

        public virtual void Visit(SubExpression expression)
        {
            expression.Inner.Accept(this);
        }
        public virtual void Visit(QueryExpression expression)
        {
            expression.Query.Accept(this);
        }

        public virtual void Visit(CaseClause caseClause)
        {
            caseClause.Condition.Accept(this);
            caseClause.Clause.Accept(this);
        }

        public virtual void Visit(ArgumentExpression argument)
        {
            argument.Expression.Accept(this);
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
            derivedTable.Query.Accept(this);
        }

        public virtual void Visit(OpenXMLTable openXMLTable)
        {
        }

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
