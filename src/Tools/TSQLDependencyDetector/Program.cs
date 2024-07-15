using System.IO;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Tools.DependencyDetector;

public class Program
{
  static void Main(string[] args)
  {
    Console.WriteLine("TSQL Dependency Detector");
    if (args.Length == 0)
    {
      PrintUsage();
      return;
    }
    else
    {
      var statements = ParseFromFile(args[0]);
      Console.WriteLine($"Statement Count: {statements.Count()}");
      PrintStatements(statements);
    }
  }

  private static IEnumerable<Statement> ParseFromFile(string filename)
  {
    var x = Environment.CurrentDirectory;

    if (!File.Exists(filename))
      throw new ArgumentException($"File not found: {filename}");

    var code = File.ReadAllText(filename);
    return Parse(code);
  }

  private static IEnumerable<Statement> Parse(string tsqlScriptCode)
  {
    var lexer = new Lexer(tsqlScriptCode);
    lexer.Scan();
    var parser = new Parser(lexer.Lexems);
    parser.Parse();
    return parser.Statements;
  }

  private static void PrintUsage()
  {
    Console.WriteLine("TSQLDependencyDetector");
    Console.WriteLine("  A tool to find dependencies in TSQL queries");
    Console.WriteLine();
    Console.WriteLine("Usage:");  
    Console.WriteLine("TSQLDependencyDetector.exe filename");
    Console.WriteLine("  filename is a file that includes TSQL code");
    Console.WriteLine();
    Console.WriteLine("Output: Dependencies in formatted form");
  }

  private static void PrintStatements(IEnumerable<Statement> statements)
  {
    foreach(var statement in statements)
      Console.WriteLine(statement);
  }
}