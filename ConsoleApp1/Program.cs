using ConvertApiDotNet;
using ConvertApiDotNet.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var directory = @"C:\Users\User\Desktop\pdfFiles";
            var outputFolder = @"C:\Users\User\Desktop\pdfFiles\Results";
            //var pattern = "JavaScript";

            var service = new FileService();
            //var counter = 0;

            //List<string> files = Directory.GetFiles(directory).ToList();

            //Dictionary<int, BooksData> pageInfos = new Dictionary<int, BooksData>();

            //Console.WriteLine("{0} files found", files.Count);

            //foreach (var file in files)
            //{
            //    using (PdfReader reader = new PdfReader(file))
            //    {
            //        var res = service.FileHandler(reader, pattern, file);
            //        foreach (var item in res)
            //        {
            //            pageInfos.Add(counter, item);
            //            counter++;
            //        }
            //    }
            //}

            //foreach (var info in pageInfos)
            //{
            //    try
            //    {
            //        service.StoreRersult(outputFolder, info.Value.FileName, info.Value.Page, info.Key);
            //        service.PercentCalculatorFormatter(info.Key, pageInfos.Count);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Something went wrong with file: {0}, page {1}", info.Value.FileName, info.Value.Page);
            //        Console.WriteLine(ex.Message);
            //    }
            //}


            List<string> files = Directory.GetFiles(directory).ToList();
            foreach (var file in files)
            {
                //Need to be think about some temp directory for new converted files
                service.Converter(file, outputFolder)
            }
            // Set conversion parameters for PDF format

        }
    }
}
