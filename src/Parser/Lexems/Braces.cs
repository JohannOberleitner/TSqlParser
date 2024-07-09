
namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems
{
    /// <summary>
    /// Represents an LBrace "("
    /// </summary>
    public class LBrace : Lexem
    {
        /// <summary>
        /// Constructs an LBrace instance.
        /// </summary>
        public LBrace() { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns just LBRACE (</returns>
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
        /// <summary>
        /// Constructs an RBrace instance.
        /// </summary>
        public RBrace()
        {
        }

        /// <summary>
        /// Returns just RBRACE )
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Constructs an LBracket instance.
        /// </summary>
        public LBracket()
        {
        }

        /// <summary>
        /// Returns just [ )
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Constructs an RBracket instance.
        /// </summary>
        public RBracket()
        {
        }

        /// <summary>
        /// Returns just ]
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return " ] ";
        }
    }
}
