using DigitalSignService.Interfaces;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel;
using System.Diagnostics;

namespace DigitalSignService.Services
{
    public class CheckOutService : ICheckOutService
    {

        public class TaskInfo
        {
            public string Host { get; set; }
            public int DocumentId { get; set; }
            public string FileName { get; set; }
            public byte[] FileData { get; set; }
            public BackgroundWorker Worker { get; set; }
            public bool Changed { get; set; } = false;
            public bool Terminated { get; set; } = false;
        }


        private const string fileDir = "DMS";

        public IDMSWrapper Wrapper { get; }
        public IAppSettingService AppSettingService { get; }

        public CheckOutService(
            IDMSWrapper wrapper,
            IAppSettingService appSettingService)
        {
            Wrapper = wrapper;
            AppSettingService = appSettingService;
        }

        static Dictionary<string, TaskInfo> Tasks = new();

        public async Task<string> AddFile(string Host, int documentId)
        {
            var doc = await Wrapper.GetDocument(Host, documentId);
            var fileData = await Wrapper.CheckOut(Host, doc.Image.Id);
            if (fileData == null || fileData.Length == 0) { return ""; };

            string guid = doc.Image.Id.ToString();// Guid.NewGuid().ToString();
            if (!Tasks.ContainsKey(guid))
            {
                var filename = doc.Image.OriginalFileName;
                string _path = Path.Combine(Path.GetTempPath(), fileDir);
                var fName = Path.Combine(_path, guid + "_" + filename);
                try
                {
                    if (!Directory.Exists(_path))
                        Directory.CreateDirectory(_path);
                    File.WriteAllBytes(fName, fileData);
                    BackgroundWorker backgroundWorker = new BackgroundWorker();
                    Tasks.Add(guid, new TaskInfo() { Host = Host, DocumentId = documentId, FileName = fName, Worker = backgroundWorker });

                    backgroundWorker.DoWork += async (sender, e) =>
                    {
                        try
                        {
                            var fName = e.Argument?.ToString() ?? "";
                            if (!string.IsNullOrWhiteSpace(fName))
                            {
                                Process P = new Process();
                                P.StartInfo = new ProcessStartInfo(fName);
                                P.StartInfo.UseShellExecute = true;
                                P.StartInfo.CreateNoWindow = true;
                                if (P.Start())
                                {
                                    P.WaitForExit();
                                    var SHA1 = OpenDMS.Domain.DigitalSignature.MessageDigest.HashString(OpenDMS.Domain.DigitalSignature.MessageDigest.HashType.SHA256, fileData);
                                    var newfileData = File.ReadAllBytes(fName);
                                    var SHA2 = OpenDMS.Domain.DigitalSignature.MessageDigest.HashString(OpenDMS.Domain.DigitalSignature.MessageDigest.HashType.SHA256, newfileData);
                                    if (SHA1 != SHA2)
                                    {
                                        Tasks[guid].FileData = newfileData;
                                        var FName = Tasks[guid].FileName;
                                        int i = FName.IndexOf('_');
                                        if (i >= 0) FName = FName.Substring(i + 1);
                                        var data = await Wrapper.AddFile(Tasks[guid].Host, Tasks[guid].DocumentId, FName, newfileData); //.GetAwaiter().GetResult();
                                        Tasks[guid].Changed = data > 0;
                                    }
                                }
                                try
                                {
                                    if (File.Exists(fName))
                                        File.Delete(fName);
                                }
                                catch
                                {
                                }
                            }
                        }
                        finally
                        {
                            Tasks[guid].Terminated = true;
                        }
                    };
                    backgroundWorker.RunWorkerAsync(fName);
                    return guid;
                }
                catch (Exception)
                {
                    if (File.Exists(fName))
                        File.Delete(fName);
                    throw;
                }
            }
            return guid;
        }


        public async Task<bool> Changed(string guid)
        {
            var T = Tasks[guid];
            if (T.Terminated)
            {
                Tasks.Remove(guid);
                return T.Changed;
            }
            return false;
        }

        public async Task<bool> Alive(string guid)
        {
            return Tasks.ContainsKey(guid);
        }
    }
}
