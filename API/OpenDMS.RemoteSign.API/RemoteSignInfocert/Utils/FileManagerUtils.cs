using Newtonsoft.Json;
using RemoteSignInfocert.Models;

namespace RemoteSignInfocert.Utils
{
    public class FileManagerUtils
    {
        public static string SIGN_ROOM_EXT = ".SignJson";
        public static string FILE_PARAMETER_EXT = ".SignJson";

        private static void WriteJsonFile<T>(string filename, T content)
        {
            var jsonContent = JsonConvert.SerializeObject(content);
            File.WriteAllText(filename, jsonContent);
        }

        private static T GetJsonFile<T>(string filename)
        {
            var content = File.ReadAllText(filename);
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static FileParameter LoadFileParameter(string filename)
        {
            return GetJsonFile<FileParameter>(filename);
        }

        public static void WriteFileParameter(string filename, FileParameter fp)
        {
            WriteJsonFile(filename, fp);
        }

        public static IEnumerable<FileParameter> GetAllFileParameter(string SignRoomPath)
        {
            var result = new List<FileParameter>();
            foreach (var fl in Directory.GetFiles(SignRoomPath))
            {
                if (fl.EndsWith(FILE_PARAMETER_EXT, StringComparison.InvariantCultureIgnoreCase))
                    result.Add(LoadFileParameter(fl));
            }
            return result;
        }

        public static IEnumerable<string> GetAllFileToSign(string SignRoomPath)
        {
            var result = new List<string>();
            foreach (var fl in Directory.GetFiles(SignRoomPath))
            {
                if (!fl.EndsWith(FILE_PARAMETER_EXT, StringComparison.InvariantCultureIgnoreCase))
                    result.Add(fl);
            }
            return result;
        }

    }
}
