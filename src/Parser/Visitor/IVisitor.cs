using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor
{
    public interface IVisitor
    {
        void Visit(BlockStatement statement);
        void Visit(IfStatement statement);

        void Visit(WhileStatement statement);

        void Visit(TryCatchStatement statement);

        void Visit(ReturnStatement statement);

        void Visit(ThrowStatement statement);

        void Visit(BreakStatement statement);
        void Visit(ContinueStatement statement);
        void Visit(GotoStatement statement);
        void Visit(LabelStatement statement);
        void Visit(RaiseErrorStatement statement);
        void Visit(WaitForStatement statement);
        void Visit(WithStatement statement);

        void Visit(CallStoredProcedureStatement statement);

        void Visit(BeginTransactionStatement statement);
        void Visit(CommitStatement statement);
        void Visit(RollbackStatement statement);
        void Visit(SaveStatement statement);

        void Visit(PrintStatement statement);

        void Visit(CreateTableStatement statement);
        void Visit(AlterTableStatement statement);
        void Visit(DropTableStatement statement);
        void Visit(CreateStatisticsStatement statement);
        void Visit(CreateIndexStatement statement);
        void Visit(AlterIndexStatement statement);
        void Visit(ProcedureStatement statement);
        void Visit(TriggerStatement statement);

        void Visit(DeclareVariableStatement statement);
        void Visit(MultiDeclareStatement statement);
        void Visit(SetStatement statement);
        void Visit(SetLocalVariableStatement statement);

        void Visit(QueryStatement statement);
        void Visit(InsertStatement statement);
        void Visit(UpdateStatement statement);
        void Visit(DeleteStatement statement);
        void Visit(TruncateStatement statement);
        void Visit(MergeStatement statement);
        void Visit(OpenCursorStatement statement);
        void Visit(FetchCursorStatement statement);
        void Visit(CloseCursorStatement statement);
        void Visit(DeallocateCursorStatement statement);

        void Visit(GrantStatement statement);


        void Visit(GoStatement statement);
        void Visit(UseStatement statement);

        void Visit(UnrecognizedStatement statement);

    }
}
