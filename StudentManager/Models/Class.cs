using System;
using ConsoleTables;
using StudentManager.Controllers;
using StudentManager.Models.BaseModels;

namespace StudentManager.Models
{
    public class Class :EasyModel
    {
        [IgnoreInput]
        public int Id { get; set; }
        public string Name { get; set; }

        [IgnoreInput]
        public int TeacherId { get; set; }

        public override void Input()
        {
            base.Input();
            Id = DataProvider.GetInstance.Classes.Count + 1;
            Console.WriteLine("\tTeachers available : ");
            ConsoleTable.From(DataProvider.GetInstance.Teachers).Write();
            Console.Write("\tSelect teacher from list : ");
            TeacherId = Convert.ToInt32(Console.ReadLine());
        }
    }
}