using iText.Forms;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Kernel.Pdf.Xobject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LinkPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                string[] srcFileNames = { "1.pdf", "2.pdf", "3.pdf" };
                args = (string[])srcFileNames.Clone();
            }
            string name = "Merge_"+ args.Count() +"_" + DateTimeOffset.Now.ToString("yyyyMMdd_HHmmss_fff")+ ".pdf";
            ManipulatePdf(name, args);
            Stamp(name, "example.jpg");
        }

        public static void ManipulatePdf(String dest,string[] sources) 
        {
               
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(sources[0]), new PdfWriter(dest));
            if( sources.Count() > 1 )
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

        public static void Stamp(string source,string stampFile)
        {
            string dest = "";
            PdfExtGState tranState = new PdfExtGState();
            tranState.SetFillOpacity(0.1f);
            byte[] imgdata = System.IO.File.ReadAllBytes(stampFile);

            ImageData img = ImageDataFactory.Create(imgdata);
            PdfReader reader = new PdfReader(source);
            dest = source.Replace(".pdf", "_s.pdf");

            PdfWriter writer = new PdfWriter(dest);
            PdfDocument pdf = new PdfDocument(reader,writer);
            for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
            {
                PdfPage page = pdf.GetPage(i);
                PdfCanvas canvas = new PdfCanvas(page);
                canvas.SaveState().SetExtGState(tranState);
                canvas.AddImage(img,200,300,false);
                canvas.RestoreState();
             
            }

            pdf.Close();

        }

      

    }
}
