using System;
using ConsoleTables;
using StudentManager.Controllers;
using StudentManager.Models.BaseModels;

namespace StudentManager.Models
{
    public class Teacher :EasyModel
    {
        [IgnoreInput]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }

        [IgnoreInput]
        public int SubjectId { get; set; }

        public override void Input()
        {
            Id = DataProvider.GetInstance.Teachers.Count + 1;
            base.Input();
            Console.WriteLine("\tSubjects available : ");
            ConsoleTable.From(DataProvider.GetInstance.Subjects).Write();
            Console.Write("\tSelect subject from list : ");
            SubjectId = Convert.ToInt32(Console.ReadLine());
        }
    }
}