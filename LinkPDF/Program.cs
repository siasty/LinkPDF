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
            var PdfManager = new PdfManager();
            PdfManager.ManipulatePdf(name, args);
            PdfManager.Stamp(name, "example.jpg");
        
        }

        

        

      

    }
}
