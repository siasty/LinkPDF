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
                string[] srcFileNames = { "1","1.pdf", "1.pdf", "2.pdf" };
                args = (string[])srcFileNames.Clone();
            }

            var PdfManager = new PdfManager();
    
            if (args[0] == "1") // Merge Pdf
            {
                string name = "Merge_" + (args.Count() - 1) + "_" + DateTimeOffset.Now.ToString("yyyyMMdd_HHmmss_fff") + ".pdf";
                PdfManager.ManipulatePdf(name, args);
            }
            else if (args[0] == "2") // Stamp image to pdf
            {
                PdfManager.Stamp(args, args[1] , 0.1f,200,300);
            }
            else if (args[0] == "3") // Search string in pdf
            {
                PdfManager.FindTextInPdf(args[1], args);
            }

            Console.ReadKey();
        }

        

        

      

    }
}
