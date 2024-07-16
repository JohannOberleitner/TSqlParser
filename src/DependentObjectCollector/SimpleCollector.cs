using System.Data;
using Microsoft.VisualBasic;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.DependentObjectCollector;

/// <summary>
/// Collects dependent objects from a statement.
/// These are typically, tables and views, that are used in statements.
/// The collector does not apply the search recursively to the
/// found elements.
/// </summary>
public class SimpleCollector : ICollectorVisitorStorage
{
    private readonly IList<Statement> _statements;

    public SimpleCollector(IList<Statement> statements)
    {
        _statements = statements;
    }

    private readonly Dictionary<string, List<object>> _tableUsage = new();

    private readonly Dictionary<string, List<object>> _callStoredProc = new();
    private readonly Dictionary<string, List<object>> _createStoredProc = new();

    private readonly Dictionary<string, List<object>> _functionCalls = new();


    public void AddTableUsage(string tableName, object user)
    {
        if (!_tableUsage.ContainsKey(tableName))
            _tableUsage[tableName] = new();
        _tableUsage[tableName].Add(user);
    }

    public void AddStoredProcCallUsage(string storedProcName, object user)
    {
        if (!_callStoredProc.ContainsKey(storedProcName))
            _callStoredProc[storedProcName] = new();
        _callStoredProc[storedProcName].Add(user);
    }

    public void AddStoredProcCreateUsage(string storedProcName, object user)
    {
        if (!_createStoredProc.ContainsKey(storedProcName))
            _createStoredProc[storedProcName] = new();
        _createStoredProc[storedProcName].Add(user);
    }

    public void AddFunctionCall(string functionCallName, object expression)
    {
        if (!_functionCalls.ContainsKey(functionCallName))
            _functionCalls[functionCallName] = new();
        _functionCalls[functionCallName].Add(expression);
    }

    public void Collect()
    {
        var visitor = new ChildElementCollectorVisitor(this);
        visitor.Visit(_statements);
    }

    public IDictionary<string, List<object>> TableUsage => _tableUsage;
    public IDictionary<string, List<object>> StoredProcUsage => _callStoredProc;

    public IDictionary<string, List<object>> StoredProcCreate => _createStoredProc;

    public IDictionary<string, List<object>> FunctionCalls => _functionCalls;
}
