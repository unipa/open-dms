using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDMS.PdfManager
{
    public static class Pdf2Html
    {
        //private string ExecutablePath = "";

        private const string LINUX_GHOSTSCRIPT_EXECUTABLE = "/usr/bin/pdftohtml";
        private const string WINDOWS_GHOSTSCRIPT_EXECUTABLE = "Apps\\GhostScript\\bin\\pdftohtml.exe";
        private const int GHOSTSCRIPT_TIMEOUT = 60000;
        private const int GHOSTSCRIPT_DEFAULT_WIDTH = 64;


        //public Pdf2Html()
        //{

        //    if (String.IsNullOrEmpty(ExecutablePath)) ExecutablePath = Paths.FindExecutable("pdftohtml");
        //    if ((String.IsNullOrEmpty(ExecutablePath)) && (System.IO.File.Exists(System.IO.Path.Combine(Paths.ProgramDir, "bin", "pdftohtml.exe"))))
        //        ExecutablePath = System.IO.Path.Combine(Paths.ProgramDir, "bin", "pdftohtml.exe");
        //    else
        //    if ((String.IsNullOrEmpty(ExecutablePath)) && (System.IO.File.Exists(System.IO.Path.Combine(Paths.ProgramDir, "gs", "pdftohtml.exe"))))
        //        ExecutablePath = System.IO.Path.Combine(Paths.ProgramDir, "gs", "pdftohtml.exe");
        //    else
        //    if ((String.IsNullOrEmpty(ExecutablePath)) && (System.IO.File.Exists(System.IO.Path.Combine(Paths.ProgramDir, "gs", "pdf2html.exe"))))
        //        ExecutablePath = System.IO.Path.Combine(Paths.ProgramDir, "gs", "pdf2html.exe");
        //    else
        //        ExecutablePath = "";
        //}

        private static Regex counter = new Regex(@"Processing pages \d+ through (\d+)\.", RegexOptions.Compiled);


        public static Int32 Convert(String Inputfile, Int32 FromPage, Int32 ToPage, String Format, Double Zoom, String Output)
        {
            //if (String.IsNullOrEmpty(ExecutablePath)) return 1;
            string commandline = String.Format("-c -q -i -noframes -hidden {0} {1} {2} {3} \"{4}\" \"{5}\"",
                Format,
                FromPage > 0 ? "-f " + FromPage.ToString() : "",
                ToPage > 0 ? "-l " + ToPage.ToString() : "",
                Zoom > 0 ? "-zoom " + Zoom.ToString().Replace(',', '.') : "",
                Inputfile,
                Output);

            Process P = new Process();
            P.StartInfo.FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? LINUX_GHOSTSCRIPT_EXECUTABLE : Path.Combine(AppContext.BaseDirectory, WINDOWS_GHOSTSCRIPT_EXECUTABLE);
            P.StartInfo.UseShellExecute = false;
            P.StartInfo.CreateNoWindow = true;
            P.StartInfo.RedirectStandardOutput = true;
            P.StartInfo.RedirectStandardError = true;
            P.StartInfo.RedirectStandardInput = true;
            P.StartInfo.Arguments = commandline;

            var started = P.Start();
            if (started)
            {
                //                await P.StandardOutput.BaseStream.CopyToAsync(output);
                P.WaitForExit(GHOSTSCRIPT_TIMEOUT);
                if (!P.HasExited)
                {
                    started = false;
                }
            };

            if (!started)
            {
                var errorMessage = P.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(errorMessage))
                    throw new FileNotFoundException(errorMessage);
            }
            return (P.ExitCode);

        }

        public static Int32 Convert(String Inputfile, String Format, Double Zoom, String Output)
        {
            return Convert(Inputfile, 1, 0, Format, Zoom, Output);
        }
        public static Int32 Convert(String Inputfile, String Format, String Output)
        {
            return Convert(Inputfile, 1, 0, Format, 1.5, Output);
        }

        public static Int32 ToXml(String Inputfile, Int32 FromPage, Int32 ToPage, String Output)
        {
            return Convert(Inputfile, FromPage, ToPage, "-xml", 1.5, Output);
        }
        public static Int32 ToXml(String Inputfile, String Output)
        {
            return Convert(Inputfile, 1, 0, "-xml", 1.5, Output);
        }
        public static string ToXml(String Inputfile)
        {
            var xml = "";
            string Output = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()+".tmp");
            Convert(Inputfile, 1, 0, "-xml", 1, Output);
            if (File.Exists(Output+".xml"))
            {
                xml = File.ReadAllText(Output+".xml");
                File.Delete(Output + ".xml");
            }
            return xml;
        }
    }
}
