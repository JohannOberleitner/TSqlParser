namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Statements.FromClauseDataStructures
{
    /*public class TableJoinMember : JoinMember
    {
        private TableSource _table;
        private Identifier _aliasName;
        private IList<TableHint> _tableHints;
        private string _tableHintRepresentation;

        public TableJoinMember(TableSource table, Identifier aliasName, IList<TableHint> tableHints)
        {
            _table = table;
            _aliasName = aliasName;
            _tableHints = tableHints;
            _tableHintRepresentation = MakeTableHintRepresentation();
        }
        private string MakeTableHintRepresentation()
        {
            StringBuilder sb = new StringBuilder();
            if (_tableHints.Count > 0)
            {
                sb.Append(_tableHints[0]);
                for(int i=1;i<_tableHints.Count; ++i)
                {
                    sb.Append(", ");
                    sb.Append(_tableHints[1]);
                }
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_table);            
            if(_aliasName != null)
            {
                sb.Append(" AS ");
                sb.Append(_aliasName);
            }
            if (_tableHints.Count>0)
            {
                sb.Append(" WITH(");
                sb.Append(_tableHintRepresentation);
                sb.Append(')');
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }*/

}
