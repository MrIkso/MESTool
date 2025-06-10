using System.Text;

namespace MESTool
{
    public class DictionaryUtils
    {
        public static string GenerateCharTableContent(Dictionary<ushort, string> _charTable)
        {
            if (_charTable == null || _charTable.Count == 0)
            {
                return string.Empty; 
            }
           
            var sortedEntries = _charTable.OrderBy(kvp => kvp.Key);

            StringBuilder fileContent = new StringBuilder();
            foreach (var entry in sortedEntries)
            {
                ushort code = entry.Key;
                string character = entry.Value;
                
                string hexCode = code.ToString("X");

                string characterValue = character.Replace("\n", "\\n");

                fileContent.AppendLine($"{hexCode}={characterValue}");
            }

            return fileContent.ToString();
        }
    }
}
