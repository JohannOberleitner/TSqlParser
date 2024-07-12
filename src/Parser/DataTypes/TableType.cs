using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.DataTypes
{
    public class TableType : SqlDataType
    {
        private readonly IList<ColumnDeclaration> _columnDeclarationList;
        private readonly IList<Constraint> _tableConstraints;
        private readonly string _columnListRepresentation;
        private readonly string _tableConstraintRepresentation;
        private readonly bool _skipTableKeywordOutput;

        public TableType(IList<ColumnDeclaration> columnDeclarationList, IList<Constraint> tableConstraints, bool skipTableKeywordOutput)
        {
            _columnDeclarationList = columnDeclarationList;
            _tableConstraints = tableConstraints;
            _columnListRepresentation = MakeColumnListRepresentation();
            _tableConstraintRepresentation = MakeTableConstranitRepresentation();
            _skipTableKeywordOutput = skipTableKeywordOutput;
        }

        public IList<ColumnDeclaration> ColumnDeclarationList => _columnDeclarationList;
        public IList<Constraint> TableConstraints => _tableConstraints;

        private string MakeColumnListRepresentation()
        {
            if (_columnDeclarationList.Count == 0)
                return "";

            var sb = new StringBuilder();
            sb.AppendLine();
            sb.Append(_columnDeclarationList[0].ToString());
            for (int i = 1; i < _columnDeclarationList.Count; ++i)
            {
                sb.AppendLine(",");
                sb.Append(_columnDeclarationList[i].ToString());
            }
            sb.AppendLine();
            return sb.ToString();
        }

        private string MakeTableConstranitRepresentation()
        {
            if (_tableConstraints.Count == 0)
                return "";

            var sb = new StringBuilder();
            sb.AppendLine();
            sb.Append(_tableConstraints[0].ToString());
            for (int i = 1; i < _tableConstraints.Count; ++i)
            {
                sb.AppendLine(",");
                sb.Append(_tableConstraints[i].ToString());
            }
            sb.AppendLine();
            return sb.ToString();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (!_skipTableKeywordOutput)
                sb.Append("TABLE");
            sb.Append('(');
            sb.Append(_columnListRepresentation);
            sb.Append(' ');
            sb.Append(_tableConstraintRepresentation);
            sb.Append(')');
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
