using StudentManager.Controllers;

namespace StudentManager.Views
{
    public class Menu
    {
        public DataProvider Data { get; set; }

        public Menu(DataProvider data)
        {
            Data = data;
        }

        public virtual void DrawMenu() { }
    }
}