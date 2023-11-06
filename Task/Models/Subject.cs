using System;
using System.Collections.Generic;
using System.Text;

namespace Task.Models
{
    public class Subject
    {
        public string Name { get; private set; }
        public List<int> Marks { get; set; }
        public double AverageMark => SubjectCalculation.AverageMark(this);
        public Subject()
        {
            Marks = new List<int>();
        }

        public Subject(string name, List<int> marks)
        {
            Name = name;
            Marks = marks ?? throw new ArgumentNullException(nameof(marks));
        }

        public override string ToString()
        {
            StringBuilder marks = new StringBuilder();
            foreach (var mark in Marks)
            {
                marks.Append(mark).Append(" ");
            }
            return string.Format($"{marks.ToString()}");
        }
    }
    public static class SubjectCalculation
    {
        public static double AverageMark(this Subject subject)
        {
            double result = 0;
            foreach (var mark in subject.Marks)
            {
                result += mark;
            }
            return result / subject.Marks.Count;
        }
    }
}
