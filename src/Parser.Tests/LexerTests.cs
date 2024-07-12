using NUnit.Framework;
using NUnit.Framework.Legacy;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Tests;

public class Tests
{    [SetUp]
    public void Setup()
    {
    }

    private static IEnumerable<string> ConvertLexemsToPrintableStrings(Lexer lexer) 
    =>  from lexem in lexer.Lexems
                             select lexem.ToString();

    [Test]
    public void Test_SQLStatement()
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

    [Test]
    public void Test_SQLIdentifier()
    {
        var lexer = new Lexer("SELECT dbo.myschema.anotherid FROM Something");
        lexer.Scan();
        Assert.That(ConvertLexemsToPrintableStrings(lexer), 
            Is.EqualTo(new List<string>(["Keyword SELECT", "Id dbo", ".", "Id myschema", ".", "Id anotherid", "Keyword FROM", "Id Something", "END"])).AsCollection  );
    }

    [Test]
    public void Test_LineComment_1()
    {
        var lexer = new Lexer("SELECT 1 -- this is a comment");
        lexer.Scan();
        var lexems = lexer.Lexems;
        var readableLexems = from lexem in lexems
                             select lexem.ToString();
        Assert.That(readableLexems, Is.EqualTo(new List<string>(["Keyword SELECT", "INTEGER 1", "END"])).AsCollection  );
    }

    [Test]
    public void Test_LineComment_2()
    {
        var lexer = new Lexer("-- SELECT 1 \n SELECT 2");
        lexer.Scan();
        var lexems = lexer.Lexems;
        var readableLexems = from lexem in lexems
                             select lexem.ToString();
        Assert.That(readableLexems, Is.EqualTo(new List<string>(["Keyword SELECT", "INTEGER 2", "END"])).AsCollection  );
    }

    [Test]
    public void Test_BlockComment_1()
    {
        var lexer = new Lexer("SELECT 1 /* this is a comment */, 2");
        lexer.Scan();
        Assert.That(ConvertLexemsToPrintableStrings(lexer), 
            Is.EqualTo(new List<string>(["Keyword SELECT", "INTEGER 1", "SEPARATOR", "INTEGER 2", "END"])).AsCollection  );
    }
}
