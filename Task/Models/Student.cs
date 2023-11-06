using System;
using System.Collections.Generic;
using System.Text;

namespace Task.Models
{
    public class Student
    {
        public string Name { get; private set; }
        public List<Subject> Subjects { get; set; }
        public double AverageMark => StudentCalculation.AverageMark(this);
        public Student(string name, List<Subject> subjects)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Subjects = subjects ?? throw new ArgumentNullException(nameof(subjects));
        }
        public override string ToString()
        {
            StringBuilder marks = new StringBuilder();
            foreach (var subject in Subjects)
            {
                foreach (var mark in subject.Marks)
                {
                    marks.Append(mark).Append(" ");
                }
                marks.Append(";");
            }
            return string.Format($"{Name}\t{marks}\t{StudentCalculation.AverageMark(this)}");
        }
    }

    public static class StudentCalculation
    {
        public static double AverageMark(this Student student)
        {
            double result = 0;

            foreach (var subject in student.Subjects)
            {
                result += SubjectCalculation.AverageMark(subject);
            }
            return result / student.Subjects.Count;
        }
    }

    public static class StudentRepository
    {
        public static void AddSubject(this Student student, List<Subject> subjects)
        {
            if (subjects is null)
            {
                throw new ArgumentNullException(nameof(subjects));
            }
            student.Subjects = subjects;
        }
    }
}
