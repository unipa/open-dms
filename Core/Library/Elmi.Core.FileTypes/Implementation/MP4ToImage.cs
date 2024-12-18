using Elmi.Core.PreviewGenerators;
using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.Pipes;
using MimeKit;
using OpenDMS.PdfManager;
using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Elmi.Core.FileConverters.Implementation
{
    public class MP4ToImage : IFileConverter
    {
        public string[] FromFile => new[] { "MP4" };
        public string ToFile => "PNG";

        private const string LINUX_FFMPEG_EXECUTABLE = "/usr/bin/ffmpeg";
        private const string WINDOWS_FFMPEG_EXECUTABLE = "Apps\\ffmpeg\\bin\\ffmpeg.exe";

        public async Task<Stream> Convert(string Extension, Stream data)
        {
            var Output = new MemoryStream();
            try
            {
                //ffmpeg -i filename.mp4 - vf "select=eq(n\,0)" - vframes 1 filename.png
                //ffmpeg -i pipe:0 -vf -f mp4 "select=eq(n\,0)" -vframes 1 -f apng pipe:1

                Process P = new Process();
                P.StartInfo.FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? LINUX_FFMPEG_EXECUTABLE : Path.Combine(AppContext.BaseDirectory, WINDOWS_FFMPEG_EXECUTABLE);
                P.StartInfo.UseShellExecute = false;
                P.StartInfo.RedirectStandardOutput = true;
                P.StartInfo.RedirectStandardError = true;
                P.StartInfo.RedirectStandardInput = true;
                string inputfile =Path.Combine( Path.GetTempPath(), Guid.NewGuid().ToString() + ".html");
                File.WriteAllBytes(inputfile, ((MemoryStream)data).ToArray());

                P.StartInfo.Arguments = "-i \""+ inputfile+"\" -vf \"select=eq(n\\,5)\" -vframes 1 -f apng -";

                var started = P.Start();
                if (started)
                {
                    //data.Seek(0, SeekOrigin.Begin);
                    //data.CopyTo(P.StandardInput.BaseStream);
                    //P.StandardInput.Close();
                    P.WaitForExit(1000);
                    await P.StandardOutput.BaseStream.CopyToAsync(Output);
                    if (!P.HasExited)
                    {
                        var errorMessage = P.StandardError.ReadToEnd();
                        if (!string.IsNullOrEmpty(errorMessage))
                            throw new TimeoutException(errorMessage);
                    }
                };
                if (!started)
                {
                    var errorMessage = P.StandardError.ReadToEnd();
                    if (!string.IsNullOrEmpty(errorMessage))
                        throw new FileNotFoundException(errorMessage);
                }
                if (File.Exists(inputfile))
                    File.Delete(inputfile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Output;
        }
    }
}
