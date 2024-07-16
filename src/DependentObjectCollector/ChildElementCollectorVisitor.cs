using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.DependentObjectCollector;

public class ChildElementCollectorVisitor : BaseVisitor
{
    private readonly ICollectorVisitorStorage _storage;
    private string _context;

    public ChildElementCollectorVisitor(ICollectorVisitorStorage storage)
    {     
        _storage = storage;
    }

    public override void Visit(UpdateStatement updateStatement)
    {
        _storage.AddTableUsage(updateStatement.TableName.Name, updateStatement);
    }

    public override void Visit(QueryStatement queryStatement)
    {
        base.Visit(queryStatement);

        if (queryStatement.FromClause == null)
          return;

        /* (var table in queryStatement.FromClause.TableSources)
        {
            table.Accept(this);
        }*/
        
    }

    public override void Visit(SimpleTable table)
    {
      _storage.AddTableUsage(table.TableName.ToString(), "todo");
    }

    public override void Visit(InsertStatement statement)
    {
        _storage.AddTableUsage(statement.TableName.Name, "todo");
        statement.QueryStatement?.Accept(this);
    }

    public override void Visit(CallStoredProcedureStatement statement)
    {
        _storage.AddStoredProcCallUsage(statement.Name.Name, "todo");
    }

    public override void Visit(ProcedureStatement statement)
    {
        _storage.AddStoredProcCreateUsage(statement.Identifier.Name, "todo");
        base.Visit(statement);
    }

    public override void Visit(FunctionCallExpression expression)
    {
        base.Visit(expression);
        _storage.AddFunctionCall(expression.Identifier.Name, expression);
    }

    public override void Visit(OuterJoin outerJoin)
    {
        outerJoin.JoinCondition.Accept(this);
    }   

    public override void Visit(InnerJoin outerJoin)
    {
        outerJoin.JoinCondition.Accept(this);
    }
}
