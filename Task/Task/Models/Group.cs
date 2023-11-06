using System;
using System.Collections.Generic;
using System.Text;

namespace Task.Models
{
    public class Group
    {
        public string Name { get; private set; }
        public List<Student> Students { get; set; }
        public double AverageMark => GroupCalculation.AverageMark(this);

        public Group(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        public override string ToString()
        {
            StringBuilder students = new StringBuilder();
            foreach (var student in Students)
            {
                students.Append(student).Append("\n");
            }
            return string.Format($"{Name}\n{students}");
        }
    }
    public static class GroupCalculation
    {
        public static double AverageMark(this Group group)
        {
            double result = 0;
            foreach (var student in group.Students)
            {
                result += StudentCalculation.AverageMark(student);
            }
            return result / group.Students.Count;

        }
    }
    public static class GroupRepository
    {
        public static void AddStudents(this Group group, List<Student> students)
        {
            if (students is null)
            {
                throw new ArgumentNullException(nameof(students));
            }
            group.Students = students;
        }
    }
}
