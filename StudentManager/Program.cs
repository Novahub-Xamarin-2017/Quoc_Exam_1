using System;
using StudentManager.Controllers;
using StudentManager.Views;

namespace StudentManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = DataProvider.GetInstance;
            data.LoadData();

            var menu = new MainMenu(data);
            menu.DrawMenu();
            
            Console.ReadKey();
        }
    }
}
