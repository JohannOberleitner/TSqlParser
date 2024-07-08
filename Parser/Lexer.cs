using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;
using System;
using System.Collections.Generic;
using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser
{
    /// <summary>
    /// Simple lexer class for parsing TSQL
    /// </summary>
    /// <remarks>
    /// <list>
    ///     <item>The lexer is initialized with an input string </item>
    ///     <item>The Scan function analyzes the input and stores the lexems that can be used by the parser.
    ///     <item>The supported lexems (keywords and special characters) are created in the static constructor of the class. </item>
    ///     <item>The analyzed lexems can be found with the <see cref="Lexems"/> property.</items>
    /// </list>
    /// </remarks>
    public class Lexer
    {
        
        private IList<Lexem> _lexems = new List<Lexem>();
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

        private string _input;
        
        

        private static HashSet<string> _keywords = new HashSet<string>();

        static Lexer()
        {
            _keywords.Add("USE");
            _keywords.Add("GO");
            _keywords.Add("SET");
            _keywords.Add("ALTER");
            _keywords.Add("CREATE");
            _keywords.Add("PROCEDURE");
            _keywords.Add("PROC");
            _keywords.Add("TRIGGER");
            _keywords.Add("AS");
            _keywords.Add("WITH");
            _keywords.Add("DECLARE");
            _keywords.Add("BEGIN");
            _keywords.Add("END");
            _keywords.Add("IF");
            _keywords.Add("ELSE");
            _keywords.Add("RETURN");
            _keywords.Add("EXEC");
            _keywords.Add("EXECUTE");
            _keywords.Add("SELECT");
            _keywords.Add("TOP");
            _keywords.Add("FROM");
            _keywords.Add("INNER");
            _keywords.Add("LEFT");
            _keywords.Add("RIGHT");
            _keywords.Add("OUTER");
            _keywords.Add("CROSS");
            _keywords.Add("APPLY");
            _keywords.Add("JOIN");
            _keywords.Add("ON");
            _keywords.Add("WHERE");
            _keywords.Add("GROUP");
            _keywords.Add("ORDER");
            _keywords.Add("BY");
            _keywords.Add("INSERT");
            _keywords.Add("INTO");
            _keywords.Add("VALUES");
            _keywords.Add("CASE");
            _keywords.Add("WHEN");
            _keywords.Add("THEN");
            _keywords.Add("EXISTS");
            _keywords.Add("THROW");
            _keywords.Add("RAISERROR");
            _keywords.Add("TRY");
            _keywords.Add("CATCH");
            _keywords.Add("PRINT");

            _keywords.Add("UPDATE");
            _keywords.Add("DELETE");
            _keywords.Add("UNION");
            _keywords.Add("EXCEPT");
            _keywords.Add("GOTO");
            _keywords.Add("DROP");
            _keywords.Add("TABLE");
            _keywords.Add("INDEX");
            _keywords.Add("WHILE");
            _keywords.Add("BREAK");
            _keywords.Add("CONTINUE");

            _keywords.Add("CURSOR");
            _keywords.Add("OPEN");
            _keywords.Add("FOR");
            _keywords.Add("FETCH");
            _keywords.Add("CLOSE");
            _keywords.Add("DEALLOCATE");

            _keywords.Add("WAITFOR");
            _keywords.Add("MERGE");
            _keywords.Add("REVERT");
            _keywords.Add("ROLLBACK");
            _keywords.Add("COMMIT");
            _keywords.Add("SAVE");
            _keywords.Add("TRUNCATE");
            _keywords.Add("GRANT");
        }

        
    #region Temmporary helper variables
        /// <summary>
        /// Current character position in the _input string.
        /// </summary>
        private int _pos;
        /// <summary>
        /// temporary buffer for the current lexems.
        /// </summary>
        private StringBuilder _buffer = new StringBuilder();    
        /// <summary>
        /// Counts the number of open brackets (TSQL symbol '[') 
        /// that were opened in the current lexem. These brackets are used in
        /// identifiers. Then whitespace may be used, too.
        /// </summary>
        private int _bracketCount = 0;
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
            _buffer = new StringBuilder();
            while (_pos < _input.Length)
            {
                char c = _input[_pos];
                _pos++;

                // this character is within brackets: add them to _buffer.
                if (_bracketCount > 0 && c != ']')
                {
                    _buffer.Append(c);
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

                // for line comments (double forward slashes)
                // the parsing is skipped until the end of a line 
                // Remote the last lexem (TODO: check why I added this)
                if (IsLineCommentStart(c))
                {
                    RemoveLastLexem();
                    SkipUntilEndOfLine();
                    continue;
                }

                // for a block comments (/* */), the parsing is skipped
                // until the end of the block comment.
                // Remote the last lexem (TODO: check why)
                if (IsBlockCommentStart(c))
                {
                    RemoveLastLexem();
                    SkipUntilEndOfBlockComment();
                    continue;
                }

                // if the current character starts with a string delimiter
                // (single quote: ' ) collect string until it ends.
                if(IsStringDelimiter(c))
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
                    if (c == '.' && _buffer.Length > 0)
                    {
                        // special treatment for floats
                        if (IsInteger(_buffer.ToString()))
                        {
                            _buffer.Append(c);
                            continue;
                        }
                    }
                    
                    if (_pos < _input.Length)
                    {
                        char c2 = _input[_pos];
                        if (IsOperator(c,c2))
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
                    else
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

                if (IsAlpha(c))
                {
                    _buffer.Append(c);
                    continue;
                }

                if (IsUnderbrace(c))
                {
                    _buffer.Append(c);
                    continue;
                }

                if (IsAtSymbol(c))
                {
                    _buffer.Append(c);
                    continue;
                }

                if (IsHashSymbol(c) || IsDollarSymbol(c))
                {
                    _buffer.Append(c);
                    continue;
                }

                if (IsDoubleQuote(c))
                {
                    _buffer.Append(c);
                    continue;
                }

                if (IsNumber(c))
                {
                    _buffer.Append(c);
                    continue;
                }

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

        private Lexem GetLastLexem()
        {
            if (_lexems.Count > 0)
                return _lexems[_lexems.Count - 1];
            else
                return new UnknownChar(' ');
        }

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

        private bool IsLBrace(char c)
        {
            return c == '(';
        }

        private bool IsRBrace(char c)
        {
            return c == ')';
        }

        private bool IsFieldSeparator(char c)
        {
            return c == ',';
        }

        private bool IsBracket(char c)
        {
            return c == '[' || c == ']';
        }

        private bool IsLBracket(char c)
        {
            return c == '[';
        }

        private bool IsRBracket(char c)
        {
            return c == ']';
        }

        private bool IsLCurly(char c)
        {
            return c == '{';
        }
        private bool IsRCurly(char c)
        {
            return c == '}';
        }

        private bool IsWhitespace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n' || c == '\r';
        }

        private bool IsStringDelimiter(char c)
        {
            return c == '\'';
        }

        private bool IsDoubleQuoteStringDelimiter(char c)
        {
            return c == '"';
        }

        private bool IsAlpha(char c)
        {
            return char.IsLetter(c);
        }

        private bool IsNumber(char c)
        {
            return char.IsNumber(c);
        }

        private bool IsUnderbrace(char c)
        {
            return c == '_';
        }

        private bool IsAtSymbol(char c)
        {
            return c == '@';
        }

        private bool IsHashSymbol(char c)
        { 
            return c == '#';
        }

        private bool IsDollarSymbol(char c)
        {
            return c == '$';
        }

        private bool IsKeyword(string s)
        {
            return _keywords.Contains(s.ToUpperInvariant());
        }

        private bool IsOperator(char c)
        {
            return c == '-' || c == '+' || c == '*' || c == '/' || c == '%' || c == '.'
                || c == '=' || c == '>' || c == '<' || c == '=' || c == '!'
                || c == '&' || c== '~'  || c == '|';
        }

        private bool IsOperator(string s)
        {
            string s_upper = s.ToUpperInvariant();
            return s_upper == "IS" ||
                s_upper == "NOT" || s_upper == "AND" || s_upper == "OR" ||
                s_upper == "ALL" || s_upper == "ANY" || s_upper == "EXITS" || s_upper == "BETWEEN" ||
                s_upper == "EXITS" || s_upper == "IN" || s_upper == "LIKE" || s_upper == "SOME";
        }

        private bool IsOperator(char c1, char c2)
        {
            string s = $"{c1}{c2}";

            return s == "+=" || s == "-=" || s == "*=" || s == "/=" || s == "%=" || s == "&=" || s == "^=" || s == "|="
                | s == ">=" || s == "<=" || s == "<>" || s == "!<" || s == "!=" || s == "!>";
        }

        private bool IsSemicolon(char c)
        {
            return c == ';';
        }
        private bool IsColon(char c)
        {
            return c == ':';
        }




        private bool IsDoubleQuote(char c)
        {
            return c == '"';
        }

        private bool IsInteger(string s)
        {
            foreach(var c in s)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        private bool IsFloat(string s)
        {
            double d;

            if (!double.TryParse(s, out d))
                return false;

            return true;
        }

        /// <summary>
        /// Returns if the string is a label.
        /// A label has a colon (:) as last character.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>true if the string is a label</returns>
        private bool IsLabel(string s)
        {
            return s.Length > 2 && s[s.Length-1] == ':';
        }

        private bool IsLineCommentStart(char c)
        {
            return c == '-' && IsLastLexemOperator('-') && _buffer.Length == 0;
        }

        private void SkipUntilEndOfLine()
        {
            while (_pos +1 < _input.Length)
            {
                _pos++;
                char next = _input[_pos];
                if (next == '\r' || next == '\n')
                {
                    _pos++;
                    return;
                }
            }
            if (_pos+1 == _input.Length) // to fix error by 1
            {
                _pos++;
            }
        }

        private bool IsBlockCommentStart(char c)
        {
            return c == '*' && IsLastLexemOperator('/');
        }

        private int _blockCommentNestingLevel = 0;

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
            while (_pos+1 <_input.Length)
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

        private bool IsLastLexemOperator(char operatorSymbol)
        {
            var lastLexem = GetLastLexem() as OperatorSymbol;
            return lastLexem != null && lastLexem.Symbol == operatorSymbol.ToString();
        }
    }
}
