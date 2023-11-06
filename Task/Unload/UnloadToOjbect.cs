using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Task.Exceptions;
using Task.Models;
using Task.Verefication;

namespace Task.Unload
{
    public class UnloadToOjbect
    {
        private readonly ILogger _logger;

        public UnloadToOjbect(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Возвращает входной файл в виде сущности.
        /// </summary>
        /// <param name="inputFile">Входной файл.</param>
        /// <returns></returns>
        public Group Unload(string inputFile)
        {
            List<Subject> subjects;

            List<Student> students = new List<Student>();

            Group group = new Group("ИП-41");

            DataVereficator dataVereficator = new DataVereficator(_logger);

            try
            {
                _logger.Information($"Open {inputFile} for reading and parsing data.");

                using (StreamReader streamReader = new StreamReader(inputFile, Encoding.Default))
                {
                    _logger.Information($"Read info about titles from {inputFile} and validate this.");
                    string[] headers = streamReader.ReadLine().Split(';');

                    dataVereficator.CheckInformationAboutHeaders(headers);
                    string line;
                    string[] studentInformation;


                    _logger.Information($"Read info about studens from {inputFile} and validate this.");
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        try
                        {
                            studentInformation = line.Split(';');

                            subjects = dataVereficator.CheckInformationAboutStudent(studentInformation, headers);

                            students.Add(new Student(studentInformation[0], subjects));
                        }
                        catch (CsvFormatException e)
                        {
                            _logger.Error("Invalid input.\n" +
                                e.Message);
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                _logger.Error($"File with name {inputFile} not found." +
                    e.Message);
                throw;
            }

            group.AddStudents(students);

            return group;
        }
    }
}
