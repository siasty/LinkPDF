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
        public void Stamp(string source, string stampFile)
        {
            string dest = "";
            PdfExtGState tranState = new PdfExtGState();
            tranState.SetFillOpacity(0.1f);
            byte[] imgdata = System.IO.File.ReadAllBytes(stampFile);

            ImageData img = ImageDataFactory.Create(imgdata);
            PdfReader reader = new PdfReader(source);
            dest = source.Replace(".pdf", "_s.pdf");

            PdfWriter writer = new PdfWriter(dest);
            PdfDocument pdf = new PdfDocument(reader, writer);
            for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
            {
                PdfPage page = pdf.GetPage(i);
                PdfCanvas canvas = new PdfCanvas(page);
                canvas.SaveState().SetExtGState(tranState);
                canvas.AddImage(img, 200, 300, false);
                canvas.RestoreState();

            }

            pdf.Close();

        }

        public void ManipulatePdf(String dest, string[] sources)
        {

            PdfDocument pdfDoc = new PdfDocument(new PdfReader(sources[0]), new PdfWriter(dest));
            if (sources.Count() > 1)
            {
                foreach (var item in sources.Skip(1))
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

        public void ReadTextFromPage(string str,string pdfPath)
        {
            using (PdfReader reader = new PdfReader(pdfPath))
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

                    var str1 = PdfTextExtractor.GetTextFromPage(page, strategy);


                    Console.WriteLine(str1);


                }
            }

        }
    }
}
