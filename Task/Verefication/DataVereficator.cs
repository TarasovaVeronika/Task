using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using Task.Exceptions;
using Task.Models;

namespace Task.Verefication
{

    public class DataVereficator
    {
        private readonly ILogger _logger;

        public DataVereficator(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Проверяем на корректность заголовки таблицы.
        /// </summary>
        /// <param name="headers">Заголовоки таблицы.</param>
        public void CheckInformationAboutHeaders(string[] headers)
        {
            foreach (var header in headers)
            {
                if ((!header.All(word => Char.IsLetter(word) || word.Equals('.'))) || header.Length < 1)
                {
                    _logger.Warning($"Invalid header : {header}.");
                }
            }
        }

        /// <summary>
        /// Возвращает список предметов и оценок по ним у конкретного студента.
        /// </summary>
        /// <param name="studentInformation">Информация о студенте.</param>
        /// <param name="headers">Названия предметов.</param>
        /// <returns></returns>
        public List<Subject> CheckInformationAboutStudent(string[] studentInformation, string[] headers)
        {
            string name = studentInformation[0];
            int amountOfMarks = studentInformation.Length;

            List<int> marks = new List<int>();
            List<Subject> subjects = new List<Subject>();
            CheckInformationAboutStudentName(name);

            for (int i = 1; i < amountOfMarks; i++)
            {
                marks = CheckInformationAboutStudentMarks(name, studentInformation[i].Split(','), headers[i]);
                subjects.Add(new Subject(headers[i], marks));
            }
            return subjects;
        }

        private List<int> CheckInformationAboutStudentMarks(string name, string[] marks, string subject)
        {
            if (marks.Length < 1)
            {
                throw new CsvFormatException($"{name} has not mark - {subject}.");
            }

            int result;

            List<int> parseListOfMarks = new List<int>();

            for (int i = 0; i < marks.Length; i++)
            {
                if (int.TryParse(marks[i], out result))
                {
                    if ((result > 10) || (result < 0))
                    {
                        throw new CsvFormatException($"{name} invalid format: {marks[i]} - {subject}");
                    }
                    else
                    {
                        parseListOfMarks.Add(result);
                    }
                }
                else
                {
                    throw new CsvFormatException($"{name} ivalid format: {marks[i]} - {subject}");
                }
            }

            return parseListOfMarks;
        }

        private void CheckInformationAboutStudentName(string name)
        {
            if (!name.All(word => Char.IsLetter(word) || Char.IsWhiteSpace(word) || word.Equals('.')))
            {
                _logger.Warning($"Invalid name: {name}");
            }

            if (name.All(word => Char.IsLetter(word) || word.Equals('.')))
            {
                throw new CsvFormatException($"Student : {name} has not a name.");
            }
        }
    }
}
