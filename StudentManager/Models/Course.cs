using System;
using System.Linq;
using ConsoleTables;
using StudentManager.Controllers;
using StudentManager.Models.BaseModels;

namespace StudentManager.Models
{
    public class Course :EasyModel
    {
        [IgnoreInput]
        public int Id { get; set; }

        [IgnoreInput]
        public int ClassId { get; set; }

        [IgnoreInput]
        public int SubjectId { get; set; }

        [IgnoreInput]
        public int TeacherId { get; set; }

        public override void Input()
        {
            Id = DataProvider.GetInstance.Courses.Count + 1;

            Console.WriteLine("\tClasses available : ");
            ConsoleTable.From(DataProvider.GetInstance.Classes).Write();
            Console.Write("\tSelect class from list : ");
            ClassId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\tSubjects available : ");
            ConsoleTable.From(DataProvider.GetInstance.Subjects).Write();
            Console.Write("\tSelect subject from list : ");
            SubjectId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\tTeachers available : ");
            ConsoleTable.From(DataProvider.GetInstance.Teachers.Where(teacher => teacher.SubjectId == SubjectId)).Write();
            Console.Write("\tSelect teacher from list : ");
            TeacherId = Convert.ToInt32(Console.ReadLine());
        }
    }
}