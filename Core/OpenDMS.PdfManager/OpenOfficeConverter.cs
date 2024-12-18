using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OpenDMS.PdfManager
{


    /// <summary>
    /// RUN apt update -y && apt install -y unoconv
    /// 
    /// </summary>
    public static class OpenOfficeConverter
    {
        private const string UNOCONV_EXECUTABLE = "/usr/lib/libreoffice/program/soffice.bin"; // "/usr/bin/unoconv";
        private const string WINDOWS_UNOCONV_EXECUTABLE = "Apps\\LibreOfficePortable\\App\\libreoffice\\program\\soffice.exe";
        private const int UNOCONV_TIMEOUT = 60000;




        public static async Task<Stream> ConvertToPDF(Stream inputStream)
        {
            List<string> switches =
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ?
                new List<string> {               
                "--norestore",
                "--nofirststartwizard",
                "--headless",
                "--convert-to",
                "pdf"
                }

//                new List<string> {
//  //              "-f=pdf",
//                "--stdin",
//                "--stdout"
//                //"--unsafe-quit-update"
////                "-",
////                "-"
//                }
                :
                new List<string> {
                "--norestore",
                "--nofirststartwizard",
                "--headless",
                "--convert-to",
                "pdf",
                };

            string inputfile = "";
            string outputfile = "";
            var output = new MemoryStream();
            Process P = new Process();
            P.StartInfo.FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? UNOCONV_EXECUTABLE : Path.Combine( AppDomain.CurrentDomain.BaseDirectory, WINDOWS_UNOCONV_EXECUTABLE);
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            //{
            //    P.StartInfo.UseShellExecute = false;
            //    P.StartInfo.RedirectStandardInput = true;
            //    P.StartInfo.RedirectStandardOutput = true;
            //    P.StartInfo.RedirectStandardError = true;
            //}
            //else
            {
                P.StartInfo.UseShellExecute = false;
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    P.StartInfo.LoadUserProfile = true;
                inputfile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".tmp");
                outputfile = Path.ChangeExtension(inputfile, ".pdf");
                using (var memory = new MemoryStream())
                {
                    inputStream.CopyTo(memory);
                    File.WriteAllBytes(inputfile, memory.ToArray());
                }
                P.StartInfo.WorkingDirectory = Path.GetDirectoryName(inputfile);
                switches.Add("\"" + inputfile + "\"");
                switches.Add("--outdir \"" + Path.GetDirectoryName(inputfile) + "\"");
            }
            P.StartInfo.Arguments = string.Join(" ", switches.ToArray());
            var started = P.Start ();
            try
            {
                if (started)
                {
                    //if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    //{
                    //    inputStream.Seek(0, SeekOrigin.Begin);
                    //    await inputStream.CopyToAsync(P.StandardInput.BaseStream);
                    //    P.StandardInput.Close();
                    //}
                    P.WaitForExit(UNOCONV_TIMEOUT);
                    if (P.HasExited)
                    {
                        //if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        //{
                        //    await P.StandardOutput.BaseStream.CopyToAsync(output);
                        //}
                    };
                    //if (output.Length == 0 || !P.HasExited)
                    //{
                    //    var errorMessage = P.StandardError.ReadToEnd();
                    //    if (!string.IsNullOrEmpty(errorMessage))
                    //        throw new TimeoutException(errorMessage);
                    //}
                };

                if (!started)
                {
                    var errorMessage = P.StandardError.ReadToEnd();
                    if (!string.IsNullOrEmpty(errorMessage))
                        throw new FileNotFoundException(errorMessage);
                }
                //                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                if (File.Exists(outputfile))
                {
                    output = new MemoryStream(File.ReadAllBytes(outputfile));
                    File.Delete(outputfile);
                }
                File.Delete(inputfile);
            }
            catch (Exception ex) {
                throw;
            };
            return output;
        }
    }
}