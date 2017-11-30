using System;
using System.Collections.Generic;
using StudentManager.Models.BaseModels;

namespace StudentManager
{
    public static class ListExtensions 
    {
        public static void AddDataInput<T>(this List<T> list) where T : EasyModel, new()
        {
            Console.Clear();
            Console.WriteLine("Input Data");
            Console.WriteLine("-----------------------------------------------------");
            var item = new T();
            item.Input();
            list.Add(item);
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }
    }
}
