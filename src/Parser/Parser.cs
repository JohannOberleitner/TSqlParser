using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Extensions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Lexems;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions.CaseClauseDataStructures;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Identifiers;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.GroupByClauseDataStructures;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.InsertDataStructures;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.OrderByClauseDataStructures;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.SelectClauseDataStructures;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.SetStatementRules;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser
{
    /// <summary>
    /// Main class of the TSQL Parser.
    /// </summary>
    public class Parser
    {
        private readonly IList<Lexem> _lexems;
        private int _pos;
        private readonly IList<Statement> _topStatements = new List<Statement>();

        private readonly IDictionary<string, TCreateStatementAction> _keywordActions;
        private readonly IDictionary<string, SetStatementRule> _setStatementGrammarRules;
        private readonly ISet<string> _tableHintsNotRequiringWith;

        private readonly Stack<IList<Statement>> _statementsStack = new();

        /// <summary>
        /// Initializes the parser with a list of lexems.
        /// Normally, these list was created by the Lexer.
        /// </summary>
        /// <param name="lexems"></param>
        public Parser(IList<Lexem> lexems)
        {
            _keywordActions = new Dictionary<string, TCreateStatementAction>();
            _setStatementGrammarRules = new Dictionary<string, SetStatementRule>();
            _tableHintsNotRequiringWith = new HashSet<string>();

            InitializeActions();
            InitializeSetGrammar();
            InitializeTableHintsNotRequiringWith();
            _lexems = lexems;
            _pos = 0;
        }

        private void InitializeActions()
        {
            _keywordActions["USE"] = AddUseStatement;
            _keywordActions["GO"] = AddGoStatement;
            _keywordActions["SET"] = AddSetStatement;
            _keywordActions["ALTER"] = AddAlterStatement;
            _keywordActions["CREATE"] = AddCreateStatement;
            _keywordActions["BEGIN"] = AddBeginStatement;
            _keywordActions["DECLARE"] = AddDeclareStatement;
            _keywordActions["IF"] = AddIfStatement;
            _keywordActions["RETURN"] = AddReturnStatement;
            _keywordActions["INSERT"] = AddInsertStatement;
            _keywordActions["SELECT"] = AddQueryStatement;
            _keywordActions["THROW"] = AddThrowStatement;
            _keywordActions["RAISERROR"] = AddRaiseErrorStatement;
            _keywordActions["EXEC"] = AddExecStatement;
            _keywordActions["EXECUTE"] = AddExecStatement;
            _keywordActions["UPDATE"] = AddUpdateStatement;
            _keywordActions["DELETE"] = AddDeleteStatement;
            _keywordActions["PRINT"] = AddPrintStatement;
            _keywordActions["GOTO"] = AddGotoStatement;
            _keywordActions["DROP"] = AddDropStatement;
            _keywordActions["WHILE"] = AddWhileStatement;
            _keywordActions["BREAK"] = AddBreakStatement;
            _keywordActions["CONTINUE"] = AddContinueStatement;

            _keywordActions["OPEN"] = AddOpenStatement;
            _keywordActions["CLOSE"] = AddCloseStatement;
            _keywordActions["DEALLOCATE"] = AddDeallocateStatement;
            _keywordActions["FETCH"] = AddFetchStatement;

            _keywordActions["WAITFOR"] = AddWaitForStatement;
            _keywordActions["MERGE"] = AddMergeStatement;
            _keywordActions["WITH"] = AddWithStatement;
            _keywordActions["REVERT"] = AddRevertStatement;
            _keywordActions["COMMIT"] = AddCommitStatement;
            _keywordActions["ROLLBACK"] = AddRollbackStatement;
            _keywordActions["SAVE"] = AddSaveStatement;
            _keywordActions["TRUNCATE"] = AddTruncateStatement;
            _keywordActions["GRANT"] = AddGrantStatement;
        }

        private void InitializeSetGrammar()
        {
            _setStatementGrammarRules["ANSI_NULLS"] = new SetStatementRule("ANSI_NULLS", new MandatoryAlternativesPositionRule("ON", "OFF"));
            _setStatementGrammarRules["ANSI_PADDING"] = new SetStatementRule("ANSI_PADDING", new MandatoryAlternativesPositionRule("ON", "OFF"));
            _setStatementGrammarRules["CONCAT_NULL_YIELDS_NULL"] = new SetStatementRule("CONCAT_NULL_YIELDS_NULL", new MandatoryAlternativesPositionRule("ON", "OFF"));
            _setStatementGrammarRules["ANSI_WARNINGS"] = new SetStatementRule("ANSI_WARNINGS", new MandatoryAlternativesPositionRule("ON", "OFF"));
            _setStatementGrammarRules["NUMERIC_ROUNDABORT"] = new SetStatementRule("NUMERIC_ROUNDABORT", new MandatoryAlternativesPositionRule("ON", "OFF"));
            _setStatementGrammarRules["ARITHABORT"] = new SetStatementRule("ARITHABORT", new MandatoryAlternativesPositionRule("ON", "OFF"));
            _setStatementGrammarRules["NOCOUNT"] = new SetStatementRule("NOCOUNT", new MandatoryAlternativesPositionRule("ON", "OFF"));
            _setStatementGrammarRules["QUOTED_IDENTIFIER"] = new SetStatementRule("QUOTED_IDENTIFIER", new MandatoryAlternativesPositionRule("ON", "OFF"));
            _setStatementGrammarRules["TRANSACTION"] = new SetStatementRule("TRANSACTION ISOLATION LEVEL",
                                                            new MandatoryAlternativesPositionRule(
                                                                "READ UNCOMMITTED",
                                                                "READ COMMITTED",
                                                                "REPEATBLE READ",
                                                                "SNAPSHOT",
                                                                "SERIALIZABLE"));
            _setStatementGrammarRules["TRAN"] = new SetStatementRule("TRAN ISOLATION LEVEL",
                                                            new MandatoryAlternativesPositionRule(
                                                                "READ UNCOMMITTED",
                                                                "READ COMMITTED",
                                                                "REPEATBLE READ",
                                                                "SNAPSHOT",
                                                                "SERIALIZABLE"));
            _setStatementGrammarRules["STATISTICS"] = new SetStatementRule("STATISTICS",
                                                            new MandatoryAlternativesPositionRule(
                                                                "TIME ON",
                                                                "TIME OFF",
                                                                "IO ON",
                                                                "IO OFF"));

            // _setStatementGrammarRules["ROWCOUNT"] = new SetStatementRule("ROWCOUNT", new MandatoryExpressionRule());
        }

        private void InitializeTableHintsNotRequiringWith()
        {
            _tableHintsNotRequiringWith.Add("NOLOCK");
            _tableHintsNotRequiringWith.Add("READUNCOMMITTED");
            _tableHintsNotRequiringWith.Add("UPDLOCK");
            _tableHintsNotRequiringWith.Add("REPEATABLEREAD");
            _tableHintsNotRequiringWith.Add("SERIALIZABLE");
            _tableHintsNotRequiringWith.Add("READCOMMITTED");
            _tableHintsNotRequiringWith.Add("TABLOCK");
            _tableHintsNotRequiringWith.Add("TABLOCKX");
            _tableHintsNotRequiringWith.Add("PAGLOCK");
            _tableHintsNotRequiringWith.Add("ROWLOCK");
            _tableHintsNotRequiringWith.Add("NOWAIT");
            _tableHintsNotRequiringWith.Add("READPAST");
            _tableHintsNotRequiringWith.Add("XLOCK");
            _tableHintsNotRequiringWith.Add("SNAPSHOT");
            _tableHintsNotRequiringWith.Add("NOEXPAND");
        }

        private delegate void TCreateStatementAction();


        /// <summary>
        /// Parse the provided list of lexems.
        /// </summary>
        public void Parse()
        {
            try
            {
                _statementsStack.Push(this._topStatements);

                while (_pos < _lexems.Count)
                {
                    ParseStatement();
                    if (_pos == _lexems.Count - 1 && _lexems[_pos] is EndSymbol)
                        break;

                    if (GetLastStatement() is UnrecognizedStatement)
                        break;

                }
            }
            catch (Exception ex)
            {
                /*for (int i = Math.Max(0, _statementsStack.Peek().Count - 5); i <= Math.Max(0, _statementsStack.Peek().Count - 1); ++i)
                {
                    Console.WriteLine(_statementsStack.Peek()[i]);
                }*/

                Console.WriteLine(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// This list of statements contains the result of the parsing process.
        /// They can be picked up after the parsing.
        /// </summary>
        public IList<Statement> Statements => _topStatements;
        /*
        private Lexem GetNextLexem()
        {
            AdvancedLexemPointer();
            var lexem = _lexems[_pos];
            return lexem;
        }
        */
        private Statement GetLastStatement()
        {
            var topStatementList = _statementsStack.Peek();
            return topStatementList[topStatementList.Count - 1];
        }
        private void AddStatement(Statement statement)
        {
            _statementsStack.Peek().Add(statement);
        }

        private void AdvancedLexemPointer()
        {
            if (_pos < _lexems.Count)
                _pos++;
        }

        private void ParseStatement()
        {
            Lexem next = _lexems[_pos];
            if (next is TSqlKeyword keyword)
            {
                string key = keyword.Keyword.ToUpperInvariant();
                if (_keywordActions.TryGetValue(key, out TCreateStatementAction? action))
                {
                    action();
                }
                else
                {
                    AddUnrecognizedStatement(next);
                    AdvancedLexemPointer();
                }

            }
            else if (next is Semicolon)
            {
                AdvancedLexemPointer();
            }
            else if (next is Label)
            {
                AddStatement(new LabelStatement((Label)next));
                AdvancedLexemPointer();
            }
            else if (next is EndSymbol && _pos == _lexems.Count - 1)
            {
                // ok, is at the end                
            }
            else if (next is LBrace)
            {
                AdvancedLexemPointer();
                ParseStatement();
                ExpectLexem(typeof(RBrace));
                AdvancedLexemPointer();
            }
            else
            {
                AddUnrecognizedStatement(next);
                AdvancedLexemPointer();
            }
        }

        private Identifier CollectKeywordIdentifier()
        {
            if (_lexems[_pos] is TSqlKeyword)
                throw new ParserException($"Lexem must be a keyword but was {_lexems[_pos]}");

            //AdvancedLexemPointer();
            //return new SimpleIdentifier(current);
            return CollectIdentifier();
        }

        private Identifier CollectIdentifier()
        {
            List<Lexem> lexerIdentifiers = new();

            while (_pos < _lexems.Count)
            {
                bool ignoreRequiredDotAtEnd = false;
                var current = _lexems[_pos] as LexerIdentifier;
                if (current == null)
                {
                    var currentOther = _lexems[_pos];
                    if (currentOther != null && currentOther.TokenEquals("TABLE", "CURSOR"))
                    {
                        lexerIdentifiers.Add(currentOther);
                    }
                    else if (currentOther is StringLiteral)
                    {
                        lexerIdentifiers.Add(currentOther);
                    }
                    /*else if (currentOther is OperatorSymbol && ((OperatorSymbol)currentOther).IsScopeResolutionSymbol)
                    {
                        lexerIdentifiers.Add(currentOther);
                    }*/
                    else if (currentOther is OperatorSymbol && ((OperatorSymbol)currentOther).Symbol == "*")
                    {
                        lexerIdentifiers.Add(currentOther);
                    }
                    else if (currentOther is OperatorSymbol && currentOther.Token == ".")
                    {
                        lexerIdentifiers.Add(currentOther);
                    }
                    else if (currentOther is TSqlKeyword)
                    {
                        lexerIdentifiers.Add(currentOther);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (current.Token == "::")
                    {
                        // TODO: encapsulate the previous lexerIdentifier in a ScopeClause
                        lexerIdentifiers.Add(current);
                        ignoreRequiredDotAtEnd = true;
                    }
                    else
                    {
                        lexerIdentifiers.Add(current);
                    }
                }

                AdvancedLexemPointer();
                if (_pos < _lexems.Count)
                {
                    var next = _lexems[_pos];
                    if (next.TokenEquals("::"))
                    {
                        ignoreRequiredDotAtEnd = true;
                    }

                    if (ignoreRequiredDotAtEnd)
                    {
                        // had a :: before
                    }
                    else
                    {
                        if (!_lexems[_pos].CheckWithOperationSymbol("."))
                        {
                            break;
                        }
                        AdvancedLexemPointer();
                    }
                }
            }

            if (lexerIdentifiers.Count == 0)
            {
                throw new ParserException("Invalid identifier:" + _lexems[Math.Max(0, _pos - 5)] + "," + _lexems[Math.Max(0, _pos - 4)] + _lexems[Math.Max(0, _pos - 3)] + _lexems[Math.Max(0, _pos - 2)] + _lexems[Math.Max(0, _pos - 1)]);
            }

            if (lexerIdentifiers.Count == 1)
                return new SimpleIdentifier(lexerIdentifiers[0]);
            else
                return new CombinedIdentifier(lexerIdentifiers);
        }

        private Expression CollectColumnExpression()
        {
            if (_lexems[_pos].CheckWithOperationSymbol("*"))
            {
                var expression = new IdentifierExpression(new WildcardColumnIdentifier());
                AdvancedLexemPointer();
                return expression;
            }
            else
            {
                var next = _lexems[_pos];
                if (next.TokenEquals("DISTINCT", "ALL"))
                {
                    // TODO: store DISTINCT/ALL somewhere
                    AdvancedLexemPointer();
                }

                return CollectExpression();
            }
        }

        private Expression CollectExpression()
        {
            var expr = CollectComparisonExpression();
            var next = _lexems[_pos];
            while (next.TokenEquals("AT"))
            {
                ExpectToken("AT");
                ExpectToken("TIME");
                ExpectToken("ZONE");
                // TODO var timeZoneExpression = CollectExpression();
                next = _lexems[_pos];
                // TODO: Add AT TIMEZOEN
            }
            if (next.TokenEquals("COLLATE"))
            {
                ExpectToken("COLLATE");
                // TODO var collateExpression = CollectExpression();
                // TODO: store collation
            }
            return expr;
        }

        private Expression CollectComparisonExpression()
        {
            var expr = CollectAdditionExpression();
            var next = _lexems[_pos];
            while (next is OperatorSymbol && (((OperatorSymbol)next).IsComparisonSymbol || ((OperatorSymbol)next).IsAssignmentSymbol))
            {
                AdvancedLexemPointer();
                var right = CollectAdditionExpression();
                expr = new BinaryOperatorExpression(expr, right, (OperatorSymbol)next);
                next = _lexems[_pos];
            }
            return expr;
        }

        private Expression CollectAdditionExpression()
        {
            var expr = CollectMultiplicationExpression();
            var next = _lexems[_pos];
            while (next is OperatorSymbol && ((OperatorSymbol)next).IsAdditionSymbol)
            {
                AdvancedLexemPointer();
                var right = CollectMultiplicationExpression();
                expr = new BinaryOperatorExpression(expr, right, (OperatorSymbol)next);
                next = _lexems[_pos];
            }
            return expr;
        }

        private Expression CollectMultiplicationExpression()
        {
            var expr = CollectTopPrecedenceExpression();
            var next = _lexems[_pos];
            while (next.IsMultiplicationSymbol &&
                   !_lexems[_pos + 1].TokenEquals("FROM"))
            {
                AdvancedLexemPointer();
                var right = CollectTopPrecedenceExpression();
                expr = new BinaryOperatorExpression(expr, right, (OperatorSymbol)next);
                next = _lexems[_pos];
            }
            return expr;
        }

        private Expression CollectTopPrecedenceExpression()
        {
            if (_lexems[_pos].CheckWithOperationSymbol("~"))
            {
                var operatorSymbol = (OperatorSymbol)_lexems[_pos];
                AdvancedLexemPointer();
                var inner = CollectTopPrecedenceExpression();
                var expr = new UnaryOperatorExpression(inner, operatorSymbol);
                return expr;
            }
            else
            {
                var expr = CollectPrimaryExpression();
                return expr;
            }
        }

        private Expression CollectPrimaryExpression()
        {
            Expression? expr = null;
            if (_lexems[_pos] is IntegerLiteral)
            {
                expr = new IntegerExpression((IntegerLiteral)_lexems[_pos]);
                AdvancedLexemPointer();
            }
            else if (_lexems[_pos] is StringLiteral)
            {
                expr = new StringExpression((StringLiteral)_lexems[_pos]);
                AdvancedLexemPointer();
            }
            else if (_lexems[_pos] is FloatLiteral)
            {
                expr = new FloatExpression((FloatLiteral)_lexems[_pos]);
                AdvancedLexemPointer();
            }
            else if (_lexems[_pos] is LexerIdentifier)
            {
                var identifier = CollectIdentifier();
                var next = _lexems[_pos];
                if (next is LBrace)
                {
                    expr = ParseFunctionCallExpression(identifier);
                }
                else if (identifier.IsEqual("NEXT"))
                {
                    expr = ParseNextValueForFunctionCallExpression(identifier);
                }
                else
                {
                    expr = new IdentifierExpression(identifier);
                }
            }
            else if (_lexems[_pos] is OperatorSymbol symbol)
            {
                AdvancedLexemPointer();
                var inner = CollectExpression();

                expr = new UnaryOperatorExpression(inner, symbol);
            }
            else if (_lexems[_pos] is LBrace)
            {
                expr = ParseSubExpression();
            }
            else if (_lexems[_pos] is TSqlKeyword && _lexems[_pos].TokenEquals("CASE"))
            {
                expr = ParseCaseExpression();
            }
            else if (_lexems[_pos].TokenEquals("EXISTS"))
            {
                expr = CollectPredicate();
            }
            else if (_lexems[_pos].TokenEquals("NOT"))
            {
                expr = CollectPredicate();
            }
            else if (_lexems[_pos].TokenEquals("VALUES"))
            {
                expr = ParseValuesFunctionCallExpression();
            }
            else if (_lexems[_pos] is TSqlKeyword)
            {
                var identifier = CollectKeywordIdentifier();
                var next = _lexems[_pos];
                if (next is LBrace)
                {
                    expr = ParseFunctionCallExpression(identifier);
                }
                else if (identifier.IsEqual("NEXT"))
                {
                    expr = ParseNextValueForFunctionCallExpression(identifier);
                }
                else
                {
                    if (identifier != null)
                        expr = new IdentifierExpression(identifier);

                    //throw new Exception($"No LBRACE after {identifier}");
                }
            }

            if (expr == null)
                throw new ParserException("CollectPrimaryExpression returns with 'null' expression");

            return expr!;
        }

        private Expression CollectSearchCondition()
        {
            return CollectSearchConditionOr();
        }

        private Expression CollectSearchConditionOr()
        {
            var expr = CollectSearchConditionAnd();
            var next = _lexems[_pos];
            while (next.CheckWithOperationSymbol("OR"))
            {
                AdvancedLexemPointer();
                var right = CollectSearchConditionAnd();
                expr = new BinaryOperatorExpression(expr, right, (OperatorSymbol)next);
                next = _lexems[_pos];
            }
            return expr;
        }

        private Expression CollectSearchConditionAnd()
        {
            var expr = CollectSearchConditionNot();
            var next = _lexems[_pos];
            while (next.CheckWithOperationSymbol("AND"))
            {
                AdvancedLexemPointer();
                var right = CollectSearchConditionNot();
                expr = new BinaryOperatorExpression(expr, right, (OperatorSymbol)next);
                next = _lexems[_pos];
            }
            return expr;
        }

        private Expression CollectSearchConditionNot()
        {
            var next = _lexems[_pos];
            if (next.CheckWithOperationSymbol("NOT"))
            {
                AdvancedLexemPointer();
                var inner = CollectSearchConditionNot();
                return new UnaryOperatorExpression(inner, (OperatorSymbol)next);
            }
            else
            {
                return CollectPredicate();
            }
        }

        private Expression CollectPredicate()
        {
            Expression? expr;
            var next = _lexems[_pos];
            if (next is LBrace)         // '(' search_condition ')'
            {
                AdvancedLexemPointer();
                next = _lexems[_pos];
                if (next.TokenEquals("SELECT"))
                {
                    _pos--;
                    var subQuery = ParseSubExpression();
                    expr = subQuery;
                }
                else
                {
                    var inner = CollectSearchCondition();
                    ExpectLexem(typeof(RBrace));
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                    expr = new SubExpression(inner);
                    // TODO: check here
                    if (next is OperatorSymbol)
                    {
                        if (next.TokenEquals("AND", "OR", "NOT"))
                        {
                            // do nothing
                        }
                        else
                        {
                            var currentOperator = (OperatorSymbol)next;
                            AdvancedLexemPointer();
                            var rightExpression = CollectExpression();
                            expr = new BinaryOperatorExpression(expr, rightExpression, currentOperator);
                        }
                    }
                }
            }
            else if (next is TSqlKeyword && next.TokenEquals("EXISTS"))
            {
                AdvancedLexemPointer();
                ExpectLexem(typeof(LBrace));
                expr = ParseSubExpression();
            }
            else
            {
                expr = CollectExpression();
            }
            next = _lexems[_pos];
            if (next.IsComparisonSymbol)
            {
                // TODO: ALL/SOME/ANY '(' subquery ')'

                AdvancedLexemPointer();
                var right = CollectExpression();
                return new BinaryOperatorExpression(expr, right, (OperatorSymbol)next);
            }
            else if (next.IsMultiplicationSymbol)
            {
                AdvancedLexemPointer();
                var right = CollectExpression();
                return new BinaryOperatorExpression(expr, right, (OperatorSymbol)next);
            }
            else if (next.CheckWithOperationSymbol("NOT"))
            {
                AdvancedLexemPointer();
                expr = ParseSubPredicate(expr, true);

            }
            else if (next.CheckWithOperationSymbols("IN", "LIKE"))
            {
                expr = ParseSubPredicate(expr, false);
            }
            else if (next.CheckWithOperationSymbol("IS"))
            {
                AdvancedLexemPointer();
                var next2 = _lexems[_pos];
                bool isNot = false;
                if (next2.CheckWithOperationSymbol("NOT"))
                {
                    AdvancedLexemPointer();
                    isNot = true;
                    next2 = _lexems[_pos];
                }

                if (next2.TokenEquals("NULL"))
                {
                    expr = new PostfixUnaryOperatorExpression(expr, new SimpleIdentifier((LexerIdentifier)next2), isNot);
                    AdvancedLexemPointer();
                }
            }
            else if (next.TokenEquals("BETWEEN"))
            {
                expr = ParseSubPredicate(expr, false);
            }

            if (expr == null)
                throw new ParserException("CollectPredicate returns with 'null' expression");

            return expr;
        }

        private Expression ParseSubPredicate(Expression subject, bool isNot)
        {
            var next = _lexems[_pos];
            Expression? expr;
            if (next.CheckWithOperationSymbol("IN"))
            {
                expr = ParseInExpression(subject, isNot);
                // TODO: BETWWEEN, IN, LIKE
            }
            else if (next.CheckWithOperationSymbol("LIKE"))
            {
                expr = ParseLikeExpression(subject, isNot);
            }
            else if (next.TokenEquals("BETWEEN"))
            {
                ExpectToken("BETWEEN");
                var lowerBoundary = CollectExpression();
                ExpectToken("AND");
                var upperBoundary = CollectExpression();
                expr = new BetweenOperatorExpression(subject, lowerBoundary, upperBoundary);
            }
            else
            {
                throw new ParserException($"Unidentified lexem {next.Token}");
            }
            return expr;
        }

        private Expression ParseInExpression(Expression subject, bool isNot)
        {
            // var next = _lexems[_pos];
            ExpectLexem(typeof(OperatorSymbol), "IN");
            IList<Expression> items = new List<Expression>();

            AdvancedLexemPointer();
            ExpectLexem(typeof(LBrace));
            var next = _lexems[_pos + 1];
            if (next is TSqlKeyword && next.TokenEquals("SELECT"))
            {
                var item = ParseSubExpression();
                items.Add(item);
            }
            else
            {
                AdvancedLexemPointer();
                next = _lexems[_pos];
                while (next is not RBrace)
                {
                    var item = CollectExpression();
                    items.Add(item);
                    next = _lexems[_pos];
                    if (next is FieldSeparator)
                    {
                        AdvancedLexemPointer();
                    }
                }
                ExpectLexem(typeof(RBrace));
                AdvancedLexemPointer();
            }

            return new InExpression(subject, items, isNot);
        }

        private Expression ParseLikeExpression(Expression subject, bool isNot)
        {
            // var next = _lexems[_pos];
            ExpectLexem(typeof(OperatorSymbol), "LIKE");
            AdvancedLexemPointer();

            var comparisionExpression = CollectExpression();
            if (CheckToken("ESCAPE"))
            {
                ExpectToken("ESCAPE");
                // next = _lexems[_pos];
                AdvancedLexemPointer();
            }

            return new LikeExpression(subject, comparisionExpression, isNot);
        }

        private Expression CollectCursorExpression()
        {
            ExpectToken("CURSOR");
            ExpectOptionalStatement("FORWARD_ONLY");
            ExpectOptionalStatement("SCROLL");
            ExpectOptionalStatement("STATIC");
            ExpectOptionalStatement("KEYSET");
            ExpectOptionalStatement("DYNAMIC");
            ExpectOptionalStatement("FAST_FORWARD");
            ExpectOptionalStatement("READ_ONLY");
            ExpectOptionalStatement("SCROLL_LOCKS");
            ExpectOptionalStatement("OPTIMISTIC");
            ExpectOptionalStatement("TYPE_WARNING");

            ExpectToken("FOR");
            var query = ParseSubExpression();
            return new CursorExpression(query);
        }


        /*private Expression CollectOrExpression()
        {
            var expr = CollectAndExpression();
            var next = _lexems[_pos];
            while (next is OperatorSymbol && ((OperatorSymbol)next).Symbol.ToUpperInvariant() == "OR")
            {
                AdvancedLexemPointer();
                var right = CollectAndExpression();
                expr = new BinaryOperatorExpression(expr, right, (OperatorSymbol)next);
                next = _lexems[_pos];
            }
            return expr;
        }

        private Expression CollectAndExpression()
        {
            var expr = CollectNotExpression();
            var next = _lexems[_pos];
            while (next is OperatorSymbol && ((OperatorSymbol)next).Symbol.ToUpperInvariant() == "AND")
            {
                AdvancedLexemPointer();
                var right = CollectNotExpression();
                expr = new BinaryOperatorExpression(expr, right, (OperatorSymbol)next);
                next = _lexems[_pos];
            }
            return expr;
        }

        private Expression CollectNotExpression()
        {
            var expr = CollectComparisonExpression();
            var next = _lexems[_pos];
            while (next is OperatorSymbol && ((OperatorSymbol)next).Symbol.ToUpperInvariant() == "NOT")
            {
                AdvancedLexemPointer();
                var right = CollectComparisonExpression();
                expr = new UnaryOperatorExpression(expr, (OperatorSymbol)next);
                next = _lexems[_pos];
            }
            return expr;
        }*/



        /*private Expression CollectPredicate()
        {

        }*/

        private Expression ParseSubExpression()
        {
            var next = _lexems[_pos];
            bool hasBraces = false;
            if (next is LBrace)
            {
                hasBraces = true;
                AdvancedLexemPointer();
            }
            next = _lexems[_pos];
            if (next.KeywordEquals("SELECT"))
            {
                _statementsStack.Push(new List<Statement>());
                ParseStatement();
                var subQueryStatements = _statementsStack.Pop();
                if (hasBraces)
                {
                    ExpectLexem(typeof(RBrace));
                    AdvancedLexemPointer();
                }
                return new SubExpression(new QueryExpression((QueryStatement)subQueryStatements[0]));
            }
            else
            {
                var expr = CollectSearchCondition();
                if (hasBraces)
                {
                    ExpectLexem(typeof(RBrace));
                    AdvancedLexemPointer();
                }
                return new SubExpression(expr);
            }
        }

        private Expression ParseCaseExpression()
        {
            IList<CaseClause> clauses = new List<CaseClause>();
            Expression? elseClause = null;

            ExpectStatement("CASE");
            /* Expression? inputExpression; */ // TODO: Add to Case Function + unit test
            var next = _lexems[_pos];
            if (!next.TokenEquals("WHEN"))
            {
                /* inputExpression = */
                CollectExpression();
            }
            next = _lexems[_pos];
            while (next.KeywordEquals("WHEN"))
            {
                var clause = ParseWhenClause();
                clauses.Add(clause);
                next = _lexems[_pos];
            }
            next = _lexems[_pos];
            if (next.KeywordEquals("ELSE"))
            {
                ExpectStatement("ELSE");
                elseClause = CollectExpression();
            }
            ExpectStatement("END");

            return new CaseExpression(clauses, elseClause);
        }

        private CaseClause ParseWhenClause()
        {
            ExpectStatement("WHEN");
            var condition = CollectSearchCondition();
            ExpectStatement("THEN");
            //var clause = CollectSearchCondition();
            var clause = CollectExpression();
            return new CaseClause(condition, clause);
        }

        private Expression ParseNextValueForFunctionCallExpression(Identifier functionName)
        {
            ExpectToken("VALUE");
            ExpectToken("FOR");
            var sequenceName = CollectIdentifier();
            var next = _lexems[_pos];
            if (next.TokenEquals("OVER"))
            {
                throw new NotImplementedException("NEXT VALUE FOR ... OVER byClause; not implemented");
            }
            var arguments = new List<ArgumentExpression>();
            arguments.Add(new ArgumentExpression(new IdentifierExpression(sequenceName), false));
            // important to jump out, there is no closing RBRACE
            return new FunctionCallExpression(functionName, arguments);
        }

        private Expression ParseFunctionCallExpression(Identifier functionName)
        {
            ExpectLexem(typeof(LBrace));
            AdvancedLexemPointer();
            Expression? result;

            if (functionName.IsEqual("CAST"))
            {
                var argument = CollectExpression();
                ExpectStatement("AS");
                var targetDataType = CollectDataType();
                result = new CastFunctionCallExpression(argument, targetDataType);
            }
            else if (functionName.IsEqual("ROW_NUMBER") ||
                     functionName.IsEqual("FIRST_VALUE"))
            {
                // TODO: suppoert not only ROW_NUMBER as funciton Name
                result = ParseRowNumberCallExpression();
            }
            else
            {
                var argumentList = ParseArgumentListWithOutput(true);
                result = new FunctionCallExpression(functionName, argumentList);
            }

            ExpectLexem(typeof(RBrace));
            AdvancedLexemPointer();
            var next = _lexems[_pos];
            if (next.TokenEquals("OVER"))
            {
                result = ParseOverClauseExpression((FunctionCallExpression)result);
            }

            return result;
        }

        private Expression ParseValuesFunctionCallExpression()
        {
            var id = _lexems[_pos];
            ExpectStatement("VALUES");

            var next = _lexems[_pos];
            while (next is LBrace)
            {
                ExpectLexem(typeof(LBrace));
                AdvancedLexemPointer();

                /* TODO var argumentList = */
                ParseArgumentListWithOutput(true);
                ExpectLexem(typeof(RBrace));
                AdvancedLexemPointer();
                next = _lexems[_pos];
                if (next is FieldSeparator)
                {
                    AdvancedLexemPointer();

                    next = _lexems[_pos];
                }
            }

            /*var next = _lexems[_pos];
            if (next.Token.ToUpperInvariant() == "AS")
            {
                AdvancedLexemPointer();
                next = _lexems[_pos];
            }

            if (next is LBrace)
            {
                ExpectLexem(typeof(LBrace));
                AdvancedLexemPointer();
                var aliasList = ParseArgumentListWithOutput(true);
                ExpectLexem(typeof(RBrace));
                AdvancedLexemPointer();
            }*/
            // TODO: enhnace this
            return new FunctionCallExpression(new SimpleIdentifier(id), new List<ArgumentExpression>());
        }

        private Expression ParseRowNumberCallExpression()
        {
            IList<Expression> partitionByValueExpresions = new List<Expression>();
            OrderByClause? orderByClause;

            /* var argumentList = */
            ParseArgumentList(); // skip empty argument list
            ExpectLexem(typeof(RBrace));
            AdvancedLexemPointer();
            var next = _lexems[_pos];
            if (!next.TokenEquals("OVER"))
                throw new ParserException("ROW_NUMBER expects OVER as next symbol");
            AdvancedLexemPointer();
            ExpectLexem(typeof(LBrace));
            AdvancedLexemPointer();
            next = _lexems[_pos];
            while (next.TokenEquals("PARTITION") || next is FieldSeparator)
            {
                if (next.TokenEquals("PARTITION"))
                {
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                    if (!next.TokenEquals("BY"))
                        throw new ParserException("PARTITION expects BY");
                    AdvancedLexemPointer();
                }
                else
                {
                    AdvancedLexemPointer();
                }

                var partitionByExpression = CollectExpression();
                partitionByValueExpresions.Add(partitionByExpression);
                next = _lexems[_pos];
                /*if (next is FieldSeparator)
                {
                    PROBABYLY REMOVE THESE SCOPE
                    AdvancedLexemPointer();
                }*/
            }
            next = _lexems[_pos];
            if (next.TokenEquals("ORDER"))
            {
                ExpectStatement("ORDER");
                ExpectStatement("BY");

                orderByClause = ParseOrderByClause();
            }
            else
            {
                throw new ParserException("ROW_NUMBER expects ORDER BY");
            }
            ExpectLexem(typeof(RBrace));
            return new RowNumberFunctionCallExpression(partitionByValueExpresions, orderByClause!);
        }

        private Expression ParseOverClauseExpression(FunctionCallExpression baseFunction)
        {
            IList<Expression> partitionByValueExpresions = new List<Expression>();
            /* OrderByClause? orderByClause; */

            ExpectToken("OVER");
            // TODO chek var next = _lexems[_pos];
            ExpectLexem(typeof(LBrace));
            AdvancedLexemPointer();
            var next = _lexems[_pos];
            while (next.TokenEquals("PARTITION") || next is FieldSeparator)
            {
                if (next.TokenEquals("PARTITION"))
                {
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                    if (!next.TokenEquals("BY"))
                        throw new ParserException("PARTITION expects BY");
                    AdvancedLexemPointer();
                }
                else
                {
                    AdvancedLexemPointer();
                }


                var partitionByExpression = CollectExpression();
                partitionByValueExpresions.Add(partitionByExpression);
                next = _lexems[_pos];
                /* KILL this block
                AdvancedLexemPointer();
                next = _lexems[_pos];
                if (next.Token.ToUpperInvariant() != "BY")
                    throw new Exception("PARTITION expects BY");

                AdvancedLexemPointer();
                var partitionByExpression = CollectExpression();
                partitionByValueExpresions.Add(partitionByExpression);
                next = _lexems[_pos];
                if (next is FieldSeparator)
                {
                    AdvancedLexemPointer();
                }*/
            }
            next = _lexems[_pos];
            if (next.TokenEquals("ORDER"))
            {
                ExpectStatement("ORDER");
                ExpectStatement("BY");

                /* orderByClause = */
                ParseOrderByClause();
            }
            else
            {
                //throw new Exception("OVER ORDER BY");
            }
            ExpectLexem(typeof(RBrace));
            AdvancedLexemPointer();
            //return new RowNumberFunctionCallExpression(partitionByValueExpresions, orderByClause);
            // TODO: Add OverFunctionCallExpression;
            return baseFunction;
        }

        private IList<Expression> ParseColumnList()
        {
            var argumentList = new List<Expression>();
            var next = _lexems[_pos];
            while (!(next is RBrace || next.TokenEquals("INTO")))
            {
                var argument = CollectExpression();
                /* var alias = */
                ParseAlias();   //TODO: copy alias to argument

                next = _lexems[_pos];
                argumentList.Add(argument);
                if (next is FieldSeparator)
                {
                    AdvancedLexemPointer();
                    continue;
                }
                if (next is RBrace ||
                    next.TokenEquals("INTO", "VALUES") ||
                    next is TSqlKeyword ||
                    next is Semicolon)
                {
                    break;
                }
                else
                {
                    throw new ParserException($"Invalid lexem. Expected separator ',' but was {next}");
                }

            }
            return argumentList;
        }

        private IList<Expression> ParseArgumentList()
        {
            var argumentList = new List<Expression>();
            var next = _lexems[_pos];
            while (!(next is RBrace ||
                     next.TokenEquals("INTO", "OUTPUT", "OPTION") ||
                     (next is TSqlKeyword && !next.KeywordEquals("CASE")) ||
                     next is Semicolon))
            {
                var argument = CollectExpression();
                next = _lexems[_pos];

                argumentList.Add(argument);
                if (next is FieldSeparator)
                {
                    AdvancedLexemPointer();
                    continue;
                }
                if (next is RBrace ||
                    next.TokenEquals("INTO", "OUTPUT", "OPTION") ||
                    (next is TSqlKeyword && !next.KeywordEquals("CASE")) ||
                    next is Semicolon)
                {
                    continue;
                }
                else
                {
                    throw new ParserException($"Invalid lexem. Expected separator ',' but was {next}");
                }

            }
            return argumentList;
        }

        private IList<ArgumentExpression> ParseArgumentListWithOutput(bool outputSupported)
        {
            var argumentList = new List<ArgumentExpression>();
            var next = _lexems[_pos];
            while (!(next is RBrace ||
                     next.TokenEquals("INTO", "WHEN") ||
                     (next is TSqlKeyword && !next.KeywordEquals("CASE", "LEFT", "RIGHT")) ||
                     next is EndSymbol ||
                     next is Semicolon
                    )
                    ||
                     next.TokenEquals("GO"))
            {
                var argument = CollectColumnExpression();
                next = _lexems[_pos];
                bool isOutput = false;
                if (next.TokenEquals("AS"))
                {
                    ParseAlias();
                    next = _lexems[_pos];
                }

                if (outputSupported && next.TokenEquals("OUTPUT"))
                {
                    isOutput = true;
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                }
                argumentList.Add(new ArgumentExpression(argument, isOutput));

                if (next is FieldSeparator)
                {
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                    continue;
                }
                if (next is RBrace || next.TokenEquals("INTO"))
                {
                    continue;
                }
                else if (!outputSupported && next.TokenEquals("OUTPUT"))
                {
                    break;
                }
                else
                {
                    //throw new Exception($"Invalid lexem. Expected separator ',' but was {next}");
                }

            }
            return argumentList;
        }

        private void AddUnrecognizedStatement(Lexem? lexem)
        {
            if (lexem != null)
            {
                Console.WriteLine(lexem);
                /* Console.WriteLine(_lexems[_pos - 4]);
                    Console.WriteLine(_lexems[_pos - 3]);
                    Console.WriteLine(_lexems[_pos - 2]);
                    Console.WriteLine(_lexems[_pos - 1]);
                */

                AddStatement(new UnrecognizedStatement(lexem));
            }
            else
            {
                // TODO: convert to exception?
                Console.WriteLine("AddUnrecognizedStatement called with null lexel");
            }
        }

        private void AddUseStatement()
        {
            AdvancedLexemPointer();
            var identifier = CollectIdentifier();
            AddStatement(new UseStatement(identifier));
        }

        private void AddGoStatement()
        {
            AdvancedLexemPointer();
            AddStatement(new GoStatement());
        }

        private void AddSetStatement()
        {
            AdvancedLexemPointer();
            int pos = _pos;
            var identifier = CollectIdentifier();
            if (_setStatementGrammarRules.ContainsKey(identifier.Name.ToUpperInvariant()))
            {
                _pos = pos;
                var rule = _setStatementGrammarRules[identifier.Name.ToUpperInvariant()];
                // TODO REMOVE SetStatementMatchValues? matchValues = null;
                if (rule.Matches(_lexems, ref _pos, out SetStatementMatchValues? matchValues))
                {
                    AddStatement(new SetStatement(rule, matchValues!));
                }
            }
            else if (identifier.IsEqual("ROWCOUNT"))
            {
                var valueExpression = CollectExpression();
                AddStatement(new SetLocalVariableStatement(identifier, valueExpression)); // TODO: specially deal with ROWCOUNT: no "="
            }
            else if (identifier.IsEqual("LOCK_TIMEOUT"))  // TODO: specially deal with ROWCOUNT: no "="
            {
                var valueExpression = CollectExpression();
                AddStatement(new SetLocalVariableStatement(identifier, valueExpression));
            }
            else if (identifier.IsEqual("IDENTITY_INSERT"))
            {
                var myId = _lexems[_pos];
                var expr = CollectExpression();
                ExpectToken("ON", "OFF");
                AddStatement(new SetLocalVariableStatement(new SimpleIdentifier(myId), expr)); // TODO: do proper, ON/OFF not stored
            }
            else if (identifier.IsEqual("DATEFORMAT"))
            {
                var expr = CollectExpression(); // TODO: specificially add SET DATEFORMAT dmt
                AddStatement(new SetLocalVariableStatement(identifier, expr));
            }
            else
            {
                Expression? expr = null;
                var next = _lexems[_pos];
                if (next is OperatorSymbol && ((OperatorSymbol)next).IsAssignmentSymbol)
                {
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                    if (next.TokenEquals("CURSOR"))
                    {
                        expr = CollectCursorExpression();
                    }
                    else // normal expression
                    {
                        expr = CollectExpression();
                    }
                }
                AddStatement(new SetLocalVariableStatement(identifier, expr!));
            }
        }

        private void AddAlterStatement()
        {
            AdvancedLexemPointer();
            var next = _lexems[_pos];
            if (next.KeywordEquals("PROCEDURE", "PROC"))
            {
                AddProcedureStatement(CreationType.Alter);
            }
            else if (next.KeywordEquals("TABLE"))
            {
                AddAlterTableStatement();
            }
            else if (next.TokenEquals("INDEX"))
            {
                AddAlterIndexStatement();
            }
            else
            {
                AddUnrecognizedStatement(next);
            }
        }

        private void AddCreateStatement()
        {
            AdvancedLexemPointer();
            var next = _lexems[_pos];
            if (next.KeywordEquals("PROCEDURE", "PROC"))
            {
                AddProcedureStatement(CreationType.Create);
            }
            else if (next.KeywordEquals("TRIGGER"))
            {
                AddTriggerStatement(CreationType.Create);
            }
            else if (next.KeywordEquals("TABLE"))
            {
                AddCreateTableStatement(CreationType.Create);
            }
            else if (next.TokenEquals("CLUSTERED", "NONCLUSTERED", "UNIQUE", "INDEX"))
            {
                AddCreateIndexStatement();
            }
            else if (next.TokenEquals("STATISTICS"))
            {
                AddCreateStatisticsStatement();
            }
            else
            {
                AddUnrecognizedStatement(next);
            }
        }

        private void AddCreateStatisticsStatement()
        {
            ExpectToken("STATISTICS");
            var statisticsName = CollectIdentifier();
            ExpectToken("ON");
            var target = CollectExpression();
            if (CheckToken("WITH"))
            {
                ExpectToken("WITH");
                if (CheckToken("SAMPLE"))
                {
                    ExpectToken("SAMPLE");
                    /* TODO var number = */
                    CollectExpression();
                    ExpectToken("PERCENT", "ROWS");
                }
                var next = _lexems[_pos];
                if (next is FieldSeparator)
                    AdvancedLexemPointer();
                if (CheckToken("NORECOMPUTE"))
                {
                    ExpectToken("NORECOMPUTE");
                }
            }
            //TODO: Complete Statistics and add further values to statement
            AddStatement(new CreateStatisticsStatement(statisticsName, target));
        }

        private void AddDropStatement()
        {
            AdvancedLexemPointer();
            var next = _lexems[_pos];
            if (next.KeywordEquals("PROCEDURE", "PROC"))
            {
                //   AddProcedureStatement(CreationType.Create);
                throw new NotImplementedException("DROP PROCEDURE not yet parsed");
            }
            else if (next.KeywordEquals("TABLE"))
            {
                AddDropTableStatement();
            }
            else
            {
                AddUnrecognizedStatement(next);
            }
        }

        private void AddGrantStatement()
        {
            ExpectToken("GRANT");
            ExpectOptionalToken("ALL");
            ExpectOptionalToken("PRIVILEGES");
            /* TODO var next = _lexems[_pos]; */
            string permission = "";
            if (CheckToken("EXECUTE"))
            {
                permission = "EXECUTE";
            }

            Expression? principal = null;
            if (CheckToken("ON"))
            {
                ExpectToken("TO");
                principal = CollectExpression();
            }

            if (principal == null)
                throw new ParserException("Principal is null!");
            AddStatement(new GrantStatement(permission, principal!));
        }

        private void AddTruncateStatement()
        {
            ExpectToken("TRUNCATE");
            ExpectToken("TABLE");
            var tableName = CollectIdentifier();
            var next = _lexems[_pos];
            while (next is FieldSeparator)
            {
                AdvancedLexemPointer();
                tableName = CollectIdentifier();
                next = _lexems[_pos]; // TODO support multiple tables
            }
            AddStatement(new TruncateStatement(tableName));
        }

        private void AddWhileStatement()
        {
            AdvancedLexemPointer();
            var condition = CollectSearchCondition();

            _statementsStack.Push(new List<Statement>());
            ParseStatement();
            var codeBlock = _statementsStack.Pop();
            if (codeBlock.Count == 0)
                throw new ParserException("Invalid structure after WHILE: no statements");

            AddStatement(new WhileStatement(condition, codeBlock[0]));
        }

        private void AddBreakStatement()
        {
            AdvancedLexemPointer();
            AddStatement(new BreakStatement());
        }

        private void AddContinueStatement()
        {
            AdvancedLexemPointer();
            AddStatement(new ContinueStatement());
        }

        private void AddOpenStatement()
        {
            ExpectStatement("OPEN");
            var cursorName = CollectIdentifier();
            AddStatement(new OpenCursorStatement(cursorName));
        }
        private void AddCloseStatement()
        {
            ExpectStatement("CLOSE");
            var cursorName = CollectIdentifier();
            AddStatement(new CloseCursorStatement(cursorName));
        }
        private void AddDeallocateStatement()
        {
            ExpectStatement("DEALLOCATE");
            var cursorName = CollectIdentifier();
            AddStatement(new DeallocateCursorStatement(cursorName));
        }

        private void AddFetchStatement()
        {
            ExpectStatement("FETCH");
            ExpectOptionalStatement("NEXT");
            ExpectOptionalStatement("FIRST");

            /* var next = _lexems[_pos]; */
            if (CheckToken("ABSOLUTE"))
            {
                ExpectToken("ABSOLUTE"); // TODO Store the paraemeter
                /* var steps =*/
                CollectExpression(); // 
            }

            ExpectOptionalStatement("FROM");

            var cursorName = CollectIdentifier();
            var next = _lexems[_pos];
            IList<Expression> variableNames = new List<Expression>();
            if (next.TokenEquals("INTO"))
            {
                AdvancedLexemPointer();

                var variable = CollectExpression();
                variableNames.Add(variable);
                next = _lexems[_pos];
                while (next is FieldSeparator)
                {
                    AdvancedLexemPointer();
                    variable = CollectExpression();
                    variableNames.Add(variable);
                    next = _lexems[_pos];
                }
            }
            AddStatement(new FetchCursorStatement(cursorName, variableNames));
        }

        private void AddWaitForStatement()
        {
            AdvancedLexemPointer();
            ExpectOptionalStatement("DELAY");
            var delay = CollectExpression();
            AddStatement(new WaitForStatement(delay));
        }

        private void AddMergeStatement()
        {
            AdvancedLexemPointer();
            var next = _lexems[_pos];
            /* TODO TopClauseExpression? topClause = null;*/
            if (next.KeywordEquals("TOP"))
            {
                /* topClause = */
                ParseTopClause();
                next = _lexems[_pos];
            }
            /* Identifier? tableName = null; */
            if (next.KeywordEquals("INTO"))
            {
                AdvancedLexemPointer();
            }
            /* tableName = */
            CollectIdentifier();
            /* var margeHints = */
            ParseTableHints();
            /* var tableAlias = */
            ParseAlias();

            ExpectToken("USING");
            //var tableSource1 = ParseTableSourceItemJoined();
            //var tableSource = CollectExpression();
            //var tableSourceAlias = ParseAlias();

            /* var tableSource = */
            ParseTableWithHints();

            ExpectToken("ON");
            /* var mergeSearchConditioN  = */
            CollectSearchCondition();
            next = _lexems[_pos];
            while (next.TokenEquals("WHEN"))
            {
                AdvancedLexemPointer();
                next = _lexems[_pos];
                if (next.TokenEquals("MATCHED"))
                {
                    ParseWhenMatchedClause();
                }
                else if (next.TokenEquals("NOT"))
                {
                    ParseWhenNotMatchedClause();
                }
                next = _lexems[_pos];
            }

            next = _lexems[_pos];
            if (next.TokenEquals("OUTPUT"))
            {
                /* var outputClause = */
                ParseOutputClause();
            }

            if (CheckToken("OPTION"))
            {
                // TODO: do proper function for this
                ParseQueryOptions();

            }

            AddStatement(new MergeStatement());
        }

        private void ParseWhenMatchedClause()
        {
            ExpectToken("MATCHED");
            var next = _lexems[_pos];
            if (next.TokenEquals("AND"))
            {
                ExpectToken("AND");
                /* var clause_searchCondition = */
                CollectSearchCondition();
            }
            ExpectToken("THEN");
            next = _lexems[_pos];
            if (next.TokenEquals("DELETE"))
            {
                ExpectStatement("DELETE");
                // not further action to be done                
            }
            else if (next.TokenEquals("UPDATE"))
            {
                ExpectStatement("UPDATE");
                ExpectStatement("SET");
                /* var arguments = */
                ParseArgumentListWithOutput(false);
            }
        }

        private void ParseWhenNotMatchedClause()
        {
            ExpectToken("NOT");
            ExpectToken("MATCHED");
            var next = _lexems[_pos];
            if (next.TokenEquals("BY"))
            {
                ExpectToken("BY");
                next = _lexems[_pos];

                if (next.TokenEquals("SOURCE"))
                {
                    ExpectToken("SOURCE");
                    next = _lexems[_pos];
                }
                else
                {
                    ExpectToken("TARGET");
                    next = _lexems[_pos];
                }

                if (next.TokenEquals("AND"))
                {
                    ExpectToken("AND");
                    /* var clause_searchCondition = */
                    CollectSearchCondition();
                }
            }
            else if (next.TokenEquals("AND"))
            {
                ExpectToken("AND");
                /* var clause_searchCondition = */
                CollectSearchCondition();
            }

            ExpectToken("THEN");

            next = _lexems[_pos];
            if (next.TokenEquals("DELETE"))
            {
                ExpectStatement("DELETE");
                // not further action to be done                
            }
            else if (next.TokenEquals("INSERT"))
            {
                ExpectToken("INSERT");
                next = _lexems[_pos];
                /* IList<Identifier> columnNames = new List<Identifier>();*/
                if (next is LBrace)
                {
                    /* columnNames = */
                    ParseInsertColumnList();
                }

                next = _lexems[_pos];
                if (next.TokenEquals("VALUES"))
                {
                    AdvancedLexemPointer();
                    /* var valueExpressions = */
                    ParseValueExpressionList();
                }
                else
                {
                    ExpectToken("DEFAULT");
                    ExpectToken("VALUES");
                }
            }
        }

        private void AddWithStatement()
        {
            ExpectStatement("WITH");
            var next = _lexems[_pos];
            Identifier? cteId = null;
            Expression? firstQuery = null;

            do
            {
                if (next is FieldSeparator)
                {
                    AdvancedLexemPointer();
                    /* next = _lexems[_pos]; */
                }

                var cteExpressionName = CollectIdentifier();
                cteId ??= cteExpressionName;
                /* IList<Identifier> columnNames = new List<Identifier>(); */

                next = _lexems[_pos];
                if (next is LBrace)
                {
                    /* columnNames = */
                    ParseInsertColumnList();
                }
                ExpectToken("AS");
                next = _lexems[_pos];

                if (next is LBrace)
                {
                    var query = ParseSubExpression();
                    firstQuery ??= query;
                    next = _lexems[_pos];
                }
            } while (next is FieldSeparator);

            //TODO: support adding multiple CTEs to the WithStatement
            AddStatement(new WithStatement(cteId, ((QueryExpression)((SubExpression)firstQuery!).Inner).Query));
        }


        private void AddBeginStatement()
        {
            AdvancedLexemPointer();
            var next = _lexems[_pos];
            if (next.TokenEquals("TRY"))
            {
                AddTryStatement();
            }
            else if (next.TokenEquals("TRANSACTION") ||
                     next.TokenEquals("TRAN"))
            {
                AdvancedLexemPointer();
                next = _lexems[_pos];
                if (next is LexerIdentifier)
                {
                    AdvancedLexemPointer();
                }
                AddStatement(new BeginTransactionStatement());
                // TODO: Aadd Transaction
            }
            else if (next.TokenEquals("ATOMIC"))
            {
                ExpectToken("ATOMIC");
                /* next = _lexems[_pos]; */
                /* var withAtomicOptions = */
                ParseWithAtomicOptionClause();
                var statementBlock = ParseStatementBlock();
                AddStatement(new BlockStatement(statementBlock));
            }
            else
            {
                var statementBlock = ParseStatementBlock();
                AddStatement(new BlockStatement(statementBlock));
            }
        }
        private IList<Statement> ParseStatementBlock()
        {
            _statementsStack.Push(new List<Statement>());
            var next = _lexems[_pos];
            while (next != null)
            {
                if (next.KeywordEquals("END"))
                {
                    AdvancedLexemPointer();
                    break;
                }
                ParseStatement();
                while (_lexems[_pos] is Semicolon)
                {
                    AdvancedLexemPointer();
                }
                // TODO: LABEL Handling
                if (_pos < _lexems.Count)
                {
                    // TODO: Add dealing with Label Lexems here                                        
                    next = _lexems[_pos];
                    if (!(next is TSqlKeyword || next is Label))
                    {
                        next = null;
                    }
                }
            }
            var bodyStatements = _statementsStack.Pop();
            return bodyStatements;
        }

        private void AddTryStatement()
        {
            ExpectStatement("TRY");
            var tryStatementBlock = ParseStatementBlock();
            ExpectStatement("TRY");
            ExpectStatement("BEGIN");
            ExpectStatement("CATCH");
            var catchStatementBlock = ParseStatementBlock();
            ExpectStatement("CATCH");

            AddStatement(new TryCatchStatement(new BlockStatement(tryStatementBlock, true), new BlockStatement(catchStatementBlock, true)));
        }

        /// <summary>
        /// Called when a keyword is strictly expected. Throws an exception otherwise.
        /// Advances to the next lexem
        /// </summary>
        /// <param name="keyword"></param>
        private void ExpectStatement(string keyword)
        {
            if (_pos < _lexems.Count)
            {
                var next = _lexems[_pos];
                if (!next.KeywordEquals(keyword))
                    throw new ParserException($"Wrong keyword as next symbol: {next.Token}.Expected: {keyword}");

                AdvancedLexemPointer();
            }
        }

        private bool CheckToken(string token)
        {
            if (_pos < _lexems.Count)
            {
                var next = _lexems[_pos];
                return next.TokenEquals(token);
            }
            return false;
        }

        private void ExpectToken(string token)
        {
            if (_pos < _lexems.Count)
            {
                var next = _lexems[_pos];
                if (!next.TokenEquals(token))
                    throw new ParserException($"Wrong lexem as next symbol: {next.Token}. Expected: {token}");

                AdvancedLexemPointer();
            }
        }

        private void ExpectOptionalToken(params string[] tokens)
        {
            if (_pos < _lexems.Count)
            {
                var next = _lexems[_pos];
                if (next.TokenEquals(tokens))
                    AdvancedLexemPointer();
            }
        }

        private void ExpectToken(params string[] tokens)
        {
            if (_pos < _lexems.Count)
            {
                var next = _lexems[_pos];
                bool found = next.TokenEquals(tokens);

                if (!found)
                    throw new ParserException($"Wrong lexem as next symbol: {next.Token}. Expected: {tokens.ToString('|')}");

                AdvancedLexemPointer();
            }
        }

        private void ExpectOptionalStatement(string keyword)
        {
            if (_pos < _lexems.Count)
            {
                var next = _lexems[_pos];
                if (next == null)
                    return;

                if (!next.TokenEquals(keyword))
                {
                    // stay
                }
                else
                {
                    AdvancedLexemPointer();
                }
            }
        }

        /*
        private void ExpectOptionalStatement(string keyword, string expectedInstead)
        {
            if (_pos < _lexems.Count)
            {
                var next = _lexems[_pos];
                if (next == null)
                    throw new Exception($"No keyword as next symbol. Expected: {keyword}");

                if (!next.Token.ToUpperInvariant().Equals(keyword))
                {
                    if (!next.Token.ToUpperInvariant().Equals(expectedInstead))
                    {
                        throw new Exception($"Wrong keyword as next symbol: {next.Token}. Expected: {keyword} or {expectedInstead}");
                    }
                }
                else
                {
                    AdvancedLexemPointer();
                }
            }
        }
        */

        private void ExpectLexem(Type lexemType, string? expectedToken = null)
        {
            if (_pos < _lexems.Count)
            {
                if (_lexems[_pos].GetType().Equals(lexemType))
                {
                    if (expectedToken != null)
                    {
                        if (_lexems[_pos].TokenEquals(expectedToken))
                        {
                            // ok
                        }
                        else
                        {
                            throw new ParserException($"Expected lexem not there: epexcted: {expectedToken} but was: {lexemType.Name}");
                        }
                    }

                    // ok
                }
                else
                {
                    throw new ParserException($"Expected lexem not there: {lexemType.Name} but was {_lexems[_pos]}");
                }
            }
            else
            {
                throw new ParserException($"Lexem stream ended but expected lexem: {lexemType.Name}");
            }
        }

        private void AddProcedureStatement(CreationType creationType)
        {
            AdvancedLexemPointer();
            var identifier = CollectIdentifier();

            var parameterDeclarationList = CollectParameterDeclarations();
            var next = _lexems[_pos];
            if (next.TokenEquals("WITH"))
            {
                /* var procedureOptions = */
                ParseWithProcedureOptionClause();
            }

            ExpectStatement("AS");
            next = _lexems[_pos];
            if (next.TokenEquals("BEGIN"))
            {
                _statementsStack.Push(new List<Statement>());
                ParseStatement();
                var body = _statementsStack.Pop();
                if (body.Count == 0)
                    throw new ParserException("Invalid structure after AS: no statements");
                else if (body.Count > 1)
                {
                    throw new ParserException("Invalid structure after AS: more than 1 statement");
                }
                AddStatement(new ProcedureStatement(creationType, identifier, parameterDeclarationList, body[0]));
            }
            else
            {
                var body = ParseStatementBlock();
                if (body.Count == 0)
                    throw new ParserException("Invalid structure after AS: no statements");

                var blockStatement = new BlockStatement(body, true);
                AddStatement(new ProcedureStatement(creationType, identifier, parameterDeclarationList, blockStatement));
            }
        }

        private void AddTriggerStatement(CreationType creationType)
        {
            AdvancedLexemPointer();
            var identifier = CollectIdentifier();
            ExpectStatement("ON");
            var tableName = CollectIdentifier();
            var next = _lexems[_pos];
            /* IList<string> triggerSourceActions = new List<string>(); */
            if (next.TokenEquals("FOR"))
            {
                ExpectToken("FOR");
                /* triggerSourceActions = */
                CollectTriggerSourceActions();
            }
            else if (next.TokenEquals("AFTER"))
            {
                ExpectToken("AFTER");
                /* triggerSourceActions = */
                CollectTriggerSourceActions();
            }

            next = _lexems[_pos];
            if (next.TokenEquals("NOT"))
            {
                ExpectToken("NOT");
                ExpectToken("FOR");
                ExpectToken("REPLICATION");
            }

            ExpectStatement("AS");
            next = _lexems[_pos];
            if (next.TokenEquals("BEGIN"))
            {
                _statementsStack.Push(new List<Statement>());
                ParseStatement();
                var body = _statementsStack.Pop();
                if (body.Count == 0)
                    throw new ParserException("Invalid structure after AS: no statements");
                else if (body.Count > 1)
                {
                    throw new ParserException("Invalid structure after AS: more than 1 statement");
                }
                AddStatement(new TriggerStatement(creationType, identifier, tableName, body[0]));
            }
            else
            {
                var body = ParseStatementBlock();
                if (body.Count == 0)
                    throw new ParserException("Invalid structure after AS: no statements");

                var blockStatement = new BlockStatement(body, true);
                AddStatement(new TriggerStatement(creationType, identifier, tableName, blockStatement));
            }
        }
        private IList<string> CollectTriggerSourceActions()
        {
            IList<string> triggerSourceActions = new List<string>();
            var next = _lexems[_pos];
            while (IsTriggerStatement(next))
            {
                triggerSourceActions.Add(next.Token.ToUpperInvariant());
                AdvancedLexemPointer();
                next = _lexems[_pos];
                if (next is FieldSeparator)
                {
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                    continue;
                }
            }
            return triggerSourceActions;
        }

        private static bool IsTriggerStatement(Lexem token)
        {
            var upperCaseToken = token.Token.ToUpperInvariant();
            return upperCaseToken == "UPDATE" || upperCaseToken == "INSERT" || upperCaseToken == "DELETE";
        }
        private List<string> ParseWithAtomicOptionClause()
        {
            List<string> withOptions = new();

            ExpectStatement("WITH");
            ExpectLexem(typeof(LBrace));
            AdvancedLexemPointer();
            var next = _lexems[_pos];
            while (next is not RBrace)
            {
                if (next.TokenEquals("TRANSACTION"))
                {
                    ExpectToken("TRANSACTION");
                    ExpectToken("ISOLATION");
                    ExpectToken("LEVEL");
                    ExpectLexem(typeof(OperatorSymbol), "=");
                    AdvancedLexemPointer();
                    /* var isolationLevel = */
                    CollectExpression();
                    withOptions.Add("TRANSACTION ISOLATION LEVEL");
                }
                else if (next.TokenEquals("LANGUAGE"))
                {
                    ExpectToken("LANGUAGE");
                    ExpectLexem(typeof(OperatorSymbol), "=");
                    AdvancedLexemPointer();
                    /* var language = */
                    CollectExpression();
                    withOptions.Add("LANGUAGE");
                }
                else
                {
                    throw new ParserException($"Unidentified {next.Token}");
                }

                next = _lexems[_pos];
                if (next is FieldSeparator)
                {
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                }
            }
            AdvancedLexemPointer();
            return withOptions;
        }

        private List<string> ParseWithProcedureOptionClause()
        {
            List<string> procedureWithOptions = new();

            ExpectStatement("WITH");
            var next = _lexems[_pos];

            while (!next.TokenEquals("AS", "FOR"))
            {
                if (next.TokenEquals("EXECUTE"))
                {
                    ExpectToken("EXECUTE");
                    ExpectToken("AS");
                    var executeAsClause = CollectExpression();
                    procedureWithOptions.Add(executeAsClause.ToString());
                }
                else
                {
                    if (next.TokenEquals("ENCRYPTION"))
                    {
                        ExpectToken("ENCRYPTION");
                        procedureWithOptions.Add("ENCRYPTION");
                    }
                    else if (next.TokenEquals("RECOMPILE"))
                    {
                        ExpectToken("RECOMPILE");
                        procedureWithOptions.Add("RECOMPILE");
                    }
                    else if (next.TokenEquals("NATIVE_COMPILATION"))
                    {
                        ExpectToken("NATIVE_COMPILATION");
                        procedureWithOptions.Add("NATIVE_COMPILATION");
                    }
                    else if (next.TokenEquals("SCHEMABINDING"))
                    {
                        ExpectToken("SCHEMABINDING");
                        procedureWithOptions.Add("SCHEMABINDING");
                    }
                    else
                    {
                        throw new ParserException($"Unidentified {next.Token}");
                    }
                }

                next = _lexems[_pos];
                if (next is FieldSeparator)
                {
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                }
            }
            return procedureWithOptions;
        }

        private void AddAlterTableStatement()
        {
            var oldPos = _pos;
            ExpectToken("TABLE");
            var tableName = CollectIdentifier();
            var next = _lexems[_pos];
            if (next.TokenEquals("WITH"))
            {
                AdvancedLexemPointer();
                next = _lexems[_pos];
            }
            string checkType;
            if (next.TokenEquals("NOCHECK", "CHECK"))
            {
                checkType = next.Token;
                AdvancedLexemPointer();
                if (CheckToken("CONSTRAINT"))
                {
                    ExpectToken("CONSTRAINT");
                    if (CheckToken("ALL"))
                    {
                        ExpectToken("ALL");
                        AddStatement(new AlterTableStatement(tableName,
                            $"{checkType} CONSTRAINT ALL"));
                        return;
                    }
                }
                throw new NotSupportedException($"Invalid token for ALTER TABLE {tableName}: {_lexems[_pos]}");
            }
            else
            {
                _pos = oldPos;
                AddCreateTableStatement(CreationType.Alter);
            }

        }

        private void AddCreateTableStatement(CreationType creationType)
        {
            ExpectStatement("TABLE");
            var tableDefinition = CollectTableDeclaration();
            AddStatement(new CreateTableStatement(creationType, tableDefinition.Item1, tableDefinition.Item2));
        }

        private void AddCreateIndexStatement()
        {
            var next = _lexems[_pos];
            bool unique = false;
            if (next.TokenEquals("UNIQUE"))
            {
                unique = true;
                AdvancedLexemPointer();
            }

            string clusteredType = "";
            next = _lexems[_pos];
            if (next.TokenEquals("CLUSTERED", "NONCLUSTERED"))
            {
                clusteredType = next.Token;
                AdvancedLexemPointer();
            }
            /* next = _lexems[_pos]; */

            ExpectStatement("INDEX");
            var indexName = CollectIdentifier();
            ExpectStatement("ON");
            var onExpression = CollectExpression();
            AddStatement(new CreateIndexStatement(indexName, unique, clusteredType, onExpression));
        }

        /*
        private IList<Expression> CollectWithOptions()
        {
            IList<Expression> withOptions = new List<Expression>();

            if (CheckToken("WITH"))
            {
                ExpectToken("WITH");
                var next = _lexems[_pos];
                if (next is LBrace)
                {
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                }

                while (!(next is RBrace))
                {
                    next = _lexems[_pos];
                    var option = CollectExpression();
                    withOptions.Add(option);
                    next = _lexems[_pos];
                    if (next is FieldSeparator)
                    {
                        AdvancedLexemPointer();
                        next = _lexems[_pos];
                    }

                }
                ExpectLexem(typeof(RBrace));
                AdvancedLexemPointer();
            }
            return withOptions;
        }
        */

        private void AddAlterIndexStatement()
        {
            ExpectToken("INDEX");
            var indexName = CollectIdentifier();
            ExpectToken("ON");
            var target = CollectExpression();
            if (CheckToken("REBUILD"))
            {
                ExpectToken("REBUILD");
                // TODO: store REBUILD clause
                if (CheckToken("WITH"))
                {
                    ExpectToken("WITH");
                    var next = _lexems[_pos];
                    if (next is LBrace)
                    {
                        AdvancedLexemPointer();
                        next = _lexems[_pos];
                    }

                    while (next is not RBrace)
                    {
                        /* var rebuildOption = */
                        CollectExpression();
                        next = _lexems[_pos];
                        if (next is FieldSeparator)
                        {
                            AdvancedLexemPointer();
                            next = _lexems[_pos];
                        }

                    }
                    ExpectLexem(typeof(RBrace));
                    AdvancedLexemPointer();
                }
            }

            AddStatement(new AlterIndexStatement(indexName, target));
        }

        private void AddDropTableStatement()
        {
            ExpectStatement("TABLE");
            if (CheckToken("IF"))
            {
                ExpectToken("IF");
                ExpectToken("EXISTS");
            }

            var identifier = CollectIdentifier();

            AddStatement(new DropTableStatement(identifier));
        }

        private void AddDeclareStatement()
        {
            IList<DeclareVariableStatement> declarationStatements = new List<DeclareVariableStatement>();
            AdvancedLexemPointer();
            bool processNext;
            do
            {
                var identifier = CollectIdentifier();
                var variableType = CollectDataType();
                //AdvancedLexemPointer();
                Expression? initialValueExpression = null;
                if (_pos < _lexems.Count && (_lexems[_pos] is OperatorSymbol) && ((OperatorSymbol)_lexems[_pos]).Symbol == "=")
                {
                    AdvancedLexemPointer();
                    if (_pos < _lexems.Count)
                    {
                        initialValueExpression = CollectExpression();
                    }
                }

                var declarationStatement = new DeclareVariableStatement(identifier, variableType, initialValueExpression);
                declarationStatements.Add(declarationStatement);
                processNext = _pos < _lexems.Count && _lexems[_pos] is FieldSeparator;
                if (processNext)
                    AdvancedLexemPointer();
            } while (_pos < _lexems.Count && processNext);

            if (declarationStatements.Count == 1)
                AddStatement(declarationStatements[0]);
            else
                AddStatement(new MultiDeclareStatement(declarationStatements));
        }

        private void AddIfStatement()
        {
            AdvancedLexemPointer();
            var condition = CollectSearchCondition();
            //var condition = CollectExpression();
            _statementsStack.Push(new List<Statement>());
            ParseStatement();
            var thenClause = _statementsStack.Pop();
            if (thenClause.Count == 0)
                throw new ParserException("Invalid structure after IF: no statements");
            else if (thenClause.Count > 1)
            {
                throw new ParserException("Invalid structure after IF: more than 1 statement");
            }

            var next = _lexems[_pos];
            while (next is Semicolon)
            {
                AdvancedLexemPointer();
                next = _lexems[_pos];
            }
            if (next.KeywordEquals("ELSE"))
            {
                AdvancedLexemPointer();
                _statementsStack.Push(new List<Statement>());
                ParseStatement();
                var elseClause = _statementsStack.Pop();
                if (elseClause.Count == 0)
                    throw new ParserException("Invalid structure after ELSE: no statements");
                else if (elseClause.Count > 1)
                {
                    throw new ParserException("Invalid structure after ELSE: more than 1 statement");
                }
                AddStatement(new IfStatement(condition, thenClause[0], elseClause[0]));
            }
            else
            {
                AddStatement(new IfStatement(condition, thenClause[0]));
            }
        }

        private void AddReturnStatement()
        {
            AdvancedLexemPointer();
            var next = _lexems[_pos];
            Expression? returnValueExpression = null;
            if (!(next is TSqlKeyword || next is Semicolon))
            {
                returnValueExpression = CollectExpression();
            }
            AddStatement(new ReturnStatement(returnValueExpression));
        }

        private void AddThrowStatement()
        {
            AdvancedLexemPointer();
            var next = _lexems[_pos];
            if (next is not TSqlKeyword && next is not EndSymbol && next is not Semicolon)
            {
                var errorNumber = CollectExpression();
                ExpectLexem(typeof(FieldSeparator));
                AdvancedLexemPointer();
                var message = CollectExpression();
                ExpectLexem(typeof(FieldSeparator));
                AdvancedLexemPointer();
                var state = CollectExpression();

                AddStatement(new ThrowStatement(errorNumber, message, state));
            }
            else
            {
                AddStatement(new ThrowStatement());
            }
        }

        private void AddRaiseErrorStatement()
        {
            AdvancedLexemPointer();
            ExpectLexem(typeof(LBrace));
            AdvancedLexemPointer();
            var msgId = CollectExpression();
            ExpectLexem(typeof(FieldSeparator));
            AdvancedLexemPointer();
            var severity = CollectExpression();
            ExpectLexem(typeof(FieldSeparator));
            AdvancedLexemPointer();
            var state = CollectExpression();
            var next = _lexems[_pos];
            while (next is FieldSeparator)
            {
                AdvancedLexemPointer();
                /* var expr = */
                CollectExpression();
                // TODO: add expr to RaiseErrorStatement
                next = _lexems[_pos];
            }
            ExpectLexem(typeof(RBrace));
            AdvancedLexemPointer();

            next = _lexems[_pos];
            if (next.TokenEquals("WITH"))
            {
                AdvancedLexemPointer();
                while (true)
                {
                    /* var option = */
                    CollectExpression();
                    // TODO with option                     
                    next = _lexems[_pos];
                    if (next is FieldSeparator)
                    {
                        AdvancedLexemPointer();
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            AddStatement(new RaiseErrorStatement(msgId, severity, state));
        }

        private void AddRevertStatement()
        {
            // TODO add RevertStatement
            AdvancedLexemPointer();
        }

        private void AddCommitStatement()
        {
            ExpectStatement("COMMIT");
            ExpectOptionalToken("TRANSACTION", "TRAN");
            var next = _lexems[_pos];
            if (next is LexerIdentifier)
            {
                AdvancedLexemPointer();
            }

            AddStatement(new CommitStatement());
        }

        private void AddRollbackStatement()
        {
            ExpectStatement("ROLLBACK");
            ExpectOptionalToken("TRANSACTION", "TRAN");
            var next = _lexems[_pos];
            if (next is LexerIdentifier)
            {
                AdvancedLexemPointer();
            }

            AddStatement(new RollbackStatement());
            // TODO add RollbackStatement                       
        }

        private void AddSaveStatement()
        {
            ExpectStatement("SAVE");
            ExpectOptionalToken("TRANSACTION", "TRAN");
            var savePoint = CollectExpression();
            AddStatement(new SaveStatement(savePoint));
        }

        private void AddExecStatement()
        {
            AdvancedLexemPointer();
            var next = _lexems[_pos];
            if (next.TokenEquals("AS"))
            {
                // TODO: Add Caller Statement
                ExpectStatement("AS");
                /* var caller = */
                CollectExpression();
            }
            else
            {

                var storedProcedureName = CollectExpression();
                Expression? resultVariable = null;
                next = _lexems[_pos];
                if (next.CheckWithOperationSymbol("="))
                {
                    resultVariable = storedProcedureName;
                    AdvancedLexemPointer();
                    storedProcedureName = CollectExpression();
                }
                var argumentList = ParseArgumentListWithOutput(true);
                AddStatement(new CallStoredProcedureStatement(storedProcedureName, argumentList, resultVariable));
            }
        }

        private void AddInsertStatement()
        {
            AdvancedLexemPointer();
            var next = _lexems[_pos];
            if (next.KeywordEquals("INTO"))
            {
                AdvancedLexemPointer();
            }
            var tableName = CollectIdentifier();
            /* var tableHints = */
            ParseTableHints();

            IList<Identifier> columnNames = new List<Identifier>();
            next = _lexems[_pos];
            if (next is LBrace)
            {
                columnNames = ParseInsertColumnList();
            }

            next = _lexems[_pos];
            OutputClause? outputClause = null;
            if (next.TokenEquals("OUTPUT"))
            {
                outputClause = ParseOutputClause();
            }

            IList<Expression> valueExpressions = new List<Expression>();
            next = _lexems[_pos];
            if (next.TokenEquals("DEFAULT"))
            {
                ExpectToken("DEFAULT");
                ExpectToken("VALUES");
                AddStatement(new InsertStatement(tableName, columnNames, outputClause, valueExpressions));
            }
            if (next.KeywordEquals("VALUES"))
            {
                AdvancedLexemPointer();
                valueExpressions = ParseValueExpressionList();
                next = _lexems[_pos];
                while (next is FieldSeparator)
                {
                    AdvancedLexemPointer();
                    valueExpressions = ParseValueExpressionList();
                    next = _lexems[_pos];
                }
                //TODO: multiple VALUES expressions
                AddStatement(new InsertStatement(tableName, columnNames, outputClause, valueExpressions));
            }
            else if (next is LBrace)
            {
                var subQuery = ParseSubExpression();

                if (CheckToken("OPTION"))
                {
                    // TODO: do proper function for this
                    ParseQueryOptions();
                }

                //var table = new DerivedTable((QueryExpression)((SubExpression)subQuery).Inner);
                AddStatement(new InsertStatement(tableName, columnNames, outputClause, ((QueryExpression)((SubExpression)subQuery).Inner).Query));
            }
            else if (next.KeywordEquals("SELECT"))
            {
                _statementsStack.Push(new List<Statement>());
                ParseStatement();
                var queryClause = _statementsStack.Pop();
                if (queryClause.Count == 0)
                    throw new ParserException("Invalid structure after INSERT for SELECT: no statements");
                else if (queryClause.Count > 1)
                {
                    throw new ParserException("Invalid structure after INSERT for SELECT: more than 1 statement");
                }

                if (CheckToken("OPTION"))
                {
                    // TODO: do proper function for this
                    ParseQueryOptions();
                }

                AddStatement(new InsertStatement(tableName, columnNames, outputClause, (QueryStatement)queryClause[0]));
            }
        }

        private void AddUpdateStatement()
        {
            AdvancedLexemPointer();

            /* TopClauseExpression topClause = null; */
            var next = _lexems[_pos];
            if (next.KeywordEquals("TOP"))
            {
                /* topClause = */
                ParseTopClause();
            }

            var tableName = CollectIdentifier();
            /* var tableHints = */
            ParseTableHints();

            ExpectStatement("SET");
            //AdvancedLexemPointer();

            var argumentList = ParseArgumentList();
            next = _lexems[_pos];
            if (next.TokenEquals("OUTPUT"))
            {
                /* OutputClause outputClause = */
                ParseOutputClause();
            }

            next = _lexems[_pos];
            FromClause? fromClause = null;
            if (next.TokenEquals("FROM"))
            {
                fromClause = ParseFromClause();
                next = _lexems[_pos];
            }
            WhereClause? whereClause = null;
            if (next.TokenEquals("WHERE"))
            {
                whereClause = ParseWhereClause();
                /* next = _lexems[_pos]; */
            }

            if (CheckToken("OPTION"))
            {
                // TODO: do proper function for this
                ParseQueryOptions();

            }

            AddStatement(new UpdateStatement(tableName, argumentList,
                fromClause, whereClause));
        }

        private void AddDeleteStatement()
        {
            AdvancedLexemPointer();

            /* TopClauseExpression topClause = null; */
            var next = _lexems[_pos];
            if (next.KeywordEquals("TOP"))
            {
                /* topClause = */
                ParseTopClause();
                /* next = _lexems[_pos]; */
            }

            ExpectOptionalStatement("FROM");
            var tableName = CollectIdentifier();

            /* var tableHints = */
            ParseTableHints();

            next = _lexems[_pos];
            /* OutputClause outputClause = null; */
            if (next.TokenEquals("OUTPUT"))
            {
                /* outputClause = */
                ParseOutputClause();
            }

            next = _lexems[_pos];
            /* FromClause? fromClause; */
            // TODO: FROM twice, certainly wrong!
            if (next.TokenEquals("FROM"))
            {
                /* fromClause = */
                ParseFromClause();
            }

            next = _lexems[_pos];
            WhereClause? whereClause = null;
            if (!next.KeywordEquals("WHERE"))
            {
                //throw new Exception("DELETE expects WHERE clause");
            }
            else
            {
                whereClause = ParseWhereClause();
            }

            if (CheckToken("OPTION"))
            {
                // TODO: do proper function for this
                ParseQueryOptions();

            }

            AddStatement(new DeleteStatement(tableName, whereClause));
        }

        private void AddPrintStatement()
        {
            ExpectStatement("PRINT");
            var expression = CollectExpression();
            AddStatement(new PrintStatement(expression));
        }

        private void AddGotoStatement()
        {
            ExpectStatement("GOTO");
            var label = CollectIdentifier();
            AddStatement(new GotoStatement(label));
        }

        private OutputClause ParseOutputClause()
        {
            AdvancedLexemPointer();
            var columnNameList = ParseColumnList();
            var next = _lexems[_pos];
            if (next.TokenEquals("INTO"))
            {
                AdvancedLexemPointer();
            }
            else
            {
                return new OutputClause(columnNameList);
            }

            var outputTableName = CollectIdentifier();
            IList<Identifier> outputTableColumnNames = new List<Identifier>();
            next = _lexems[_pos];
            if (next is LBrace)
            {
                outputTableColumnNames = ParseInsertColumnList();
            }

            return new OutputClause(columnNameList, outputTableName, outputTableColumnNames);
        }

        private void AddQueryStatement()
        {
            AdvancedLexemPointer();
            var next = _lexems[_pos];
            TopClauseExpression? topClause = null;

            if (next.TokenEquals("DISTINCT"))
            {
                // TODO: Add Distinct to QUERY
                AdvancedLexemPointer();
                next = _lexems[_pos];
            }

            if (next.KeywordEquals("TOP"))
            {
                topClause = ParseTopClause();
                next = _lexems[_pos];
            }

            var columnExpressionList = new List<ColumnDescriptor>();

            while (!next.KeywordEquals("FROM"))
            {
                var expr = CollectColumnExpression();
                var alias = ParseAlias();
                columnExpressionList.Add(new ColumnDescriptor(expr, alias));
                next = _lexems[_pos];
                if (next is FieldSeparator)
                {
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                    continue;
                }
                else
                {
                    break; //other symbol than not FieldSeparator -> exit
                }
            }
            if (next.TokenEquals("INTO"))
            {
                // TODO: Add Distinct to QUERY
                AdvancedLexemPointer();
                /* var intoTable = */
                CollectIdentifier();
                next = _lexems[_pos];
            }

            FromClause? fromClause = null;
            if (next.KeywordEquals("FROM"))
            {
                fromClause = ParseFromClause();
                next = _lexems[_pos];
            }
            WhereClause? whereClause = null;
            if (next.KeywordEquals("WHERE"))
            {
                whereClause = ParseWhereClause();
                next = _lexems[_pos];
            }
            GroupByClause? groupByClause = null;
            if (next.KeywordEquals("GROUP"))
            {
                ExpectStatement("GROUP");
                ExpectStatement("BY");
                groupByClause = ParseGroupBy();
                next = _lexems[_pos];
            }
            /* Expression? havingClause; */
            if (next.TokenEquals("HAVING"))
            {
                AdvancedLexemPointer();
                /* havingClause = */
                CollectSearchCondition();
                next = _lexems[_pos];
            }

            OrderByClause? orderByClause = null;
            if (next.KeywordEquals("ORDER"))
            {
                ExpectStatement("ORDER");
                ExpectStatement("BY");

                orderByClause = ParseOrderByClause();
            }

            ParseForClause();

            if (CheckToken("OPTION"))
            {
                // TODO: do proper function for this
                ParseQueryOptions();

            }
            next = _lexems[_pos];

            ParseForClause();

            SetOperationExpression? setOperationExpr = null;
            if (next is TSqlKeyword && IsSetOperator((TSqlKeyword)next))
            {
                var setOperatorSymbol = (TSqlKeyword)next;
                AdvancedLexemPointer();
                // NOT SKIP AdvancedLexemPointer();
                next = _lexems[_pos];
                if (next.TokenEquals("ALL"))
                {
                    // TODO: store ALL
                    AdvancedLexemPointer();
                }

                var expression = ParseSubExpression();
                setOperationExpr = new SetOperationExpression(setOperatorSymbol, ((QueryExpression)((SubExpression)expression).Inner).Query);
            }

            AddStatement(new QueryStatement(
                topClause, columnExpressionList, fromClause,
                whereClause, groupByClause, orderByClause, setOperationExpr));
        }

        private void ParseForClause()
        {
            int oldPos = _pos;
            var next = _lexems[_pos];
            if (next.TokenEquals("FOR"))
            {
                // TODO Build function FOR
                AdvancedLexemPointer();
                next = _lexems[_pos];
                if (next.TokenEquals("XML"))
                {
                    ParseXMLForClause();
                }
                else if (next.TokenEquals("UPDATE"))
                {
                    // this FOR UPDATE belongs to the cursor
                    _pos = oldPos;
                }
                else if (next.TokenEquals("READ"))
                {
                    ExpectToken("READ");
                    ExpectToken("ONLY");
                }
                else
                {
                    throw new ParserException("Only simple FOR clause in SELECT supported");
                }
            }
        }

        private object? ParseXMLForClause()
        {
            ExpectToken("XML");
            if (CheckToken("RAW"))
            {
                ExpectToken("RAW");
                var next = _lexems[_pos];
                if (next is LBrace)
                {
                    AdvancedLexemPointer();
                    /* var elementName = */
                    CollectExpression();
                    ExpectLexem(typeof(RBrace));
                    AdvancedLexemPointer();
                }
                ParseCommonDirectivesForXML();
            }
            else if (CheckToken("AUTO"))
            {
                ExpectToken("AUTO");
                ParseCommonDirectivesForXML();
            }
            else if (CheckToken("EXPLICIT"))
            {
                ExpectToken("EXPLICIT");
                ParseCommonDirectivesForXML();
            }
            else if (CheckToken("PATH"))
            {
                ExpectToken("PATH");
                var next = _lexems[_pos];
                if (next is LBrace)
                {
                    AdvancedLexemPointer();
                    /* var elementName = */
                    CollectExpression();
                    ExpectLexem(typeof(RBrace));
                    AdvancedLexemPointer();
                }
                ParseCommonDirectivesForXML();
            }
            else
            {
                var next = _lexems[_pos];
                throw new ParserException($"Unexpected lexem in XML FOR {next.Token}");
            }

            return null;
        }

        private void ParseCommonDirectivesForXML()
        {
            var next = _lexems[_pos];
            if (next is FieldSeparator)
            {
                AdvancedLexemPointer();
                if (CheckToken("ROOT"))
                {
                    ExpectToken("ROOT");
                    next = _lexems[_pos];
                    if (next is LBrace)
                    {
                        AdvancedLexemPointer();
                        /* var rootName = */
                        CollectExpression();
                        ExpectLexem(typeof(RBrace));
                        AdvancedLexemPointer();
                    }
                }
                else if (CheckToken("TYPE"))
                {
                    ExpectToken("TYPE");
                }
                else if (CheckToken("ELEMENTS"))
                {
                    ExpectToken("ELEMENTS");
                }
                else
                {
                    throw new NotImplementedException($"ParseCommonDirectivesForXML does not support {_lexems[_pos]}");
                }
            }
            else
            {
                // empty CommonDirectivesForXML seems to be allowed
            }
        }
        private void ParseQueryOptions()
        {
            ExpectToken("OPTION");
            var next = _lexems[_pos];
            if (next is LBrace)
            {
                AdvancedLexemPointer();
                next = _lexems[_pos];
            }
            while (next is not RBrace)
            {
                if (CheckToken("MAXDOP"))
                {
                    ExpectToken("MAXDOP");
                    /* var numberProcessors = */
                    CollectExpression();
                }
                else if (CheckToken("KEEPFIXED"))
                {
                    ExpectToken("KEEPFIXED");
                    ExpectToken("PLAN");
                }
                else if (CheckToken("FORCE"))
                {
                    ExpectToken("FORCE");
                    ExpectToken("ORDER");
                }
                else if (CheckToken("LOOP"))
                {
                    ExpectToken("LOOP");
                    ExpectToken("JOIN");
                }
                else if (CheckToken("OPTIMIZE"))
                {
                    ExpectToken("OPTIMIZE");
                    ExpectToken("FOR");
                    next = _lexems[_pos];
                    if (next is LBrace)
                    {
                        AdvancedLexemPointer();
                        while (next is not RBrace)
                        {
                            CollectExpression();
                            next = _lexems[_pos];
                            if (next is FieldSeparator)
                            {
                                AdvancedLexemPointer();
                                next = _lexems[_pos];
                            }
                        }
                        AdvancedLexemPointer();
                    }
                    else
                    {
                        ExpectToken("UNKNOWN");
                    }
                }
                else
                {
                    CollectExpression();
                }

                next = _lexems[_pos];
                if (next is FieldSeparator)
                {
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                }
            }
            AdvancedLexemPointer();
        }

        private IList<Identifier> ParseInsertColumnList()
        {
            ExpectLexem(typeof(LBrace));
            AdvancedLexemPointer();
            var columnNameExpressions = ParseColumnList();
            ExpectLexem(typeof(RBrace));
            AdvancedLexemPointer();

            var columnNames = new List<Identifier>();
            foreach (var columnName in columnNameExpressions)
            {
                columnNames.Add(((IdentifierExpression)columnName).Identifier);
            }
            return columnNames;
        }

        private IList<Expression> ParseValueExpressionList()
        {
            ExpectLexem(typeof(LBrace));
            AdvancedLexemPointer();
            var valueExpressions = ParseArgumentList();
            ExpectLexem(typeof(RBrace));
            AdvancedLexemPointer();

            return valueExpressions;
        }

        private TopClauseExpression ParseTopClause()
        {
            ExpectStatement("TOP");
            var next = _lexems[_pos];
            Expression? topExpression;
            bool hasBraces = false;
            if (next is LBrace)
            {
                hasBraces = true;
                AdvancedLexemPointer();
                topExpression = CollectExpression();
                ExpectLexem(typeof(RBrace));
                AdvancedLexemPointer();
            }
            else
            {
                topExpression = CollectExpression();
            }

            if (CheckToken("WITH"))
            {
                ExpectToken("WITH");
                ExpectToken("TIES");
            }

            return new TopClauseExpression(topExpression, hasBraces, true);
        }

        private TableSource ParseTableSourceItem()
        {
            var next = _lexems[_pos];

            TableSource table;
            if (next is LBrace) // must be a subquery -> derived table, what about mutliple LBRACE
            {
                AdvancedLexemPointer();
                next = _lexems[_pos];
                if (next.TokenEquals("SELECT"))
                {
                    Expression subQuery = ParseSubExpression();
                    ExpectLexem(typeof(RBrace));
                    AdvancedLexemPointer();
                    var alias = ParseAlias();
                    var tableHintsAfterAlias = ParseTableHints();
                    table = new DerivedTable((QueryExpression)((SubExpression)subQuery).Inner, alias, tableHintsAfterAlias);
                }
                else
                {
                    table = ParseTableSourceItemJoined();
                    ExpectLexem(typeof(RBrace));
                    AdvancedLexemPointer();
                    /* var alias = */
                    ParseAlias();
                    /* var tableHintsAfterAlias = */
                    ParseTableHints();
                    // TODO: add alias + tableHints somehow
                }
            }
            else if (next.TokenEquals("OPENXML"))
            {
                var openXMLName = CollectIdentifier();
                var functionCall = ParseFunctionCallExpression(openXMLName);
                if (CheckToken("WITH"))
                {
                    ParseOpenXMLWithClause();
                }
                var alias = ParseAlias();
                var tableHintsAfterAlias = ParseTableHints();
                table = new OpenXMLTable(functionCall, alias, tableHintsAfterAlias);
            }
            else
            {
                table = ParseTableWithHints();
            }
            return table;
        }

        private TableSource ParseTableWithHints()
        {
            var table = CollectExpression();
            var tableHints = ParseTableHints();
            var alias = ParseAlias();
            var tableHintsAfterAlias = ParseTableHints();
            return new SimpleTable(table, tableHints, alias, tableHintsAfterAlias);
        }

        private void ParseOpenXMLWithClause()
        {
            ExpectToken("WITH");
            ExpectLexem(typeof(LBrace));
            AdvancedLexemPointer();
            var next = _lexems[_pos];
            while (next is not RBrace)
            {
                /* var columName = */
                CollectExpression();
                /* var columnType = */
                CollectDataType();
                next = _lexems[_pos];
                if (next is not FieldSeparator)
                {
                    /* var columnPattern = */
                    CollectExpression();
                    next = _lexems[_pos];
                }

                if (next is FieldSeparator)
                {
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                    continue;
                }
            }
            AdvancedLexemPointer();
        }
        /*
        private JoinMember ParseJoinMember()
        {
            JoinMember joinMember = null;
            var tableMember = ParseTableMember();
            joinMember = tableMember;
            var next = _lexems[_pos];            
            while (next is FieldSeparator || IsJoin())
            {
                if (next is FieldSeparator)
                {
                    AdvancedLexemPointer();
                    var otherMember = ParseTableMember();
                    joinMember = new TableJoin(tableMember, otherMember);
                }
                else
                {
                    bool requiresOn = false;
                    bool requiresRightJoinMember = true;
                    string joinType;
                    string crossType = null;
                    TableJoinMember right = null;
                    if (CheckToken("JOIN"))
                    {
                        ExpectToken("JOIN");
                        joinType = "JOIN";
                        requiresOn = true;
                        requiresRightJoinMember = true;
                    } else
                    if (CheckToken("INNER"))
                    {
                        ExpectToken("INNER");
                        ExpectOptionalToken("LOOP");
                        ExpectToken("JOIN");
                        joinType = "INNER JOIN";
                        requiresOn = true;
                        requiresRightJoinMember = true;
                    } else if (CheckToken("LEFT"))
                    {
                        ExpectToken("LEFT");
                        ExpectOptionalToken("OUTER");
                        ExpectOptionalToken("LOOP");
                        ExpectToken("JOIN");
                        joinType = "LEFT OUTER JOIN";
                        requiresOn = true;
                        requiresRightJoinMember = true;
                    } else if (CheckToken("CROSS"))
                    {
                        ExpectToken("CROSS");
                        next = _lexems[_pos];
                        crossType = next.ToString();
                        ExpectToken("APPLY", "JOIN");
                        joinType = "CROSS";
                        requiresOn = false;
                        requiresRightJoinMember = false;
                    }

                    else { throw new NotImplementedException($"JOINS with {_lexems[_pos]} not supported");}

                    if (requiresRightJoinMember)
                        right = ParseTableMember();   
                    if (requiresOn)
                    {
                        ExpectToken("ON");
                    }

                    
                    if (joinType == "CROSS")
                    {
                        right = ParseTableMember();
                        joinMember = new CrossApply(right, crossType);
                    }
                    else
                    {
                        var joinCondition = CollectSearchCondition();
                        joinMember = new Join(joinType, joinMember, right, joinCondition);
                    }
                }
                next = _lexems[_pos];
            }
            return joinMember;

        }
        */

        private TableSource ParseTableSourceItemJoined()
        {
            IList<Join> joins = new List<Join>();
            TableSource tableSource;
            //if (CanParseDifferentTableSource())
            tableSource = ParseTableSourceItem();
            //else 
            //  tableSource = ParseTableSourceItem();
            while (IsJoin() || IsPivot())
            {
                if (CheckToken("JOIN"))
                {
                    ExpectToken("JOIN");
                    var joinSource = ParseTableSource();
                    ExpectToken("ON");
                    var joinCondition = CollectSearchCondition();
                    joins.Add(new InnerJoin(joinSource, "", joinCondition));
                }
                else if (CheckToken("INNER"))
                {
                    ExpectToken("INNER");
                    ExpectOptionalToken("LOOP", "HASH", "MERGE", "REMOTE");
                    // var joinHint = ""; // TODO: ParseJoinHints
                    ExpectToken("JOIN");
                    var joinSource = ParseTableSource();
                    ExpectToken("ON");
                    var joinCondition = CollectSearchCondition();
                    joins.Add(new InnerJoin(joinSource, "INNER", joinCondition));
                }
                else if (CheckToken("LEFT"))
                {
                    ExpectToken("LEFT");
                    ExpectOptionalToken("OUTER");
                    ExpectOptionalToken("LOOP", "HASH", "MERGE", "REMOTE");
                    var joinHint = ""; // TODO: ParseJoinHints
                    ExpectToken("JOIN");
                    var joinSource = ParseTableSource();
                    ExpectToken("ON");
                    var joinCondition = CollectSearchCondition();
                    joins.Add(new OuterJoin(joinSource, "LEFT", joinHint, joinCondition));
                }
                else if (CheckToken("RIGHT"))
                {
                    ExpectToken("RIGHT");
                    ExpectOptionalToken("OUTER");
                    ExpectOptionalToken("LOOP", "HASH", "MERGE", "REMOTE");
                    var joinHint = ""; // TODO: ParseJoinHints
                    ExpectToken("JOIN");
                    var joinSource = ParseTableSource();
                    ExpectToken("ON");
                    var joinCondition = CollectSearchCondition();
                    joins.Add(new OuterJoin(joinSource, "RIGHT", joinHint, joinCondition));
                }
                else if (CheckToken("FULL"))
                {
                    ExpectToken("FULL");
                    ExpectOptionalToken("OUTER");
                    ExpectOptionalToken("LOOP", "HASH", "MERGE", "REMOTE");
                    var joinHint = ""; // TODO: ParseJoinHints
                    ExpectToken("JOIN");
                    var joinSource = ParseTableSource();
                    ExpectToken("ON");
                    var joinCondition = CollectSearchCondition();
                    joins.Add(new OuterJoin(joinSource, "FULL", joinHint, joinCondition));
                }
                else if (CheckToken("CROSS"))
                {
                    ExpectToken("CROSS");
                    if (CheckToken("APPLY"))
                    {
                        ExpectToken("APPLY");
                        var joinSource = ParseTableSource();
                        joins.Add(new CrossApply(joinSource));
                    }
                    else if (CheckToken("JOIN"))
                    {
                        ExpectToken("JOIN");
                        var joinSource = ParseTableSource();
                        joins.Add(new CrossJoin(joinSource));
                    }
                    else
                    {
                        throw new ParserException("Invalid CROSS join");
                    }
                }
                else if (CheckToken("OUTER"))
                {
                    ExpectToken("OUTER");
                    if (CheckToken("APPLY"))
                    {
                        ExpectToken("APPLY");
                        var joinSource = ParseTableSource();
                        joins.Add(new CrossApply(joinSource));
                    }

                }
                else if (CheckToken("PIVOT"))
                {
                    /* var pivotClause = */
                    ParsePivotClause();
                    /* var alias = */
                    ParseAlias();
                }
            }

            if (joins.Count == 0)
                return tableSource;
            else
                return new TableJoin(tableSource, joins);
        }
        /*
        private bool CanParseDifferentTableSource()
        {
            return (_lexems[_pos] is LBrace && !(_pos < _lexems.Count - 1 && _lexems[_pos + 1].Token.ToUpperInvariant() == "SELECT"));
        }
        */

        private TableSource ParseTableSource()
        {
            TableSource result;
            var next = _lexems[_pos];
            if (next is LBrace)
            {
                if (_pos < _lexems.Count - 1 && _lexems[_pos + 1].TokenEquals("SELECT"))
                {

                    //AdvancedLexemPointer();

                    var inner = ParseTableSourceItem();
                    //var inner = ParseSubExpression();
                    //ExpectLexem(typeof(RBrace));
                    //AdvancedLexemPointer();
                    result = new SubTableSource(inner);
                }
                else if (_pos < _lexems.Count - 1 && _lexems[_pos + 1] is LBrace)
                {
                    AdvancedLexemPointer();
                    result = ParseTableSource();
                    ExpectLexem(typeof(RBrace));
                    AdvancedLexemPointer();
                }
                else
                {
                    AdvancedLexemPointer();
                    result = ParseTableSourceItemJoined();
                    ExpectLexem(typeof(RBrace));
                    AdvancedLexemPointer();

                }
            }
            else
            {
                result = ParseTableSourceItemJoined();
            }
            return result;
        }

        private IList<TableSource> ParseTableSources()
        {
            var tableSources = new List<TableSource>();
            //var tableSource = ParseTableSource();
            var tableSource = ParseTableSourceItemJoined();
            tableSources.Add(tableSource);
            var next = _lexems[_pos];
            while (next is FieldSeparator)
            {
                AdvancedLexemPointer();
                tableSource = ParseTableSource();
                tableSources.Add(tableSource);
                next = _lexems[_pos];
            }
            return tableSources;
        }

        private FromClause ParseFromClause()
        {
            AdvancedLexemPointer();
            return new FromClause(ParseTableSources());
            /*
            
            if ()

            while ((keyword is TSqlKeyword && (((TSqlKeyword)keyword).Token.ToUpperInvariant() == "INNER" ||
                                               ((TSqlKeyword)keyword).Token.ToUpperInvariant() == "LEFT" ||
                                               ((TSqlKeyword)keyword).Token.ToUpperInvariant() == "RIGHT" ||
                                               ((TSqlKeyword)keyword).Token.ToUpperInvariant() == "CROSS" ||
                                               ((TSqlKeyword)keyword).Token.ToUpperInvariant() == "JOIN"
                                               ) || next is FieldSeparator
                                                )
                )
            {*/
        }
        /*
        private FromClause ParseFromClause()
        {
            IList<FromClauseMember> members = new List<FromClauseMember>();
            TSqlKeyword keyword = null;

            ExpectStatement("FROM");
            var next = _lexems[_pos];
            var next2 = _lexems[_pos + 1];
            Table table = null;
            if (next is LBrace && next2.Token.ToUpperInvariant() == "SELECT")
            {
                var subQuery = ParseSubExpression();                                
                table = new DerivedTable((QueryExpression)((SubExpression)subQuery).Inner);
            }
            else
            {
                var oldPos = _pos;
                var tableName = CollectIdentifier();
                next = _lexems[_pos];
                if (next is LBrace)
                {
                    var after = _lexems[_pos + 1];
                    if (!IsTableHintWithoutWith(after.ToString()))
                    {
                        _pos = oldPos;
                        var expression = CollectExpression();
                        table = new ExpressionTableSource(expression);
                    }
                }
                else
                {
                    table = new SimpleTable(tableName);
                }                
            }

            var alias = ParseAlias();
            var tableHints = ParseTableHints();
            members.Add(new FromClauseTable(table, alias, tableHints));

            next = _lexems[_pos];
            keyword = next as TSqlKeyword;
            while ((keyword is TSqlKeyword && ( ((TSqlKeyword)keyword).Token.ToUpperInvariant() == "INNER" ||
                                               ((TSqlKeyword)keyword).Token.ToUpperInvariant() == "LEFT"  ||
                                               ((TSqlKeyword)keyword).Token.ToUpperInvariant() == "RIGHT" ||
                                               ((TSqlKeyword)keyword).Token.ToUpperInvariant() == "CROSS" ||
                                               ((TSqlKeyword)keyword).Token.ToUpperInvariant() == "JOIN"
                                               ) || next is FieldSeparator
                                                )
                )
            {
                bool isJoin = false;
                bool isCrossApply = false;
                string crossType = null;
                string joinType = null;
                
                if (keyword != null && keyword.Token.ToUpperInvariant() == "JOIN")
                {
                    ExpectStatement("JOIN");
                    joinType = "JOIN";
                    isJoin = true;
                }
                if (keyword != null && keyword.Token.ToUpperInvariant() == "INNER")
                {
                    ExpectStatement("INNER");
                    ExpectOptionalStatement("LOOP", "JOIN");
                    ExpectStatement("JOIN");
                    joinType = "INNER JOIN";
                    isJoin = true;
                }
                else if (keyword != null && keyword.Token.ToUpperInvariant() == "LEFT")
                {
                    ExpectToken("LEFT");
                    ExpectOptionalToken("OUTER");
                    ExpectOptionalToken("LOOP");                     // TODO: keep LOOP?
                    ExpectToken("JOIN");
                    joinType = "LEFT OUTER JOIN";
                    isJoin = true;
                }
                else if (keyword != null && keyword.Token.ToUpperInvariant() == "CROSS")
                {
                    ExpectStatement("CROSS");
                    next = _lexems[_pos];
                    if (next.Token.ToUpperInvariant() == "APPLY")
                    { 
                    ExpectStatement("APPLY");
                    joinType = "CROSS APPLY";
                    crossType = "APPLY";
                    isJoin = false;
                    }
                    else if (next.Token.ToUpperInvariant() == "JOIN")
                    {
                        ExpectStatement("JOIN");
                        joinType = "CROSS JOIN";
                        crossType = "JOIN";
                        isJoin = false;
                    }
                }
                else if (next is FieldSeparator)
                {
                    AdvancedLexemPointer();                    
                }

                next = _lexems[_pos];
                if (next is LBrace)
                {
                    var subQuery = ParseSubExpression();                    
                    table = new DerivedTable((QueryExpression)((SubExpression)subQuery).Inner);
                }
                else
                {
                    var oldPos = _pos;
                    var tableName = CollectIdentifier();
                    next = _lexems[_pos];
                    if (next is LBrace)
                    {
                        var after = _lexems[_pos + 1];
                        if (!IsTableHintWithoutWith(after.ToString()))
                        {
                            _pos = oldPos;
                            var expression = CollectExpression();
                            table = new ExpressionTableSource(expression);
                        }
                    }
                    else
                    {
                        table = new SimpleTable(tableName);
                    }

                }
                alias = ParseAlias();
                tableHints = ParseTableHints();

                if (isJoin)
                {
                    ExpectStatement("ON");
                    var joinCondition = CollectSearchCondition();
                    members.Add(new FromClauseJoin(joinType, table, alias, tableHints, joinCondition));
                }
                else if (isCrossApply)
                {
                    
                    members.Add(new FromCrossApply(((DerivedTable)table).Query, alias, crossType));
                }
                else
                {
                    members.Add(new FromClauseTable(table, alias, tableHints));
                }                                
                                    
                next = _lexems[_pos];
                keyword = next as TSqlKeyword;
            }
            next = _lexems[_pos];

            if (next is TSqlKeyword)
            {
                if (((TSqlKeyword)next).Token.ToUpperInvariant() == "WHERE" || ((TSqlKeyword)next).Token.ToUpperInvariant() == "ORDER")
                {
                    // skip
                }                
                else
                {
                    // SKIP, too: ignore exception: throw new Exception($"Unknown keyword {next.Token}");
                }
            }            

            return new FromClause(members);
        }
        */

        private object ParsePivotClause()
        {
            ExpectToken("PIVOT");
            ExpectLexem(typeof(LBrace));
            AdvancedLexemPointer();
            /* var aggregateExpression = */
            CollectExpression();

            ExpectToken("FOR");
            /* var pivotColumn = */
            CollectExpression();
            ExpectToken("IN");
            ExpectLexem(typeof(LBrace));
            AdvancedLexemPointer();

            /* var argumentList = */
            ParseArgumentList();
            ExpectLexem(typeof(RBrace));
            AdvancedLexemPointer();
            ExpectLexem(typeof(RBrace));
            AdvancedLexemPointer();

            return "";
        }

        private Identifier? ParseAlias()
        {
            Identifier? alias = null;

            var next = _lexems[_pos];
            var keyword = next as TSqlKeyword;
            if (keyword != null && keyword.TokenEquals("AS"))
            {
                AdvancedLexemPointer();
                alias = CollectIdentifier();
                next = _lexems[_pos];
                if (next is not LBrace)
                {
                }
                else // TODO Multi-part Alias
                {
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                    while (next is not RBrace)
                    {
                        alias = CollectIdentifier();
                        next = _lexems[_pos];
                        if (next is FieldSeparator)
                        {
                            AdvancedLexemPointer();
                            next = _lexems[_pos];
                        }
                    }
                    AdvancedLexemPointer();
                }
            }
            else if ((next is LexerIdentifier ||
                      next is StringLiteral ||
                      next is IntegerLiteral ||
                      next.TokenEquals("GO")
                     )
                     &&
                     !next.TokenEquals("USING", "OPTION", "FROM", "WHERE", "INTO",
                                        "OPEN", "ORDER", "LEFT", "INNER", "OUTER",
                                        "FULL", "JOIN", "ON", "IF")
                )
            {
                alias = CollectIdentifier();
            }
            return alias;
        }

        private IList<TableHint> ParseTableHints()
        {
            var tableHints = new List<TableHint>();
            var next = _lexems[_pos];
            if (next.KeywordEquals("WITH"))
            {
                AdvancedLexemPointer();
                ExpectLexem(typeof(LBrace));
                AdvancedLexemPointer();
                next = _lexems[_pos];
                while (next is not RBrace)
                {
                    if (next.TokenEquals("INDEX"))
                    {
                        // TODO: add this INDEX hint
                        AdvancedLexemPointer();
                        var hint = CollectExpression();
                        tableHints.Add(new TableHint($"INDEX{hint}"));
                    }
                    else
                    {
                        var hint = CollectExpression();
                        tableHints.Add(new TableHint(hint.ToString()));
                    }

                    next = _lexems[_pos];
                    if (next is FieldSeparator)
                    {
                        AdvancedLexemPointer();
                        next = _lexems[_pos];
                        continue;
                    }
                    else
                    {
                        // ExpectLexem(typeof(RBrace));
                    }
                }
                ExpectLexem(typeof(RBrace));
                AdvancedLexemPointer();
            }
            else if (next is LBrace)
            {
                var after = _lexems[_pos + 1];
                if (IsTableHintWithoutWith(after.Token))
                {
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                    while (next is not RBrace)
                    {
                        var hint = CollectExpression();
                        tableHints.Add(new TableHint(hint.ToString()));
                        next = _lexems[_pos];
                        if (next is FieldSeparator)
                        {
                            AdvancedLexemPointer();
                            next = _lexems[_pos];
                            continue;
                        }
                        else
                        {
                            ExpectLexem(typeof(RBrace));
                        }
                    }
                    ExpectLexem(typeof(RBrace));
                    AdvancedLexemPointer();
                }
            }
            return tableHints;
        }

        private WhereClause ParseWhereClause()
        {
            ExpectStatement("WHERE");
            if (CheckToken("CURRENT"))
            {
                ExpectToken("CURRENT");
                ExpectToken("OF");
                var cursorName = CollectExpression();
                return new WhereClause(new CurrentOfExpression(cursorName));
            }
            else
            {
                var searchCondition = CollectSearchCondition();
                return new WhereClause(searchCondition);
            }
        }

        private GroupByClause ParseGroupBy()
        {
            IList<GroupByClauseMember> clauseMembers = new List<GroupByClauseMember>();
            var id = CollectExpression();
            clauseMembers.Add(new GroupByClauseMember(id));
            var next = _lexems[_pos];
            while (next is FieldSeparator)
            {
                AdvancedLexemPointer();
                id = CollectExpression();
                clauseMembers.Add(new GroupByClauseMember(id));
                next = _lexems[_pos];
            }

            return new GroupByClause(clauseMembers);
        }

        private OrderByClause ParseOrderByClause()
        {
            IList<OrderByClauseMember> clauseMembers = new List<OrderByClauseMember>();
            var id = CollectExpression();
            var orderDirection = OrderDirection.None;
            var next = _lexems[_pos];
            if (next.TokenEquals("ASC"))
            {
                orderDirection = OrderDirection.Asc;
                AdvancedLexemPointer();
            }
            else if (next.TokenEquals("DESC"))
            {
                orderDirection = OrderDirection.Desc;
                AdvancedLexemPointer();
            }
            clauseMembers.Add(new OrderByClauseMember(id, orderDirection));
            next = _lexems[_pos];
            while (next is FieldSeparator)
            {
                AdvancedLexemPointer();
                id = CollectExpression();
                next = _lexems[_pos];
                orderDirection = OrderDirection.None;
                if (next.TokenEquals("ASC"))
                {
                    orderDirection = OrderDirection.Asc;
                    AdvancedLexemPointer();
                }
                else if (next.TokenEquals("DESC"))
                {
                    orderDirection = OrderDirection.Desc;
                    AdvancedLexemPointer();
                }

                clauseMembers.Add(new OrderByClauseMember(id, orderDirection));
                next = _lexems[_pos];
            }
            if (CheckToken("OFFSET"))
            {
                ExpectToken("OFFSET");
                /* var offsetExpression = */
                CollectExpression();
                ExpectToken("ROW", "ROWS");
                if (CheckToken("FETCH"))
                {
                    ExpectToken("FETCH");
                    ExpectToken("FIRST", "NEXT");
                    /* var fetchExpression = */
                    CollectExpression();
                    ExpectToken("ROW", "ROWS");
                    ExpectToken("ONLY");
                    // TODO: add to GroupByClause
                }
            }

            return new OrderByClause(clauseMembers);
        }

        private IList<ParameterDeclaration> CollectParameterDeclarations()
        {
            var parameterDeclarationList = new List<ParameterDeclaration>();
            bool hasAdditionalBraces = false;

            var next = _lexems[_pos];
            if (next is LBrace)
            {
                hasAdditionalBraces = true;
                AdvancedLexemPointer();
            }

            while (_pos < _lexems.Count)
            {
                next = _lexems[_pos];
                if (next.KeywordEquals("AS"))
                {
                    break;
                }
                else if (next is RBrace && hasAdditionalBraces)
                {
                    AdvancedLexemPointer();
                    break;
                }
                else if (next.TokenEquals("WITH"))
                {
                    break; // no parametsr
                }
                parameterDeclarationList.Add(ParseParameterDeclaration());
            }
            return parameterDeclarationList;
        }

        private SqlDataType CollectDataType()
        {
            var next = _lexems[_pos];
            if (next is TSqlKeyword && next.TokenEquals("AS"))
                AdvancedLexemPointer();

            var identifier = CollectIdentifier();
            var typeName = identifier.NameWithBrackets.ToUpperInvariant();
            if (typeName == "VARCHAR" || typeName == "CHAR" || typeName == "NVARCHAR" ||
                typeName == "NCHAR" || typeName == "NVARCHAR" || typeName == "VARBINARY" ||
                typeName == "BINARY")
                return CollectCharacterDataType(identifier);
            if (typeName == "DATETIME2")
                return CollectDateTimeDataType(identifier);
            else if (typeName == "DECIMAL" || typeName == "NUMERIC" || typeName == "DEC" || typeName == "FLOAT")
                return CollectNumericDataType(identifier);
            else if (typeName == "TABLE")
                return CollectTableType(/* identifier, */ false);
            else if (typeName == "CURSOR")
                return CollectCursorType();
            else
                return new SimpleDataType(identifier);
        }

        private Tuple<Identifier, TableType> CollectTableDeclaration()
        {
            var identifier = CollectIdentifier();
            var tableType = CollectTableType(/*identifier, */ true);
            return new Tuple<Identifier, TableType>(identifier, tableType);
        }

        private TableType CollectTableType(/*Identifier baseIdentifier, */ bool skipTableKeywordInOutput)
        {
            var next = _lexems[_pos];
            if (next is LBrace)
            {
                AdvancedLexemPointer();
                var columnDeclarationListAndConstraints = ParseColumnDeclarationList();
                return new TableType(columnDeclarationListAndConstraints.Item1, columnDeclarationListAndConstraints.Item2, skipTableKeywordInOutput);
            }
            throw new ParserException("Invalid syntax for DECLARE TABLE: no left brace '('");
        }

        private Tuple<IList<ColumnDeclaration>, IList<Constraint>> ParseColumnDeclarationList()
        {
            var columnDeclarationList = new Tuple<IList<ColumnDeclaration>, IList<Constraint>>(
                new List<ColumnDeclaration>(), new List<Constraint>());

            while (_pos < _lexems.Count)
            {
                var next = _lexems[_pos];

                if (next is RBrace)
                {
                    AdvancedLexemPointer();
                    return columnDeclarationList;
                }

                if (IsTableConstraint(next.Token))
                {
                    columnDeclarationList.Item2.Add(ParseTableConstraint());
                }
                else if (IsTableIndex(next.Token))
                {
                    /* var tableIndex = */
                    ParseTableIndex();
                    // add to Return Tuple
                }
                else
                {
                    columnDeclarationList.Item1.Add(ParseColumnDeclaration());
                }

                next = _lexems[_pos];
                if (next is FieldSeparator)
                    AdvancedLexemPointer();
            }
            throw new ParserException("ParseColumnDeclarationList moved beyond the set of available lexems");
        }


        private static readonly string[] _columnConstraints = [
                "PRIMARY", "UNIQUE", "CHECK",
                "NULL", "NOT", "CONSTRAINT"
        ];

        private static bool IsColumnConstraint(string keyword)
        {
            foreach (var columnConstraint in _columnConstraints)
            {
                if (keyword.Equals(columnConstraint, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private static readonly string[] _tableConstraints = [
                "PRIMARY", "UNIQUE", "CHECK"
        ];

        private static bool IsTableConstraint(string keyword)
        {
            foreach (var tableConstraint in _tableConstraints)
            {
                if (keyword.Equals(tableConstraint, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
        private static bool IsTableIndex(string keyword)
        {
            return keyword.Equals("INDEX", StringComparison.OrdinalIgnoreCase);
        }

        private ColumnDeclaration ParseColumnDeclaration()
        {
            var identifier = CollectIdentifier();
            var dataType = CollectDataType();

            var columnConstraintsBefore = ParseColumnConstraints();
            var identity = ParseIdentity();
            var defaultValue = ParseDefaultValue();
            /* var columnConstraintsAfter = */
            ParseColumnConstraints();
            //TODO: now wie have 2 ColumnConstraints: Before and After Identity field

            // some hacks in case the columnConstraint is located BEFORE identity or defaultValue
            if (identity == null)
                ParseIdentity();
            if (defaultValue == null)
                ParseDefaultValue();

            /* var index = */
            ParseColumnIndex();

            /*
            IList<Identifier> additionalFlags = new List<Identifier>();
            Lexem next = _lexems[_pos];
            while (_pos < _lexems.Count && next is LexerIdentifier)
            {
                var flag = CollectIdentifier();
                additionalFlags.Add(flag);

                if (_pos < _lexems.Count)
                    next = _lexems[_pos];
            }
            if (next == null || !(next is RBrace || next is FieldSeparator))
                throw new Exception("Invalid end for column declaration");
                */

            return new ColumnDeclaration(identifier, dataType, identity, defaultValue, columnConstraintsBefore);
        }

        private IList<ColumnConstraint> ParseColumnConstraints()
        {

            var columnConstraints = new List<ColumnConstraint>();

            //AdvancedLexemPointer();
            var previous = _lexems[_pos];
            string constraintType = previous.Token.ToUpperInvariant();
            while (IsColumnConstraint(constraintType))
            {
                var oldPos = _pos;
                if (constraintType == "NULL")
                {
                    ExpectToken("NULL");
                    columnConstraints.Add(new ColumnConstraint(previous.Token));
                }
                else if (constraintType == "NOT")
                {
                    ExpectToken("NOT");
                    ExpectToken("NULL");
                    // TODO constraintType = "NOT NULL";
                    columnConstraints.Add(new ColumnConstraint(previous.Token));
                }
                else if (constraintType == "PRIMARY" || constraintType == "UNIQUE")
                {
                    if (constraintType == "PRIMARY")
                    {
                        ExpectToken("PRIMARY");
                        ExpectToken("KEY");
                        // TODO constraintType = "PRIMARY KEY";                            
                    }
                    else
                    {
                        ExpectToken("UNIQUE");
                    }

                    /* string clusteredType = ""; */
                    if (CheckToken("CLUSTERED"))
                    {
                        ExpectToken("CLUSTERED");
                        // TODO clusteredType = "CLUSTERED";
                    }
                    else if (CheckToken("NONCLUSTERED"))
                    {
                        ExpectToken("NONCLUSTERED");
                        // TODO clusteredType = "NONCLUSTERED";
                    }

                    var next = _lexems[_pos];
                    if (next is LBrace)
                    {
                        _pos = oldPos;
                        break;
                    }

                    if (next.TokenEquals("WITH"))
                    {
                        // TODO: Refactor and extract a ParseWithConstraintType function
                        AdvancedLexemPointer();
                        next = _lexems[_pos];
                        if (next is LBrace)
                        {
                            AdvancedLexemPointer();
                            next = _lexems[_pos];
                            if (next.TokenEquals("IGNORE_DUP_KEY"))
                            {
                                ExpectToken("IGNORE_DUP_KEY");
                                next = _lexems[_pos];
                                if (next.Token != "=")
                                {
                                    throw new ParserException("IGNORE_DUP_KEY expects = operator as follower");
                                }
                                AdvancedLexemPointer();
                                var _ /* expr */  = CollectExpression();
                                // TODO: Add to columnConstraint
                            }
                            ExpectLexem(typeof(RBrace));
                            AdvancedLexemPointer();
                        }
                    }
                }
                else if (constraintType == "INDEX")
                {
                    // move to separate INDEX clause
                    AdvancedLexemPointer();
                    var _ /* indexName */ = CollectExpression();
                    var next = _lexems[_pos];
                    if (next.TokenEquals("CLUSTERED", "NONCLUSTERED"))
                    {
                        AdvancedLexemPointer();
                    }
                }
                else if (constraintType == "CONSTRAINT")
                {
                    AdvancedLexemPointer();
                    var _ /* TODO constraintName*/ = CollectExpression();
                    var next = _lexems[_pos];
                    if (!next.TokenEquals("UNIQUE"))
                        throw new ParserException("Only Column constraint supported UNIQUE");
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                    if (next is LBrace)
                    {
                        AdvancedLexemPointer();
                        while (next is not RBrace)
                        {
                            /* var expr = */
                            CollectExpression();
                            next = _lexems[_pos];
                            if (next.TokenEquals("ASC", "DESC"))
                            {
                                AdvancedLexemPointer(); // TODO: skip ORDER/DIRECTION
                            }
                            next = _lexems[_pos];
                            if (next is FieldSeparator)
                            {
                                AdvancedLexemPointer();
                                continue;
                            }
                        }
                    }

                    AdvancedLexemPointer();
                    columnConstraints.Add(new ColumnConstraint(previous.Token));
                }
                else
                    throw new ParserException($"Unknown column constraint {previous.Token}");
                previous = _lexems[_pos];
                constraintType = previous.Token.ToUpperInvariant();
            }
            return columnConstraints;
        }

        private IdentityClause? ParseIdentity()
        {
            var next = _lexems[_pos];
            if (next.TokenEquals("IDENTITY"))
            {
                AdvancedLexemPointer();
                next = _lexems[_pos];
                if (next is LBrace)
                {
                    AdvancedLexemPointer();
                    var seed = CollectExpression();
                    ExpectLexem(typeof(FieldSeparator));
                    AdvancedLexemPointer();
                    var increment = CollectExpression();
                    ExpectLexem(typeof(RBrace));
                    AdvancedLexemPointer();
                    return new IdentityClause(seed, increment);
                }
                else
                {
                    return new IdentityClause();
                }
            }
            return null;
        }

        private DefaultValueClause? ParseDefaultValue()
        {
            if (CheckToken("DEFAULT"))
            {
                ExpectToken("DEFAULT");
                var constantExpression = CollectExpression();
                return new DefaultValueClause(constantExpression);
            }
            return null;
        }

        private object? ParseColumnIndex()
        {
            if (!CheckToken("INDEX"))
                return null;

            ExpectToken("INDEX");
            /* var indexName = */
            CollectIdentifier();
            /* string clustered = ""; */
            var next = _lexems[_pos];
            if (next.TokenEquals("CLUSTERED", "NONCLUSTERED"))
            {
                /* clustered = next.Token; */
                AdvancedLexemPointer();
            }

            // TODO: generate columnIndex
            return null;
        }

        private object? ParseTableIndex()
        {
            if (!CheckToken("INDEX"))
                return null;

            ExpectToken("INDEX");
            var indexName = CollectIdentifier();
            /* string clustered = ""; */
            var next = _lexems[_pos];
            if ("CLUSTERED".Equals(next.Token.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase) ||
                "NONCLUSTERED".Equals(next.Token.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                // TODO clustered = next.Token;                // index ix1 nonclustered (frGameID, frTargetID)                 
                AdvancedLexemPointer();
                next = _lexems[_pos];
            }
            // TODO bool unique = false;
            if ("UNIQUE".Equals(next.Token.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            {
                ExpectToken("UNIQUE");
                //unique = true;
            }
            next = _lexems[_pos];
            if (next is LBrace)
            {
                AdvancedLexemPointer();
                next = _lexems[_pos];
                while (next is not RBrace)
                {
                    var _ /* columnName */ = CollectExpression();
                    ExpectOptionalToken("ASC", "DESC");
                    next = _lexems[_pos];
                    if (next is FieldSeparator)
                    {
                        AdvancedLexemPointer();
                        next = _lexems[_pos];
                    }
                }
                AdvancedLexemPointer();
                // TODO: generate INDEX type
                return null;
            }
            throw new NotImplementedException($"Unspecified INDEX type for INDEX {indexName}: {next.Token}");
        }

        private Constraint ParseTableConstraint()
        {
            var next = _lexems[_pos];
            string constraintType = next.Token.ToUpperInvariant();
            if (constraintType == "PRIMARY" || constraintType == "UNIQUE")
            {
                if (constraintType == "PRIMARY")
                {
                    ExpectToken("PRIMARY");
                    ExpectToken("KEY");
                    constraintType = "PRIMARY KEY";
                }
                else
                {
                    ExpectToken("UNIQUE");
                }

                string clusteredType = "";
                if (CheckToken("CLUSTERED"))
                {
                    ExpectToken("CLUSTERED");
                    clusteredType = "CLUSTERED";
                }
                else if (CheckToken("NONCLUSTERED"))
                {
                    ExpectToken("NONCLUSTERED");
                    clusteredType = "NONCLUSTERED";
                }

                IList<string> columnNames = [];
                next = _lexems[_pos];
                if (next is LBrace)
                {
                    AdvancedLexemPointer();
                    while (next is not RBrace)
                    {
                        var expr = CollectExpression();
                        columnNames.Add(expr.ToString());
                        next = _lexems[_pos];
                        if (next is FieldSeparator)
                        {
                            AdvancedLexemPointer();
                            next = _lexems[_pos];
                        }
                    }
                    AdvancedLexemPointer();
                }

                // TODO var withOptions = CollectWithOptions();                

                return new TableConstraint(constraintType, clusteredType, columnNames);
            }
            else if (constraintType == "CHECK")
            {
                ExpectToken("CHECK");
                if (CheckToken("NOT"))
                {
                    ExpectToken("NOT");
                    ExpectToken("FOR");
                    ExpectToken("REPLICATION");
                }
                ExpectLexem(typeof(LBrace));
                AdvancedLexemPointer();
                var checkCondition = CollectSearchCondition();
                ExpectLexem(typeof(RBrace));
                AdvancedLexemPointer();
                return new CheckConstraint(checkCondition);
            }
            else
                throw new ParserException($"Invalid lexem in parsing table constraint {constraintType} {next.Token}");
        }
        /*
                var next = _lexems[_pos];
                if (next is LBrace)
                {
                    _pos = oldPos;
                    break;
                }

                

                if (next.Token.ToUpperInvariant() == "WITH")
                {
                    // TODO: Refactor and extract a ParseWithConstraintType function
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                    if (next is LBrace)
                    {
                        AdvancedLexemPointer();
                        next = _lexems[_pos];
                        if (next.Token.ToUpperInvariant() == "IGNORE_DUP_KEY")
                        {
                            ExpectToken("IGNORE_DUP_KEY");
                            next = _lexems[_pos];
                            if (next.Token != "=")
                            {
                                throw new Exception("IGNORE_DUP_KEY expects = operator as follower");
                            }
                            AdvancedLexemPointer();
                            var expr = CollectExpression();
                            // TODO: Add to columnConstraint
                        }
                        ExpectLexem(typeof(RBrace));
                        AdvancedLexemPointer();
                    }
                }
            }
            
            if (constraintType == "PRIMARY KEY" || constraintType == "UNIQUE")
            {
                IList<string> columnNames = new List<string>();
                
                if (next is LBrace)
                {
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                    while (! (next is RBrace))
                    {                        
                        next = _lexems[_pos];
                        if (next is LexerIdentifier)
                        {
                            columnNames.Add(next.Token);
                            AdvancedLexemPointer();
                            next = _lexems[_pos];
                            if (next.Token.ToUpperInvariant() == "ASC" || next.Token.ToUpperInvariant() == "DESC")
                            {
                                AdvancedLexemPointer(); // TODO: skip ORDER/DIRECTION
                            }
                        }

                        next = _lexems[_pos];
                        if (next is FieldSeparator)
                        {
                            AdvancedLexemPointer();
                            continue;
                        }
                        else if (next is RBrace)
                        {                            
                            continue;
                        }
                        else
                        {
                            throw new Exception($"Invalid lexem in parsing table constraint {next.Token}");
                        }
                    }
                    AdvancedLexemPointer();
                    
                }
                else
                {
                    throw new Exception($"Invalid lexem in parsing table constraint {next.Token}");
                }
            }
            else if (constraintType == "INDEX")
            {
                IList<string> columnNames = new List<string>();
                identifier = CollectIdentifier();
                next = _lexems[_pos];
                if (next.Token.ToUpperInvariant() == "CLUSTERED" || next.Token.ToUpperInvariant() == "NONCLUSTERED")
                {
                    AdvancedLexemPointer();
                    next = _lexems[_pos];
                }
                ExpectLexem(typeof(LBrace));
                AdvancedLexemPointer();
                next = _lexems[_pos];
                while(!(next is RBrace))
                {
                    var columnName = CollectExpression();
                    columnNames.Add(columnName.ToString());

                    next = _lexems[_pos];
                    if (next is FieldSeparator)
                    {
                        AdvancedLexemPointer();
                        next = _lexems[_pos];
                    }
                }
                
                AdvancedLexemPointer();
                // TODO: do someting with the types (CLUSTERED, ...)
                return new TableConstraint(constraintType, columnNames);
            }
            else
            {
                throw new Exception("CHECK table constraint not yet supported");
            }
        }
        */

        private CursorType CollectCursorType()
        {
            ExpectOptionalToken("LOCAL", "GLOBAL");
            ExpectOptionalToken("FORWARD_ONLY", "SCROLL");
            ExpectOptionalToken("STATIC", "KEYSET", "DYNAMIC", "FAST_FORWARD");
            ExpectOptionalToken("READ_ONLY", "SCROLL_LOCKS", "OPTIMISTIC");
            ExpectOptionalToken("TYPE_WARNING");

            var next = _lexems[_pos];
            if (next.TokenEquals("FOR"))
            {
                ExpectStatement("FOR");
                var query = ParseSubExpression();

                next = _lexems[_pos];
                if ("FOR".Equals(next.Token.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
                {
                    ExpectToken("FOR");
                    ExpectToken("UPDATE");
                    if (CheckToken("OF"))
                    {
                        ExpectToken("OF");
                        // TODO var columnExpr = CollectExpression();
                        next = _lexems[_pos];
                        while (next is FieldSeparator)
                        {
                            AdvancedLexemPointer();
                            // TODO columnExpr = CollectColumnExpression();
                            next = _lexems[_pos];
                            // TODO: Add to Cursor
                        }
                    }
                }

                return new CursorType((QueryExpression)((SubExpression)query).Inner);
            }
            else
            {
                return new CursorType();
            }

        }

        private DateTime2DataType CollectDateTimeDataType(Identifier baseIdentifier)
        {
            var next = _lexems[_pos];
            if (next is LBrace)
            {
                ExpectLexem(typeof(LBrace));
                AdvancedLexemPointer();
                next = _lexems[_pos];
                if (next is IntegerLiteral)
                {
                    var size = ((IntegerLiteral)next).Number;
                    AdvancedLexemPointer();
                    ExpectLexem(typeof(RBrace));
                    AdvancedLexemPointer();
                    return new DateTime2DataType(baseIdentifier, (int)size);
                }
                else
                    throw new ParserException("Invalid parameter for DateTime2DataType:" + next.Token);
            }
            return new DateTime2DataType(baseIdentifier, null);
        }

        private CharacterDataType CollectCharacterDataType(Identifier baseIdentifier)
        {
            long? size;

            if (_pos < _lexems.Count)
            {
                var next = _lexems[_pos];
                if (next is LBrace)
                {
                    AdvancedLexemPointer();
                    if (_pos < _lexems.Count)
                    {
                        next = _lexems[_pos];
                        if (next is IntegerLiteral)
                        {
                            size = ((IntegerLiteral)next).Number;
                            AdvancedLexemPointer();
                            next = _lexems[_pos];
                            if (next is RBrace)
                            {
                                AdvancedLexemPointer();
                                return new CharacterDataType(baseIdentifier, size.Value);
                            }
                        }
                        else if ("MAX".Equals(next.Token.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
                        {
                            AdvancedLexemPointer();
                            next = _lexems[_pos];
                            if (next is RBrace)
                            {
                                AdvancedLexemPointer();
                                return new CharacterDataType(baseIdentifier, -1);
                            }
                        }
                    }
                }
                else
                {
                    return new CharacterDataType(baseIdentifier);
                }
            }
            throw new ParserException($"Incomplete parsing during character data type {baseIdentifier.Name}");
        }

        private DecimalDataType CollectNumericDataType(Identifier baseIdentifier)
        {
            long? precision;
            long? scale;

            if (_pos < _lexems.Count)
            {
                var next = _lexems[_pos];
                if (next is LBrace)
                {
                    AdvancedLexemPointer();
                    if (_pos < _lexems.Count)
                    {
                        next = _lexems[_pos];
                        if (next is IntegerLiteral)
                        {
                            precision = ((IntegerLiteral)next).Number;
                            AdvancedLexemPointer();
                            next = _lexems[_pos];
                            if (next is FieldSeparator)
                            {
                                AdvancedLexemPointer();
                                next = _lexems[_pos];
                                if (next is IntegerLiteral)
                                {
                                    scale = ((IntegerLiteral)next).Number;
                                    AdvancedLexemPointer();
                                    next = _lexems[_pos];
                                    if (next is RBrace)
                                    {
                                        AdvancedLexemPointer();
                                        return new DecimalDataType(baseIdentifier,
                                            precision.HasValue ? (int)precision.Value : (int?)null,
                                            scale.HasValue ? (int)scale.Value : (int?)null);
                                    }
                                }
                            }
                            else if (next is RBrace)
                            {
                                AdvancedLexemPointer();
                                return new DecimalDataType(baseIdentifier,
                                        precision.HasValue ? (int)precision.Value : (int?)null, null);
                            }

                        }
                    }
                }
                return new DecimalDataType(baseIdentifier, null, null);
            }
            throw new ParserException($"Incomplete parsing in decimal data type {baseIdentifier.Name}");
        }

        private ParameterDeclaration ParseParameterDeclaration()
        {
            var identifier = CollectIdentifier();
            var parameterType = CollectDataType();
            Expression? defaultValueExpression = null;
            var next = _lexems[_pos];
            if (next is OperatorSymbol && ((OperatorSymbol)next).Symbol == "=")
            {
                AdvancedLexemPointer();
                if (_pos < _lexems.Count)
                {
                    defaultValueExpression = CollectExpression();
                    /* TODO next = _lexems[_pos]; */
                }
                /* TODO
                else
                    next = null;
                */
            }

            List<Identifier> additionalFlags = [];
            next = _lexems[_pos];
            if (_pos < _lexems.Count && next is LexerIdentifier)
            {
                var flag = CollectIdentifier();
                additionalFlags.Add(flag);

                if (_pos < _lexems.Count)
                    next = _lexems[_pos];
            }

            if (next is FieldSeparator)
            {
                AdvancedLexemPointer();
            }


            return new ParameterDeclaration(identifier, parameterType, additionalFlags, defaultValueExpression);
        }

        public static bool IsSetOperator(TSqlKeyword keyword)
        {
            string token = keyword.Token.ToUpperInvariant();
            return token == "UNION" || token == "EXCEPT" || token == "INTERSECT";
        }


        public bool IsTableHintWithoutWith(string token)
        {
            string s = token.ToUpperInvariant();
            return _tableHintsNotRequiringWith.Contains(s);
        }

        private bool IsJoin()
        {
            //var next = _lexems[_pos];
            if (CheckToken("JOIN") || CheckToken("INNER") || CheckToken("LEFT") || CheckToken("RIGHT") || CheckToken("FULL") || CheckToken("CROSS") || CheckToken("OUTER"))
            {
                //var next2 = _lexems[_pos + 1];
                return true;
            }
            return false;
        }

        private bool IsPivot()
        {
            if (CheckToken("PIVOT") || CheckToken("UNPIVOT"))
                return true;

            return false;
        }
    }
}
