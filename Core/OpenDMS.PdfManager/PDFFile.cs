using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Colors;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Layout;
using iText.Layout.Element;
using iText.Signatures;
using iText.Kernel.Pdf.Xobject;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Geom;
using SixLabors.ImageSharp;
using iText.IO.Image;

namespace OpenDMS.PdfManager
{
    public class PDFFile : IDisposable
    {
        private byte[] pdfMagicNumber = { 0x25, 0x50, 0x44, 0x46 };
        private readonly Stream fileStream;
        private readonly string filePassword;
        protected int npages = 0;
        private bool disposing = false;

        public PDFFile()
        {
            fileStream = null;
            filePassword = "";
        }

        public PDFFile(Stream FileStream, string FilePassword = "")
        {
            fileStream = FileStream;
            filePassword = FilePassword;
        }

        public PDFFile(string FileName, string FilePassword = "")
        {
            fileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            filePassword = FilePassword;
        }

        public void Dispose()
        {
            if (disposing) return;
            disposing = true;
            if (fileStream != null)
                fileStream.Dispose();
        }

        public bool IsProtected()
        {
            if (fileStream == null) return false;

            try
            {
                using (PdfReader pdf = new PdfReader(fileStream))
                {
                    return false;
                }
            }
            catch (BadPasswordException)
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Stream> Merge(Stream StreamToMerge)
        {
            return await PDFConverter.Merge(fileStream, StreamToMerge);
        }


        //public PdfXForm Fields()
        //{
        //    return new PdfXForm(Fname);
        //}



        // IMAGE MANAGEMENT
        public async Task<byte[]> GetThumbnail(int imageWidth)
        {
            using (var M = new MemoryStream())
            {
                (await PDFConverter.ConvertToPng(fileStream, imageWidth, filePassword, 1, 1)).CopyTo(M);
                return M.ToArray();
            }
        }

        public async Task<Stream> GetPageImageAsStream(Int32 PageNumber, int imageWidth)
        {
            return (await PDFConverter.ConvertToPng(fileStream, imageWidth, filePassword, PageNumber, PageNumber));
        }
        public async Task<byte[]> GetPageImage(Int32 PageNumber, int imageWidth)
        {
            using (var M = new MemoryStream())
            {
                var X = (await PDFConverter.ConvertToPng(fileStream, imageWidth, filePassword, PageNumber, PageNumber));//.CopyTo(M);
                X.Position = 0;
                X.CopyTo(M);
                return M.ToArray();
            }
        }
        public async Task<Stream> AddImage(Int32 PageNumber, SixLabors.ImageSharp.Image Image, Double X, Double Y, Boolean Percent = true, Boolean OnTop = true)
        {
            using (var ms = new MemoryStream())
            {
                await Image.SaveAsPngAsync(ms);
                return await AddImage(PageNumber, ms, X, Y, Percent, OnTop);
            }
        }

        public async Task<Stream> AddImage(Int32 PageNumber, Stream Image, Double X, Double Y, Boolean Percent = true, Boolean OnTop = true)
        {
            var dest = new MemoryStream();
            {
                using (var mem = new MemoryStream())
                {
                    if (fileStream != null) fileStream.Position = 0;
                    using (var pdfDoc = fileStream != null ? new PdfDocument(new PdfReader(fileStream), new PdfWriter(mem)) : new PdfDocument(new PdfWriter(mem)))
                    {
                        byte[] imageByte = null;
                        using (var M = new MemoryStream())
                        {
                            Image.Seek(0, SeekOrigin.Begin);
                            await Image.CopyToAsync(M);
                            imageByte = M.ToArray();
                        }

                        var R = pdfDoc.GetPage(PageNumber).GetPageSizeWithRotation();
                        Double TOT_W = R.GetWidth();
                        Double TOT_H = R.GetHeight();

                        var imagedata = iText.IO.Image.ImageDataFactory.CreatePng(imageByte);
                        float scale = (float)595 / 1024;// 0.72F; // 300*72/Image.PhysicalDimension.Width; // 0.75F;

                        Double LabelX = X;
                        Double LabelY = Y;
                        Double LabelW = imagedata.GetWidth() * scale;
                        Double LabelH = imagedata.GetHeight() * scale;
                        float TemplateW = imagedata.GetWidth() * scale;
                        float TemplateH = imagedata.GetHeight() * scale;


                        if (Percent)
                        {
                            LabelX = (LabelX / 100 * TOT_W);
                            LabelY = (TOT_H - (LabelY / 100 * TOT_H));

                            if (LabelY - LabelH > 0)
                                LabelY = LabelY - LabelH;
                            else
                                LabelY = 0;

                            if (LabelX + LabelW > TOT_W)
                                LabelX = TOT_W - LabelW;
                        };


                        //image.SetAbsolutePosition(0, 0);


                        var R2 = new iText.Kernel.Geom.Rectangle((float)LabelX, (float)LabelY, (float)( LabelW), (float)( LabelH));


                        PdfStampAnnotation annotation = new PdfStampAnnotation(R2);
                        annotation.SetName(new PdfString(Guid.NewGuid().ToString()));

                        PdfFormXObject xObj = new PdfFormXObject(new iText.Kernel.Geom.Rectangle(TemplateW, TemplateH));
                        PdfCanvas canvas = new PdfCanvas(xObj, pdfDoc);
                        canvas.AddImageFittedIntoRectangle(imagedata, new iText.Kernel.Geom.Rectangle(TemplateW, TemplateH), false);
                        annotation.SetAppearance(PdfName.N, xObj.GetPdfObject());
                        annotation.SetFlags(PdfAnnotation.PRINT | PdfAnnotation.LOCKED | PdfAnnotation.READ_ONLY);

                        var pg = pdfDoc.GetPage(PageNumber);
                        pg.AddAnnotation(annotation);

                    mem.Flush();
                    }
                    dest.Write(mem.ToArray());
                }
            }
            return dest;
        }




        // PAGE MANAGEMENT
        public Int32 GetNumberOfPages()
        {
            fileStream.Position = 0;
            using (var pdfDoc = new PdfDocument(new PdfReader(fileStream)))
                npages = pdfDoc.GetNumberOfPages();
            return npages;
        }

        public async Task<Stream> RotatePage(Int32 PageNumber, Double Degree)
        {
            PageNumber = PageNumber <= 0 ? 1 : PageNumber;
            try
            {
                using (var mem = new MemoryStream())
                {
                    mem.Position = 0;
                    using (var pdfDoc = new PdfDocument(new PdfReader(fileStream), new PdfWriter(mem)))
                    {
                        pdfDoc.GetPage(PageNumber).SetRotation((int)Degree);
                    }
                    return await Task.FromResult(mem);
                }
            }
            catch (Exception ex)
            {
                //                Log4NetHelper.GetLogger("APP").Error("PdfProperty.Rotate", ex);
            }
            return null;
        }

        public async Task<Stream> AddRasterPage(int PageNumber, byte[] PageData)
        {
            try
            {
                var dest = new MemoryStream();
                var mem = new MemoryStream();
                {
                    mem.Position = 0;
                    using (var pdfDoc = fileStream != null ? new PdfDocument(new PdfReader(fileStream), new PdfWriter(mem)) : new PdfDocument(new PdfWriter(mem)))
                    {
                        pdfDoc.SetCloseWriter(false);

                        var image = iText.IO.Image.ImageDataFactory.Create(PageData,true);
                        //var size = pdfDoc.GetDefaultPageSize();
                        var size = new PageSize(image.GetWidth(), image.GetHeight());
                        //image.SetWidth(size.GetWidth());
                        //image.SetHeight(size.GetHeight());
                        iText.Layout.Element.Image img = new iText.Layout.Element.Image(image);
                        img.SetFixedPosition(0, 0);
                        PdfPage pagina = pdfDoc.AddNewPage(PageNumber, size);
                        Document doc = new Document(pagina.GetDocument());
                        {
                            doc.SetMargins(0, 0, 0, 0);
                            doc.Add(img);
                            doc.Flush();
//                            var w = pdfDoc.GetWriter();
//                            w.Position = 0;
//                            await w.CopyToAsync(dest);
                            //w.Close();
                            doc.Close();
                        }
                    }
                    mem.Flush();
                    return mem;
                }
            }
            catch (Exception ex)
            {
                //                Log4NetHelper.GetLogger("APP").Error("PdfProperty.AddPage", ex);
            }
            return null;
        }

        public async Task<Stream> ReplaceWithRasterPage(int PageNumber, byte[] PageData)
        {
            try
            {
                using (var pdfDoc = new PdfDocument(new PdfReader(fileStream)))
                {
                    pdfDoc.RemovePage(PageNumber);

                    using (Document doc = new Document(pdfDoc))
                    {

                        var image = iText.IO.Image.ImageDataFactory.CreatePng(PageData);
                        image.SetWidth(pdfDoc.GetDefaultPageSize().GetWidth() - 50);

                        PdfPage pagina = pdfDoc.AddNewPage(PageNumber);

                        pdfDoc.AddPage(PageNumber, pagina);
                    }
                }
            }
            catch (Exception ex)
            {
                //               Log4NetHelper.GetLogger("APP").Error("PdfProperty.ReplacePage", ex);
            }
            return await Task.FromResult(fileStream);
        }

        public async Task<Stream> RemovePage(int PageNumber)
        {
            PageNumber = PageNumber <= 0 ? 1 : PageNumber;
            try
            {
                using (var mem = new MemoryStream())
                {
                    mem.Position = 0;
                    using (var pdfDoc = new PdfDocument(new PdfReader(fileStream), new PdfWriter(mem)))
                    {
                        pdfDoc.RemovePage(PageNumber);
                    }
                    return await Task.FromResult(mem);
                }
            }
            catch (Exception ex)
            {
                //               Log4NetHelper.GetLogger("APP").Error("PdfProperty.RemovePage", ex);
            }
            return null;
        }



        // FIELD MANAGEMENT
        public IEnumerable<String> FieldsOnPage(int PageNumber)
        {
            using (var pdfDoc = new PdfDocument(new PdfReader(fileStream)))
            {
                PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, false);
                foreach (var item in form.GetAllFormFields())
                {
                    var pg = form.GetField(item.Key).GetWidgets().First().GetPage();
                    if (pdfDoc.GetPageNumber(pg) == PageNumber)
                        yield return item.Key;
                }
            }

        }
        public FieldDefinition Field(String FieldName, int PageNumber, bool createIfNotExist = false)
        {
            FieldDefinition F = new FieldDefinition();
            F.FieldName = FieldName;
            using (var pdfDoc = new PdfDocument(new PdfReader(fileStream)))
            {
                PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, createIfNotExist);
                foreach (var item in form.GetAllFormFields())
                {
                    var pg = form.GetField(item.Key).GetWidgets().First().GetPage();
                    if (pdfDoc.GetPageNumber(pg) == PageNumber && item.Key.ToLower() == FieldName.ToLower())
                    {
                        F.FieldValues = new string[] { item.Value.GetValueAsString() };
                    }
                }
            }
            return F;
        }



        // SIGNATURE MANAGEMENT
        public List<String> Signatures()
        {
            try
            {
                using (var pdfDoc = new PdfDocument(new PdfReader(fileStream)))
                {
                    var sign = new SignatureUtil(pdfDoc);
                    return sign.GetSignatureNames().ToList<String>();
                }
            }
            catch
            {
                return new List<String>();
            }
        }
        public PdfSignature SignatureInfo(string signature)
        {
            try
            {
                using (var pdfDoc = new PdfDocument(new PdfReader(fileStream)))
                {
                    var sign = new SignatureUtil(pdfDoc);
                    return sign.GetSignature(signature);
                }
            }
            catch
            {
                return null;
            }
        }

        //public List<String> BlankSignatures()
        //{
        //    try
        //    {
        //        using (var pdfDoc = new PdfDocument(new PdfReader(fileStream)))
        //        {
        //            var sign = new SignatureUtil(pdfDoc);
        //            return sign.GetBlankSignatureNames().ToList<String>();
        //        }
        //    }
        //    catch
        //    {
        //        return new List<String>();
        //    }
        //}

        public List<FieldPosition> BlankSignatures()
        {
            var list = new List<FieldPosition>();
            try
            {
                using (var pdfDoc = new PdfDocument(new PdfReader(fileStream)))
                {
                    var sign = new SignatureUtil(pdfDoc);
                    var acroForm = PdfAcroForm.GetAcroForm(pdfDoc, pdfDoc.GetWriter() != null);
                    if (acroForm != null)
                    {
                        foreach (var sname in sign.GetBlankSignatureNames())
                        {
                            var f = acroForm.GetField(sname);
                            if (f != null)
                            {
                                var w = f.GetWidgets().First();
                                var rect = w.GetRectangle();
                                var field = new FieldPosition()
                                {
                                    Description = f.GetDisplayValue(), 
                                    Name = sname,
                                    Page = pdfDoc.GetPageNumber(w.GetPage()),
                                    Left = (decimal)rect.GetAsNumber(0).FloatValue(),
                                    Bottom = (decimal)rect.GetAsNumber(1).FloatValue(),
                                    Right = (decimal)rect.GetAsNumber(2).FloatValue(),
                                    Top = (decimal)rect.GetAsNumber(3).FloatValue()
                                };
                                if (string.IsNullOrEmpty(field.Description))
                                    field.Description = field.Name;
                                list.Add(field);
                            }
                        }
                    }
                    return list;
                }
            }
            catch
            {
            }
            return list;
        }

        protected const int Default_Page_Width = 1200;
        protected const int  Default_Page_Height = 1698;

        public Stream AddBlankSignatures(List<FieldPosition> Signatures)
        {
            try
            {
                var dest = new MemoryStream();
                using (var mem = new MemoryStream())
                {
                    mem.Position = 0;

                    using (var reader = new PdfReader(fileStream))
                    {
                        using (var pdfDoc = new PdfDocument(reader, new PdfWriter(mem)))
                        {
                            int i = 0;
                            iText.Kernel.Colors.Color rosso = new DeviceRgb(255, 0, 0);
                            foreach (var s in Signatures)
                            {
                                i++;


                                var R = pdfDoc.GetPage(s.Page).GetPageSizeWithRotation();
                                var X = (float)(s.Left / 100 /*Default_Page_Width*/) * R.GetWidth();
                                var Y = R.GetHeight() - (float)(s.Top / 100/*Default_Page_Height*/ /*1132*/) * R.GetHeight();
                                var Width = (float)((s.Right - s.Left) / 100 /*Default_Page_Width*/) * R.GetWidth();
                                var Height = (float)((s.Bottom - s.Top) / 100 /*Default_Page_Height*/ /* 1132*/ ) * R.GetHeight();

                                iText.Kernel.Geom.Rectangle rect = new iText.Kernel.Geom.Rectangle(X, Y - Height, Width, Height);
                          
                                var name = s.Name;
                                if (string.IsNullOrEmpty(name)) name = "Firma" + i.ToString();
                                var sign = new SignatureFormFieldBuilder(pdfDoc, name);
                                sign.SetPage(s.Page);
                                sign.SetWidgetRectangle(rect);
                                PdfSignatureFormField signField = sign.CreateSignature();
                                signField.SetColor(rosso);
                                PdfAcroForm.GetAcroForm(pdfDoc, true).AddField(signField, pdfDoc.GetPage(s.Page));
                            }
                            mem.Flush();
                            pdfDoc.Close();
                            dest.Write(mem.ToArray());
                        }
                    }
                    return dest;
                }
            }
            catch
            {

                return null;
            }
        }
        public Stream AddBlankSignature(String Name, Int32 Page, float x, float y, float width, float height)
        {
            try
            {
                var dest = new MemoryStream();
                using (var mem = new MemoryStream())
                {
                    mem.Position = 0;
                    fileStream.Position = 0;
                    using (var pdfDoc = new PdfDocument(new PdfReader(fileStream), new PdfWriter(mem)))
                    {
                        //var sign = new SignatureUtil(pdfDoc);

                        var R = pdfDoc.GetPage(Page).GetPageSizeWithRotation();


                        var Width = (width / 100) * R.GetWidth();
                        var Height = (height / 100) * R.GetHeight();

                        var X = (x / 100) * R.GetWidth();
                        var Y = R.GetHeight() - (y / 100) * R.GetHeight();





                        iText.Kernel.Geom.Rectangle rect = new iText.Kernel.Geom.Rectangle(X, Y - Height, Width, Height);
                        iText.Kernel.Colors.Color rosso = new DeviceRgb(255, 0, 0);

                        PdfSignatureFormField signField = new SignatureFormFieldBuilder(pdfDoc, Name)
                            .SetPage(Page)
                            .SetWidgetRectangle(rect)
                            .CreateSignature();
                        signField.SetColor(rosso);
                        PdfAcroForm.GetAcroForm(pdfDoc, true).AddField(signField, pdfDoc.GetPage(Page));
                        mem.Flush();
                        pdfDoc.Close();
                        dest.Write (mem.ToArray());
                    }
                    return dest;
                }
            }
            catch
            {
            }
            return null;
        }
        
        public Stream RemoveBlankSignField(String Name)
        {
            try
            {
                using (var mem = new MemoryStream())
                {
                    mem.Position = 0;

                    using (var pdfDoc = new PdfDocument(new PdfReader(fileStream), new PdfWriter(mem)))
                    {
                        PdfAcroForm.GetAcroForm(pdfDoc, true).RemoveField(Name);
                    }
                    return mem;
                }
            }
            catch
            {
            }
            return null;
        }

        public Stream AddSignature(String Name, int PageNumber, float x, float y, float width, float height)
        {
            try
            {
                var mem = AddBlankSignature(Name, PageNumber, x, y, width, height);
                if (mem != null)
                {
                    mem.Position = 0;
                    return mem;
                }
            }
            catch
            {
            }

            return null;
        }
        public Stream AddSignature(String Name)
        {
            return AddSignature(Name, 1, -1, -1, -1, -1);
        }


    }
}
