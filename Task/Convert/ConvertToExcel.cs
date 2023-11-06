using ClosedXML.Excel;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Task.Models;

namespace Task.Convert
{
    public class ConvertToExcel
    {
        private readonly ILogger _logger;

        public ConvertToExcel(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Выгружает информацию о группе в Excel-файл формата .xlsx.
        /// </summary>
        /// <param name="outputFile">Имя выходного файла файла.</param>
        /// <param name="group">Сущность, из которой выгружаем информацию о студентах в файл.</param>
        /// <param name="inputFile">Входной файл для считывания заголовков таблицы.</param>
        /// (Залоговки таблицы и т.п).</param>
        public void Convert(string outputFile, string inputFile, Group group)
        {
            _logger.Information("Convert information from .csv to .xlsx.");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int i = 0, j = 0;

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");

            string[] headers;
            try
            {
                StreamReader streamReader;

                using (streamReader = new StreamReader(inputFile))
                {
                    headers = streamReader.ReadLine().Split(';');
                    Array.Resize(ref headers, headers.Length + 1);
                    headers[headers.Length - 1] = "GPA.";

                    for (i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = headers[i];
                    }
                }
            }
            catch (FileNotFoundException)
            {

                throw;
            }

            StringBuilder marks;
            Student student;

            for (i = 0; i < group.Students.Count; i++)
            {
                student = group.Students[i];

                worksheet.Cell(i + 2, 1).Value = student.Name;

                for (j = 0; j < student.Subjects.Count; j++)
                {
                    marks = new StringBuilder();
                    foreach (var mark in student.Subjects[j].Marks)
                    {
                        marks.Append(mark).Append(' ');
                    }
                    worksheet.Cell(i + 2, j + 2).Value = marks.ToString();
                }

                worksheet.Cell(i + 2, headers.Length).Value = student.AverageMark;
            }

            worksheet.Cell(i + 2, 1).Value = " Gruop GPA: ";
            worksheet.Cell(i + 2, headers.Length).Value = group.AverageMark();

            _logger.Information($"Saving {outputFile} - files");

            workbook.SaveAs(outputFile);

            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            _logger.Information($"Converting time: {elapsedTime}");

        }
    }
}
