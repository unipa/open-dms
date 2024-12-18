using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;
using SixLabors.Fonts;

namespace OpenDMS.PdfManager
{


    /// <summary>
    /// RUN apt update -y && apt install -y ghostscript
    /// 
    /// </summary>
    public static class PDFConverter
    {
        private const string LINUX_GHOSTSCRIPT_EXECUTABLE = "/usr/bin/gs";
        private const string WINDOWS_GHOSTSCRIPT_EXECUTABLE = "Apps\\GhostScript\\bin\\gswin64c.exe";
        private const int GHOSTSCRIPT_TIMEOUT = 60000;
        private const int GHOSTSCRIPT_DEFAULT_WIDTH = 64;

        public static async Task<Stream> ConvertToPng(Stream inputStream, int width=GHOSTSCRIPT_DEFAULT_WIDTH, string filePassword="", int fromPageIndex = 1, int toPageIndex = 0)
        {
            return await ConvertToImage(inputStream, width, ImageFormat.pngalpha, filePassword, fromPageIndex, toPageIndex); 
        }
        public static async Task<Stream> ConvertToJpeg(Stream inputStream, int width = GHOSTSCRIPT_DEFAULT_WIDTH, string filePassword = "", int fromPageIndex = 1, int toPageIndex = 0)
        {
            return await ConvertToImage(inputStream, width, ImageFormat.jpeg, filePassword, fromPageIndex, toPageIndex);
        }
        public static async Task<Stream> ConvertToPostscript(Stream inputStream, string filePassword = "", int fromPageIndex = 1, int toPageIndex = 0)
        {
            return await ConvertToImage(inputStream, GHOSTSCRIPT_DEFAULT_WIDTH, ImageFormat.pswrite, filePassword, fromPageIndex, toPageIndex);
        }

        public static async Task<Stream> ConvertToWord(Stream inputStream, string filePassword = "", int fromPageIndex = 1, int toPageIndex = 0)
        {
            return await ConvertToImage(inputStream, GHOSTSCRIPT_DEFAULT_WIDTH, ImageFormat.docxwrite, filePassword, fromPageIndex, toPageIndex);
        }
        public static async Task<Stream> ConvertToXPS(Stream inputStream, string filePassword = "", int fromPageIndex = 1, int toPageIndex = 0)
        {
            return await ConvertToImage(inputStream, GHOSTSCRIPT_DEFAULT_WIDTH, ImageFormat.docxwrite, filePassword, fromPageIndex, toPageIndex);
        }

        public static async Task<Stream> ExtractPages(Stream inputStream, string filePassword = "", int fromPageIndex = 1, int toPageIndex = 0)
        {
            return await ConvertToImage(inputStream, GHOSTSCRIPT_DEFAULT_WIDTH, ImageFormat.pdfwrite, filePassword, fromPageIndex, toPageIndex);
        }

        public static async Task<Stream> Merge(Stream inputStream, Stream streamToMerge, string filePassword = "")
        {
            List<string> lines = new List<string>
            {
                $"-sDEVICE={ImageFormat.pdfwrite}",
            };
            if (!string.IsNullOrWhiteSpace(filePassword))
            {
                lines.Add($"-dUserPassword={filePassword}");
            }
            return await Convert(lines.ToArray(), inputStream, streamToMerge);
        }
        public static async Task<Stream> ConvertToImage (Stream inputStream, int width, ImageFormat ImageFormat, string filePassword = "", int fromPageIndex=1, int toPageIndex=0)
        {
            List<string> lines = new List<string>
            {
                $"-sDEVICE={ImageFormat.ToString()}",
                $"-dFirstPage={fromPageIndex}"
            };
            if (!string.IsNullOrWhiteSpace(filePassword))
            {
                lines.Add($"-dUserPassword={filePassword}");
            }
            // Pages to output
            //if (toPageIndex > fromPageIndex)
            {
                lines.Add($"-dLastPage={toPageIndex}");
            }
            // Page resolution
            lines.Add (String.Format("-r{0}", width / 8));
 //           args.Add(String.Format("-dDEVICEYRESOLUTION={0}", settings.Resolution.Height));

            return await Convert(lines.ToArray(), inputStream);
        }



        public static async Task<Stream> Convert(string[] args, params Stream[] inputStream )
        {
            List<string> switches = new List<string> {
                "-q",
                "-dQUIET",
                "-dPARANOIDSAFER",       // Run this command in safe mode
                "-dBATCH",               // Keep gs from going into interactive mode
                "-dNOPAUSE",             // Do not prompt and pause for each page
                "-dNOPROMPT",            // Disable prompts for user interaction           
                "-dMaxBitmap=500000000", // Set high for better performance
                "-dNumRenderingThreads=4", // Multi-core, come-on!
                "-dPreserveAnnots=true",
                //"-dShowAnnots=true",
                //"-dPreserveAnnots=false",
                "-dPrinted=true",
                //"-dPrinted=false",
                // Configure the output anti-aliasing, resolution, etc
                "-dAlignToPixels=0",
                "-dGridFitTT=0"
                };
            switches.AddRange(args);

            string inputfile = "";
            string outputfile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".tmp");
            switches.Add("-sOutputFile=\""+outputfile+"\"");
            //switches.Add("-sOutputFile=-");


            var output = new MemoryStream();
            {
                Process P = new Process();
                P.StartInfo.FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? LINUX_GHOSTSCRIPT_EXECUTABLE : Path.Combine(AppContext.BaseDirectory, WINDOWS_GHOSTSCRIPT_EXECUTABLE);
                P.StartInfo.UseShellExecute = false;
                P.StartInfo.CreateNoWindow= true;
                P.StartInfo.RedirectStandardOutput = true;
                P.StartInfo.RedirectStandardError = true;
                P.StartInfo.RedirectStandardInput = true;

                inputfile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".pdf");
                var f = File.OpenWrite(inputfile);
                inputStream[0].CopyTo(f);
                f.Close();  
                //File.WriteAllBytes(inputfile, ((MemoryStream)inputStream[0]).ToArray());
                //P.StartInfo.WorkingDirectory = Path.GetDirectoryName(inputfile);
                //switches.Add("-sOutputFile=\"" + (outputfile) + "\"");
                //switches.Add("-");
                switches.Add("\"" + (inputfile) + "\"");


                P.StartInfo.Arguments = string.Join(" ", switches.ToArray());
                var started = P.Start();
                if (started)
                {
                    //await P.StandardOutput.BaseStream.CopyToAsync(output);
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
                File.Delete(inputfile);
                if (File.Exists(outputfile) && new FileInfo(outputfile).Length > 1024)
                {
                    output = new MemoryStream(File.ReadAllBytes(outputfile));
                    File.Delete(outputfile);
 
                }
                return output;
            }
        }


      }
}