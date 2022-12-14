using ConvertApiDotNet;
using ConvertApiDotNet.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class FileService
    {
        public List<BooksData> FileHandler(PdfReader reader, string pattern, string file)
        {
            List<BooksData> pages = new List<BooksData>();

            for (int i = 1; i < reader.NumberOfPages; i++)
            {
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                string text = PdfTextExtractor.GetTextFromPage(reader, i, strategy);
                if (text.Contains(pattern))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("File Name");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" - {0}", System.IO.Path.GetFileName(file));
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" Page №");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(": {0}", i);
                    pages.Add(new BooksData { FileName = file, Page = i });
                }
            }

            return pages;
        }

        public void StoreRersult(string outputFolder, string fileName, int numberOfPage, int counter)
        {
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            using (PdfReader reader = new PdfReader(fileName))
            {
                Document document = new Document();

                var year = FileNameHelper(fileName);
                if (!Directory.Exists($"{outputFolder}/{year}"))
                {
                    Directory.CreateDirectory($"{outputFolder}/{year}");
                }
                var destinationFolder = $"{outputFolder}/{year}";

                PdfCopy pdfCopyFicha = new PdfCopy(document, new FileStream($"{destinationFolder}\\{System.IO.Path.GetFileNameWithoutExtension(fileName)}({counter}).pdf", FileMode.Create));
                document.Open();

                pdfCopyFicha.AddPage(pdfCopyFicha.GetImportedPage(reader, numberOfPage));
                document.Close();
            }
        }

        public void PercentCalculatorFormatter(int pageCounter, int totalPages)
        {
            var percent = pageCounter * 100 / totalPages;
            var left = totalPages - pageCounter;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Total : {0}, left: {1}", totalPages, left);
            Console.WriteLine("{0} %", percent);
        }

        public async Task Converter(string file, string outputFolder)
        {
            ConvertApi convertApi = new ConvertApi("Cr5GWW7jjOfplyVj");
            //var fileName = System.IO.Path.GetFileName(file);
            var fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(file);
            var convert = await convertApi.ConvertAsync("djvu", "pdf",
                        new ConvertApiFileParam(file)
                        );
            var year = FileNameHelper(file);
            if (!Directory.Exists($"{outputFolder}/{year}"))
            {
                Directory.CreateDirectory($"{outputFolder}/{year}");
            }
            var destinationFolder = $"{outputFolder}/{year}";
            await convert.SaveFileAsync($"{destinationFolder}\\{fileNameWithoutExt}.pdf");
        }

        private string FileNameHelper(string file)
        {
            var regexp = @"\b\d{4}";
            Regex regex = new Regex(regexp);
            var fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(file);
            return regex.Matches(fileNameWithoutExt)[0].Value;
        }
    }
}
