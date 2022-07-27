using Aspose.OCR;
using ConvertApiDotNet;
using ConvertApiDotNet.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var directory = @"C:\Users\User\Desktop\AllFiles";
            var outputFolder = @"C:\Users\User\Desktop\Results";
            var outputFolderForConvertedFiles = @"C:\Users\User\Desktop\ConvertedFiles";
            var pattern = "Еольшинцова";

            var service = new FileService();
            var counter = 0;

            List<string> pdfFiles = Directory.GetFiles(directory, "*.pdf", SearchOption.AllDirectories).ToList();

            Dictionary<int, BooksData> pageInfos = new Dictionary<int, BooksData>();

            Console.WriteLine("{0} pdf files found", pdfFiles.Count);

            List<string> djvuFiles = Directory.GetFiles(directory, "*.djvu", SearchOption.AllDirectories).ToList();

            Console.WriteLine("{0} djvu files found", djvuFiles.Count);
            
            foreach (var file in djvuFiles)
            {
                await service.Converter(file, outputFolderForConvertedFiles);
            }

            List<string> convertedFiles = Directory.GetFiles(directory, "*.pdf", SearchOption.AllDirectories).ToList();

            foreach (var file in convertedFiles)
            {
                using (PdfReader reader = new PdfReader(file))
                {
                    var res = service.FileHandler(reader, pattern, file);
                    foreach (var item in res)
                    {
                        pageInfos.Add(counter, item);
                        counter++;
                    }
                }
            }

            foreach (var file in pdfFiles)
            {
                using (PdfReader reader = new PdfReader(file))
                {
                    var res = service.FileHandler(reader, pattern, file);
                    foreach (var item in res)
                    {
                        pageInfos.Add(counter, item);
                        counter++;
                    }
                }
            }

            foreach (var info in pageInfos)
            {
                try
                {
                    service.StoreRersult(outputFolder, info.Value.FileName, info.Value.Page, info.Key);
                    service.PercentCalculatorFormatter(info.Key, pageInfos.Count);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Something went wrong with file: {0}, page {1}", info.Value.FileName, info.Value.Page);
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("Done");
            
        }
    }
}
