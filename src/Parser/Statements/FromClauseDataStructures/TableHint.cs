namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures
{
    public class TableHint
    {
        private readonly string _hint;

        public TableHint(string hint)
        {
            _hint = hint;
        }

        public override string ToString()
        {
            return _hint;
        }
    }
}
