using System;
using ConsoleTables;
using StudentManager.Controllers;
using StudentManager.Models.BaseModels;

namespace StudentManager.Models
{
    public class Student : EasyModel
    {
        [IgnoreInput]
        public int Id { get; set; }

        [PrompDisplay("Student's Name")]
        public string Name { get; set; }

        [IgnoreInput]
        [PrompDisplay("Class ID")]
        public int ClassId { get; set; }

        [PrompDisplay("Adress")]
        public string Address { get; set; }

        [PrompDisplay("BirthDate")]
        public DateTime BirthDate { get; set; }

        public override void Input()
        {
            base.Input();
            Id = DataProvider.GetInstance.Students.Count + 1;
            Console.WriteLine("\tClasses available : ");
            ConsoleTable.From(DataProvider.GetInstance.Subjects).Write();
            Console.Write("\tSelect class from list : ");
            ClassId = Convert.ToInt32(Console.ReadLine());
        }
    }
}