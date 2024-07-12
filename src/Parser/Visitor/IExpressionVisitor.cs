using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions;
using OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Expressions.CaseClauseDataStructures;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Visitor
{
    public interface IExpressionVisitor
    {
        void Visit(StringExpression expression);
        void Visit(IntegerExpression expression);
        void Visit(FloatExpression expression);

        void Visit(IdentifierExpression expression);

        void Visit(UnaryOperatorExpression expression);
        void Visit(PostfixUnaryOperatorExpression expression);
        void Visit(BinaryOperatorExpression expression);
        void Visit(InExpression expression);
        void Visit(LikeExpression expression);
        void Visit(BetweenOperatorExpression expression);
        void Visit(CastFunctionCallExpression expression);
        void Visit(FunctionCallExpression expression);
        void Visit(RowNumberFunctionCallExpression expression);
        void Visit(SetOperationExpression expression);

        void Visit(CaseExpression expression);
        void Visit(CurrentOfExpression expression);
        void Visit(CursorExpression expression);

        void Visit(SubExpression expression);
        void Visit(QueryExpression expression);

        void Visit(CaseClause caseClause);

        void Visit(ArgumentExpression expression);
    }
}
