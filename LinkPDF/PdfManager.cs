using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Extgstate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LinkPDF
{
    class PdfManager
    {
        public void Stamp(string[] sources, string stampFile,float opacity,float x, float y)
        {
            
            byte[] imgdata = File.ReadAllBytes(stampFile);

            ImageData img = ImageDataFactory.Create(imgdata);
            if (sources.Count() > 2)
            {
                foreach (var item in sources.Skip(2))
                {
                    if (File.Exists(item))
                    {
                        using (PdfDocument pdf = new PdfDocument(new PdfReader(item), new PdfWriter(item.Replace(".pdf", "_stamp.pdf"))))
                        {
                      
                            for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
                            {
                                    PdfPage page = pdf.GetPage(i);
                                    PdfExtGState tranState = new PdfExtGState();
                                    tranState.SetFillOpacity(opacity);
                                    PdfCanvas canvas = new PdfCanvas(pdf.GetPage(i));
                                    canvas.SaveState().SetExtGState(tranState);
                                    canvas.AddImage(img, x, y, false);
                                    canvas.RestoreState();
                            } 
                        }
                    }
                }
            }
        }

        public void ManipulatePdf(String dest, string[] sources)
        {

            PdfDocument pdfDoc = new PdfDocument(new PdfReader(sources[1]), new PdfWriter(dest));
            if (sources.Count() > 2)
            {
                foreach (var item in sources.Skip(2))
                {
                    if (File.Exists(item))
                    {
                        PdfDocument insertDoc = new PdfDocument(new PdfReader(item));
                        insertDoc.CopyPagesTo(1, insertDoc.GetNumberOfPages(), pdfDoc);
                        insertDoc.Close();
                    }
                }
            }

            pdfDoc.Close();

        }

        public void FindTextInPdf(string SearchStr, string[] sources)
        {
            if (sources.Count() > 2)
            {
                foreach (var item in sources.Skip(2))
                {
                    if (File.Exists(item))
                    {
                        using (PdfReader reader = new PdfReader(item))
                        using (var doc = new PdfDocument(reader))
                        {

                            var pageCount = doc.GetNumberOfPages();

                            for (int i = 1; i <= pageCount; i++)
                            {
                                PdfPage page = doc.GetPage(i);
                                var box = page.GetCropBox();
                                var rect = new Rectangle(box.GetX(), box.GetY(), box.GetWidth(), box.GetHeight());

                                var filter = new IEventFilter[1];
                                    filter[0] = new TextRegionEventFilter(rect);


                                ITextExtractionStrategy strategy = new FilteredTextEventListener(new LocationTextExtractionStrategy(), filter);

                                var str = PdfTextExtractor.GetTextFromPage(page, strategy);
                                if (str.Contains(SearchStr) == true)
                                {
                                    Console.WriteLine("Searched text found in file:[ " + item + " ] page : [ " + i + " ]");
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
