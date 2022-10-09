using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System.IO;
using System;

namespace PDFSplitter
{
    class Program 
    {

        static int Main(string[] args)
        {
            try
            {
                Console.WriteLine("Non-commercional use only.");
                Console.WriteLine("---===GreyCat Rules===---");
                Console.WriteLine("This program will split all existing PDFs in this location into separate folders");
                
                Console.WriteLine("");

                Console.WriteLine("Press any key to proceed:");
                Console.WriteLine("Press the Escape (Esc) key to quit:\n");
                var key = Console.ReadKey();
                if(key.Key == ConsoleKey.Escape)
                {
                    return 0;
                }
                string[] AllFoundPdf = Directory.GetFiles(@".\", "*.pdf");
                foreach (var SinglePdf in AllFoundPdf)
                {
                    var fileName = SinglePdf.ToString().Substring(2, SinglePdf.ToString().Length - 6);

                    var DestinationFolder = @".\Splitted_" + fileName;
                    Directory.CreateDirectory(DestinationFolder);


                    using (var pdfDoc = new PdfDocument(new PdfReader(SinglePdf.ToString())))
                    {
                        var splitter = new CustomSplitter(pdfDoc, DestinationFolder, fileName);
                        var splittedDocs = splitter.SplitByPageCount(1);

                        foreach (var splittedDoc in splittedDocs)
                        {
                            splittedDoc.Close();
                        }
                    }
                }
            Console.WriteLine("");
            Console.WriteLine("PDFs should be now separated");
            return 0;
            
            } catch (Exception e) {
                Console.WriteLine("Something went wrong. Exception - " + e.ToString());
                return 0;
            }

        }
    }

    class CustomSplitter : PdfSplitter
    {
        private int _order;
        private readonly string _destinationFolder;
        private readonly string _destinationFileName;
        
        public CustomSplitter(PdfDocument pdfDocument, string destinationFolder, string destinationFileName) : base(pdfDocument)
        {
            _destinationFileName = destinationFileName;
            _destinationFolder = destinationFolder;
            _order = 0;
        }
        
        protected override PdfWriter GetNextPdfWriter(PageRange documentPageRange)
        {
            return new PdfWriter(_destinationFolder + @"/" + _destinationFileName +  _order++ + ".pdf");
        }
    }
}
