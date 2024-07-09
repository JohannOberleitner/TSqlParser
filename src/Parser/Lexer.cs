using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser
{
    /// <summary>
    /// Simple lexer class for parsing TSQL
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>The lexer is initialized with an input string </item>
    ///     <item>The Scan function analyzes the input and stores the lexems that can be used by the parser.</item>
    ///     <item>The supported lexems (keywords and special characters) are created in the static constructor of the class. </item>
    ///     <item>The analyzed lexems can be found with the <see cref="Lexems"/> property.</item>
    /// </list>
    /// </remarks>
    public class Lexer
    {
        private readonly List<Lexem> _lexems = [];
        /// <summary>
        /// Returns a list of the lexems that the Lexer found.
        /// </summary>
        public IList<Lexem> Lexems => _lexems;

        /// <summary>
        /// Initializes the lexer with an input string
        /// </summary>
        /// <remarks>
        /// Instead of a string an input stream could have been used.
        /// String was chosen for simplicity.
        /// </remarks>
        /// <param name="input">Input string.</param>
        public Lexer(string input)
        {
            this._input = input;
        }

        private readonly string _input;

        private readonly static HashSet<string> _keywords = [
            "USE",
            "GO",
            "SET",
            "ALTER",
            "CREATE",
            "PROCEDURE",
            "PROC",
            "TRIGGER",
            "AS",
            "WITH",
            "DECLARE",
            "BEGIN",
            "END",
            "IF",
            "ELSE",
            "RETURN",
            "EXEC",
            "EXECUTE",
            "SELECT",
            "TOP",
            "FROM",
            "INNER",
            "LEFT",
            "RIGHT",
            "OUTER",
            "CROSS",
            "APPLY",
            "JOIN",
            "ON",
            "WHERE",
            "GROUP",
            "ORDER",
            "BY",
            "INSERT",
            "INTO",
            "VALUES",
            "CASE",
            "WHEN",
            "THEN",
            "EXISTS",
            "THROW",
            "RAISERROR",
            "TRY",
            "CATCH",
            "PRINT",

            "UPDATE",
            "DELETE",
            "UNION",
            "EXCEPT",
            "GOTO",
            "DROP",
            "TABLE",
            "INDEX",
            "WHILE",
            "BREAK",
            "CONTINUE",

            "CURSOR",
            "OPEN",
            "FOR",
            "FETCH",
            "CLOSE",
            "DEALLOCATE",

            "WAITFOR",
            "MERGE",
            "REVERT",
            "ROLLBACK",
            "COMMIT",
            "SAVE",
            "TRUNCATE",
            "GRANT"
        ];
        #region Temporary helper variables
        /// <summary>
        /// Current character position in the _input string.
        /// </summary>
        private int _pos;
        /// <summary>
        /// temporary buffer for the current lexems.
        /// </summary>
        private StringBuilder _buffer = new();
        /// <summary>
        /// Counts the number of open brackets (TSQL symbol '[') 
        /// that were opened in the current lexem. These brackets are used in
        /// identifiers. Then whitespace may be used, too.
        /// </summary>
        private int _bracketCount;
        /// <summary>
        ///  
        /// </summary>
        private bool _isUnicode;

        #endregion Temporary helper variables
        /// <summary>
        /// Scans the input string that was provided in the constructor.
        /// </summary>
        public void Scan()
        {
            _bracketCount = 0;
            _blockCommentNestingLevel = 0;
            _buffer = new StringBuilder();

            while (_pos < _input.Length)
            {
                char c = _input[_pos];
                _pos++;

                // this character is within brackets: add them to _buffer.
                if (_bracketCount > 0 && c != ']')
                {
                    _ = _buffer.Append(c);
                    continue;
                }

                // whitespaces typically ends a lexem
                // except within brackets [...]
                if (IsWhitespace(c))
                {
                    if (_bracketCount == 0)
                        CompleteLexem();
                    else
                        _buffer.Append(c);
                    continue;
                }

                // for line comments (--)
                // the parsing is skipped until the end of a line 
                // Remove the last lexem
                if (IsLineCommentStart(c))
                {
                    RemoveLastLexem(); // the last lexem was a - (subtraction), needs to be removed
                    SkipUntilEndOfLine();
                    continue;
                }

                // for a block comments (/* */), the parsing is skipped
                // until the end of the block comment.
                // Remove the last lexem
                if (IsBlockCommentStart(c))
                {
                    RemoveLastLexem(); // the last lexem was a / (division),, needs to be removed
                    SkipUntilEndOfBlockComment();
                    continue;
                }

                // if the current character starts with a string delimiter
                // (single quote: ' ) collect string until it ends.
                if (IsStringDelimiter(c))
                {
                    if (_buffer.Length == 1 && _buffer.ToString() == "N")
                    {
                        _buffer.Clear();
                        _isUnicode = true;
                        CollectStringUntilNextStringDelimiter();
                    }
                    else
                    {
                        CompleteLexem();
                        CollectStringUntilNextStringDelimiter();
                    }
                    continue;
                }

                // for double quote string delimiter collect string
                // until final string delimiter
                if (IsDoubleQuoteStringDelimiter(c))
                {
                    CompleteLexem();
                    CollectStringUntilNextDoubleQuoteStringDelimiter();
                    continue;
                }

                if (IsOperator(c))
                {
                    // if the operator is a dot (.) and there were characters 
                    // put in _buffer before, and this is an integer 
                    // then it is a float - and should be combined.
                    if (c == '.' && _buffer.Length > 0 && IsInteger(_buffer.ToString()))
                    {
                        _buffer.Append(c);
                        continue;
                    }

                    if (_pos < _input.Length)
                    {
                        char c2 = _input[_pos];
                        // consider 2 character-operators like "+=":
                        if (IsTwoCharOperator(c, c2))
                        {
                            CompleteLexem();
                            _lexems.Add(new OperatorSymbol($"{c}{c2}"));
                            _pos++;
                        }
                        else
                        {
                            CompleteLexem();
                            _lexems.Add(new OperatorSymbol(c.ToString()));
                        }
                    }
                    else // this else branch can happen only at the end of a script
                    {
                        CompleteLexem();
                        _lexems.Add(new OperatorSymbol(c.ToString()));
                    }

                    continue;
                }

                if (IsFieldSeparator(c))
                {
                    CompleteLexem();
                    _lexems.Add(new FieldSeparator());
                    continue;
                }

                if (IsLBrace(c))
                {
                    CompleteLexem();
                    _lexems.Add(new LBrace());
                    continue;
                }

                if (IsRBrace(c))
                {
                    CompleteLexem();
                    _lexems.Add(new RBrace());
                    continue;
                }

                // if the character is a character (from alpha-bet) add it to the _buffer
                // or if the character is a number
                // if the character is an underscore at it to the _buffer
                // or if the character is an at-symbol (@) at it to the _buffer.
                // or if the character is a hash-symbol (#) or a dollar symbol ($) at it to the _buffer.
                if (IsAlpha(c) ||
                    IsNumber(c) ||
                    IsUnderscore(c) ||
                    IsAtSymbol(c) ||
                    IsHashSymbol(c) ||
                    IsDollarSymbol(c) ||
                    IsDoubleQuote(c))
                {
                    _buffer.Append(c);
                    continue;
                }

                // if an LBracket appears and no bracket was open
                // then the lexem so far is completex and the count of open brackets
                // is increased by 1. 
                if (IsLBracket(c))
                {
                    if (_bracketCount == 0)
                        CompleteLexem();
                    _buffer.Append(c);
                    _bracketCount++;
                    continue;
                }

                if (IsRBracket(c))
                {
                    _buffer.Append(c);
                    _bracketCount--;
                    CompleteLexem();
                    continue;
                }

                if (IsColon(c))
                {
                    // if there were 2 colons consecutively, then this is a scope separator
                    // operator (::), otherwise a label.
                    if (_pos >= 2 && IsColon(_input[_pos - 2]))
                    {
                        _buffer.Remove(_buffer.Length - 1, 1);
                        CompleteLexem();
                        _lexems.Add(new LexerIdentifier("::"));
                    }
                    else
                    {
                        _buffer.Append(c);
                    }
                    continue;
                }

                if (IsSemicolon(c))
                {
                    CompleteLexem();
                    _lexems.Add(new Semicolon());
                    continue;
                }

                if (IsLCurly(c))
                {
                    CompleteLexem();
                    CollectStringUntilNextRCurly();
                    continue;
                }

                _lexems.Add(new UnknownChar(c));
            }
            CompleteLexem();

            _lexems.Add(new EndSymbol());
        }

        /// <summary>
        /// Returns the very last lexem that exists in the lexem container.
        /// </summary>
        /// <returns>The last lexem. If the container is empty it returns an UnknownChar object.</returns>
        private Lexem GetLastLexem()
        {
            if (_lexems.Count > 0)
                return _lexems[^1];
            else
                return new UnknownChar(' ');
        }

        /// <summary>
        /// Removes the last lexem in the container of lexems.
        /// This method assumes that there is at least 1 lexem in the container. 
        /// </summary>
        private void RemoveLastLexem()
        {
            _lexems.RemoveAt(_lexems.Count - 1);
        }

        /// <summary>
        /// Complete the current lexem and add it to the _lexems container.
        /// This function identifies the type of the lexem and creates the
        /// proper type.
        /// </summary>
        private void CompleteLexem()
        {
            // if the stringbuffer is empty there is no lexem
            if (_buffer.Length == 0)
                return;

            var s = _buffer.ToString();
            _buffer.Clear();
            if (IsKeyword(s))
                _lexems.Add(new TSqlKeyword(s));
            else if (IsOperator(s))
                _lexems.Add(new OperatorSymbol(s));
            else
            {
                if (IsInteger(s))
                    _lexems.Add(new IntegerLiteral(s));
                else if (IsFloat(s))
                    _lexems.Add(new FloatLiteral(s));
                else if (IsLabel(s))
                    _lexems.Add(new Label(s));
                else
                    _lexems.Add(new LexerIdentifier(s));
            }
        }

        static private bool IsLBrace(char c) => c == '(';

        static private bool IsRBrace(char c) => c == ')';

        static private bool IsFieldSeparator(char c) => c == ',';

        static private bool IsLBracket(char c) => c == '[';

        static private bool IsRBracket(char c) => c == ']';

        static private bool IsLCurly(char c) => c == '{';
        static private bool IsRCurly(char c) => c == '}';
        static private bool IsWhitespace(char c) => c == ' ' || c == '\t' || c == '\n' || c == '\r';

        static private bool IsStringDelimiter(char c) => c == '\'';

        static private bool IsDoubleQuoteStringDelimiter(char c) => c == '"';

        static private bool IsAlpha(char c) => char.IsLetter(c);

        static private bool IsNumber(char c) => char.IsNumber(c);

        static private bool IsUnderscore(char c) => c == '_';

        static private bool IsAtSymbol(char c) => c == '@';

        static private bool IsHashSymbol(char c) => c == '@';

        static private bool IsDollarSymbol(char c) => c == '$';

        static private bool IsSemicolon(char c) => c == ';';
        static private bool IsColon(char c) => c == ':';

        static private bool IsDoubleQuote(char c) => c == '"';

        static private bool IsKeyword(string s) => _keywords.Contains(s.ToUpperInvariant());

        static private bool IsOperator(char c)
        {
            return c == '-' || c == '+' || c == '*' || c == '/' || c == '%' || c == '.'
                || c == '=' || c == '>' || c == '<' || c == '=' || c == '!'
                || c == '&' || c == '~' || c == '|';
        }

        static private bool IsOperator(string s)
        {
            string s_upper = s.ToUpperInvariant();
            return s_upper == "IS" ||
                s_upper == "NOT" || s_upper == "AND" || s_upper == "OR" ||
                s_upper == "ALL" || s_upper == "ANY" || s_upper == "EXITS" || s_upper == "BETWEEN" ||
                s_upper == "EXITS" || s_upper == "IN" || s_upper == "LIKE" || s_upper == "SOME";
        }

        /// <summary>
        /// Returns if the two provided characters are an operator
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        static private bool IsTwoCharOperator(char c1, char c2)
        {
            string s = $"{c1}{c2}";

            return s == "+=" || s == "-=" || s == "*=" || s == "/=" || s == "%=" || s == "&=" || s == "^=" || s == "|="
                | s == ">=" || s == "<=" || s == "<>" || s == "!<" || s == "!=" || s == "!>";
        }

        static private bool IsInteger(string s)
        {
            foreach (var c in s)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        static private bool IsFloat(string s)
        {
            if (!double.TryParse(s, out double _))
                return false;

            return true;
        }

        /// <summary>
        /// Returns if the string is a label.
        /// A label has a colon (:) as last character.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>true if the string is a label</returns>
        static private bool IsLabel(string s) => s.Length > 2 && s[^1] == ':';

        private bool IsLineCommentStart(char c) => c == '-' && IsLastLexemOperator('-') && _buffer.Length == 0;

        private void SkipUntilEndOfLine()
        {
            while (_pos + 1 < _input.Length)
            {
                _pos++;
                char next = _input[_pos];
                if (next == '\r' || next == '\n')
                {
                    _pos++;
                    return;
                }
            }
            if (_pos + 1 == _input.Length) // to fix error by 1
            {
                _pos++;
            }
        }

        private bool IsBlockCommentStart(char c) => c == '*' && IsLastLexemOperator('/');

        private int _blockCommentNestingLevel;

        private void SkipUntilEndOfBlockComment()
        {
            char previous = ' ';
            _blockCommentNestingLevel = 1;

            while (_pos + 1 < _input.Length)
            {
                _pos++;
                char next = _input[_pos];
                if (previous == '/' && next == '*')
                {
                    _blockCommentNestingLevel++;
                }

                if (previous == '*' && next == '/')
                {
                    _blockCommentNestingLevel--;
                    if (_blockCommentNestingLevel == 0)
                    {
                        _pos++;
                        return;
                    }
                }
                previous = next;
            }
        }

        private void CollectStringUntilNextStringDelimiter()
        {
            while (_pos + 1 < _input.Length)
            {
                char next = _input[_pos];
                _pos++;
                if (IsStringDelimiter(next) && IsStringDelimiter(_input[_pos]))
                {
                    _buffer.Append(next);
                    _pos++;
                }
                else if (IsStringDelimiter(next))
                {
                    _lexems.Add(new StringLiteral(_buffer.ToString(), _isUnicode));
                    _buffer.Clear();
                    _isUnicode = true;
                    break;
                }
                _buffer.Append(next);
            }
            _isUnicode = false;
        }

        private void CollectStringUntilNextDoubleQuoteStringDelimiter()
        {
            while (_pos + 1 < _input.Length)
            {
                char next = _input[_pos];
                _pos++;
                if (IsDoubleQuoteStringDelimiter(next) && IsDoubleQuoteStringDelimiter(_input[_pos]))
                {
                    _buffer.Append(next);
                    _pos++;
                }
                else if (IsDoubleQuoteStringDelimiter(next))
                {
                    _lexems.Add(new LexerIdentifier(_buffer.ToString()));
                    _buffer.Clear();
                    _isUnicode = true;
                    break;
                }
                _buffer.Append(next);
            }
        }

        private void CollectStringUntilNextRCurly()
        {
            _buffer.Append('{');
            while (_pos + 1 < _input.Length)
            {
                char next = _input[_pos];
                _pos++;
                if (IsRCurly(next))
                {
                    _buffer.Append('}');
                    _lexems.Add(new StringLiteral(_buffer.ToString(), false));
                    _buffer.Clear();
                    _isUnicode = false;
                    break;
                }
                _buffer.Append(next);

            }
        }

        /// <summary>
        /// Checks if the last lexem in the already analyzed list of lexems is 
        /// an operator like the character provided.
        /// </summary>
        /// <param name="operatorSymbol"></param>
        /// <returns></returns>
        private bool IsLastLexemOperator(char operatorSymbol)
        {
            var lastLexem = GetLastLexem() as OperatorSymbol;
            return lastLexem != null && lastLexem.Symbol == operatorSymbol.ToString();
        }
    }
}
