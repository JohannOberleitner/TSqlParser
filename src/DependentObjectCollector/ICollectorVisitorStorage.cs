

using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.DependentObjectCollector;

public interface ICollectorVisitorStorage 
{
    void AddTableUsage(string tableName, object user);
    void AddStoredProcCallUsage(string storedProcpName, object user);
    void AddStoredProcCreateUsage(string storedProcName, object user);

}