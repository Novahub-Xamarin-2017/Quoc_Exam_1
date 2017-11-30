using System;
using StudentManager.Controllers;

namespace StudentManager.Views
{
    public class MenuInput : Menu
    {
        public MenuInput(DataProvider data) : base(data)
        {
        }

        public override void DrawMenu()
        {
            int choice;
            do
            {
                Console.Clear();
                Console.WriteLine("Student Manager Input Data");
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine("1 |\tNhap danh sach mon hoc");
                Console.WriteLine("2 |\tNhap danh sach giao vien");
                Console.WriteLine("3 |\tNhap danh sach lop hoc");
                Console.WriteLine("4 |\tNhap danh sach sinh vien");
                Console.WriteLine("5 |\tNhap danh sach khoa hoc");
                Console.WriteLine("6 |\tNhap danh sach diem");
                Console.WriteLine("7 |\tSave");
                Console.WriteLine("8 |\tSave and Exit");
                Console.Write("\nNhap thao tac : ");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Data.Subjects.AddDataInput();
                        break;
                    case 2:
                        Data.Teachers.AddDataInput();
                        break;
                    case 3:
                        Data.Classes.AddDataInput();
                        break;
                    case 4:
                        Data.Students.AddDataInput();
                        break;
                    case 5:
                        Data.Courses.AddDataInput();
                        break;
                    case 6:
                        Data.Scores.AddDataInput();
                        break;
                    case 7:
                    case 8:
                        Data.SaveData();
                        break;
                    default:
                        Console.WriteLine("Nhap sai, nhap lai.");
                        Console.ReadKey();
                        break;
                }
            } while (choice != 8);
        }
    }
}