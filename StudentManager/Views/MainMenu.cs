using System;
using StudentManager.Controllers;

namespace StudentManager.Views
{
    public class MainMenu : Menu
    {
        public MainMenu(DataProvider data) : base(data)
        {
        }

        public override void DrawMenu()
        {
            int choice;
            do
            {
                Console.Clear();
                Console.WriteLine("Student Manager Main Menu");
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine("1 |\tNhap du lieu");
                Console.WriteLine("2 |\tHien thi du lieu");
                Console.WriteLine("3 |\tTim kiem du lieu");
                Console.WriteLine("4 |\tBao cao");
                Console.WriteLine("5 |\tExit");
                Console.Write("\nChon thao tac : ");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        new MenuInput(this.Data).DrawMenu();
                        break;
                    case 2:
                        new MenuShow(this.Data).DrawMenu();
                        break;
                    case 3:
                        new MenuSearch(this.Data).DrawMenu();
                        break;
                    case 4:
                        new MenuReport(this.Data).DrawMenu();
                        break;
                    case 5:
                        break;
                    default:
                        Console.WriteLine("Nhap sai, nhap lai.");
                        break;
                }
            } while (choice != 5);
        }
    }
}