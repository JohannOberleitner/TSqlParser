using System.Text;

namespace OberleitnerTech.PortabilityAdvisor.TSqlParser.Parser.Extensions
{
    /// <summary>
    /// Helper class to generate string output for list of strings.
    /// </summary>
    public static class StringListHelpers
    {
        /// <summary>
        /// Generates a combined string output with a separator character between
        /// the strings.
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="separatorChar"></param>
        /// <returns></returns>
        public static string ToString(this string[] strings, char separatorChar = ',')
        {
            var sb = new StringBuilder();
            sb.Append('{');
            sb.Append(strings[0]);
            for (int i = 1; i < strings.Length; ++i)
            {
                sb.Append(separatorChar);
                sb.Append(strings[i]);
            }
            sb.Append('}');
            return sb.ToString();
        }
    }
}
