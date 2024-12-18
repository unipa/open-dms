using System.Text.RegularExpressions;

namespace RemoteSignInfocert
{
    public static class Utility
    {

        public static string RemoveDocIdPattern(string filename)
        {
            string pattern = "(_[(]#document_Id_([0-9]{1,})#[)]_)";
            return Regex.Replace(filename, pattern, "");
        }

        public static int ExtractDocId(string filename)
        {
            string pattern = @"[(]#document_Id_([0-9]{1,})#[)]";
            Match match = Regex.Match(filename, pattern);

            if (match.Success)
            {
                string docIdStr = match.Groups[1].Value;
                if (int.TryParse(docIdStr, out int docId))
                {
                    return docId;
                }
            }

            // Se non viene trovato alcun match o non è possibile convertirlo in int, restituisci un valore di default
            return -1; // o qualsiasi altro valore di default che si adatta al tuo caso
        }

    }
}
