using NUnit.Framework;
using NUnit.Framework.Legacy;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;

namespace Parser.Tests;

public class Tests
{    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var lexer = new Lexer("SELECT * FROM MyTable");
        lexer.Scan();
        var lexems = lexer.Lexems;
        var tokensFromResult = from lexem in lexems
                               select lexem.Token;
        var lexemTypesFromResult = from lexem in lexems
                                select lexem.GetType();

        Assert.That(tokensFromResult, Is.EqualTo(new List<string>(["SELECT", "*", "FROM", "MyTable", "END"])).AsCollection  );
        Assert.That(lexemTypesFromResult, Is.EqualTo(new List<Type>([typeof(TSqlKeyword), typeof(OperatorSymbol), typeof(TSqlKeyword), typeof(LexerIdentifier), typeof(EndSymbol)])).AsCollection  );
    }
}
