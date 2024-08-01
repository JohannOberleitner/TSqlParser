using NUnit.Framework;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Tests;

public class ParserTests
{    
  
    string _code14 = @"
            IF @i = 1 BEGIN
		          SET @testBinExpr = 2            
            END ELSE BEGIN
              SET @i = 3
            END
        ";

    string _code77 = @"
            select sum(
				case
					when eit.fIsNegative = 0
					then ei.fMoney
					else (-ei.fMoney)
				end
				) as LossAmount, 1 
             from mytable";

        string _code79 = @"
            select 	
	            a	
	            from table1 	            
	            left join table2 tab2

		            inner join table3 as tab3 
		            on tab3.id=tab2.id 
		
	            on tab2.id = table1.id

	            inner join table4 tab4
	            on tab4.id = table1.id

	            where 1=1
        ";

        string _code102 = @"
              insert into dbo.table1(a,b) 	             
              select x,y from dbo.table2
        ";

        string _code103 = @"
              select myFunc(x),y from dbo.myTable
        ";

        string _code104 = @"
              create procedure MyStoredProc
              (
                @fid INT NOT NULL,
                @atext VARCHAR(20)
              )
              AS
              BEGIN
                SELECT 
                  d
                FROM 
                  MyTable
                WHERE
                  id = fid
              END
        ";

    private Lexer? _lexer;
    private Parser? _parser;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test_Parser_14()
    {
        _lexer = new Lexer(_code14);
        _lexer.Scan();
        _parser = new Parser(_lexer.Lexems);
        _parser.Parse();

        foreach (var s in _parser.Statements)
          System.Console.Out.WriteLine(s);

        Assert.That(_parser.Statements.Count == 1);
    }

    [Test]
    public void Test_Parser_77()
    {
        _lexer = new Lexer(_code77);
        _lexer.Scan();
        _parser = new Parser(_lexer.Lexems);
        _parser.Parse();

        foreach (var s in _parser.Statements)
          System.Console.Out.WriteLine(s);

        Assert.That(_parser.Statements.Count == 1);
    }

    [Test]
    public void Test_Parser_79()
    {
        _lexer = new Lexer(_code79);
        _lexer.Scan();
        _parser = new Parser(_lexer.Lexems);
        _parser.Parse();

        foreach (var s in _parser.Statements)
          System.Console.Out.WriteLine(s);

        Assert.That(_parser.Statements.Count == 1);
    }

    [Test]
    public void Test_Parser_102()
    {
        _lexer = new Lexer(_code102);
        _lexer.Scan();
        _parser = new Parser(_lexer.Lexems);
        _parser.Parse();

        foreach (var s in _parser.Statements)
          System.Console.Out.WriteLine(s);

        Assert.That(_parser.Statements.Count == 1);
    }

    [Test]
    public void Test_Parser_103()
    {
        _lexer = new Lexer(_code103);
        _lexer.Scan();
        _parser = new Parser(_lexer.Lexems);
        _parser.Parse();

        foreach (var s in _parser.Statements)
          System.Console.Out.WriteLine(s);

        Assert.That(_parser.Statements.Count == 1);
    }

    [Test]
    public void Test_Parser_104()
    {
        _lexer = new Lexer(_code104);
        _lexer.Scan();
        _parser = new Parser(_lexer.Lexems);
        _parser.Parse();

        foreach (var s in _parser.Statements)
          System.Console.Out.WriteLine(s);

        Assert.That(_parser.Statements.Count == 1);
    }
}