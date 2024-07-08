
namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents an LBrace "("
    /// </summary>
    public class LBrace: Lexem
    {
        public LBrace()
        {
        }        

        public override string ToString()
        {
            return "LBRACE ( ";
        }
    }

    /// <summary>
    /// Represents an RBrace ")"
    /// </summary>
    public class RBrace : Lexem
    {
        public RBrace()
        {
        }

        public override string ToString()
        {
            return "RBRACE ) ";
        }
    }

    /// <summary>
    /// Represents an LBracket "["
    /// </summary>
    public class LBracket : Lexem
    {
        public LBracket()
        {
        }

        public override string ToString()
        {
            return " [ ";
        }
    }

    /// <summary>
    /// Represents an RBracket "]"
    /// </summary>
    public class RBracket : Lexem
    {
        public RBracket()
        {
        }

        public override string ToString()
        {
            return " ] ";
        }
    }
}
