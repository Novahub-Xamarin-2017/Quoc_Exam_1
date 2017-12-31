using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using StudentManager.Models;

namespace StudentManager.Controllers
{
    public class DataProvider
    {
        public List<Student> Students { get; set; }
        public List<Class> Classes { get; set; }
        public List<Course> Courses { get; set; }
        public List<Score> Scores { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<Teacher> Teachers { get; set; }

        private static DataProvider instance;
        public static DataProvider GetInstance => instance ?? (instance = new DataProvider());

        private DataProvider()
        {
            Students = new List<Student>();
            Classes = new List<Class>();
            Courses = new List<Course>();
            Scores = new List<Score>();
            Subjects = new List<Subject>();
            Teachers = new List<Teacher>();
        }

        public void LoadData()
        {
            Students = ReadJson<Student>("Data");
            Classes = ReadJson<Class>("Data");
            Courses = ReadJson<Course>("Data");
            Scores = ReadJson<Score>("Data");
            Subjects = ReadJson<Subject>("Data");
            Teachers = ReadJson<Teacher>("Data");
        }

        public void SaveData()
        {
            SaveJson(Students,"Data");
            SaveJson(Classes,"Data");
            SaveJson(Courses,"Data");
            SaveJson(Scores,"Data");
            SaveJson(Subjects,"Data");
            SaveJson(Teachers,"Data");
        }



        public List<T> ReadJson<T>(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var fileName = $"{typeof(T).Name}s.json";
            var filePath = Path.Combine($"../../{directory}", fileName);
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath,"[]");
            }
            var data = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<T>>(data);
        }

        public void SaveJson<T>(List<T> data, string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var fileName = $"{typeof(T).Name}s.json";
            var filePath = Path.Combine($"../../{directory}", fileName);
            var json = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, json);
        }
        

    }
}