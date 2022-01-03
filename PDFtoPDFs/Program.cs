using System;
using System.IO;
using System.Text.Json;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PDFtoPDFs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("O arquivo PDF a ser separado em páginas deve estar na pasta PDFToSplit dentro do projeto!!");
            Console.WriteLine();
            Console.WriteLine("Nome do arquivo a ser separado em páginas? (o nome digitado não deve conter .pdf)");
            string filename = Console.ReadLine();

            string path = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            
            string filePath = System.IO.Path.Combine(path, $@"PDFToSplit\{filename}.pdf");
            
            try
            {
                PdfReader reader = new PdfReader(filePath);

                int numberOfPages = reader.NumberOfPages;
                
                if (numberOfPages >= 1)
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        Document doc = new Document(reader.GetPageSizeWithRotation(i));
                        FileStream stream = new FileStream(System.IO.Path.Combine(path, $@"PDFSplited\{filename} - Page {i} of {reader.NumberOfPages}.pdf"), FileMode.Create);
                        PdfCopy pdfCopy = new PdfCopy(doc, stream);

                        doc.Open();
                        var importedPage = pdfCopy.GetImportedPage(reader, i);
                        pdfCopy.AddPage(importedPage);
                        doc.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Não é possível separar os arquivo, salve a mensagem de erro!");

                return;
            }

            Console.WriteLine("Arquivos criados com sucesso!! \n Os arquivos foram criados na pasta PDFSplited dentro do projeto");
            Console.WriteLine("Aperte qualquer tecla para finalizar!");
            Console.ReadKey();
        }
    }
}
