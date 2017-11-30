using System;
using System.Linq;
using ConsoleTables;
using StudentManager.Controllers;
using StudentManager.Models.BaseModels;

namespace StudentManager.Models
{
    public class Score : EasyModel
    {
        [IgnoreInput]
        public int Id { get; set; }

        [IgnoreInput]
        public int CourseId { get; set; }

        [IgnoreInput]
        public int StudentId { get; set; }

        public double StudentScore { get; set; }

        public override void Input()
        {
            Id = DataProvider.GetInstance.Scores.Count + 1;

            Console.WriteLine("\tClasses available : ");
            ConsoleTable.From(DataProvider.GetInstance.Classes).Write();
            Console.Write("\tSelect class from list : ");
            var classId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\tCourses in class : ");
            var courses = DataProvider.GetInstance.Courses
                .Join(DataProvider.GetInstance.Classes, course => course.ClassId, @class => @class.Id,
                    (course, @class) => new {course, @class})
                .Join(DataProvider.GetInstance.Subjects, t => t.course.SubjectId, subject => subject.Id,
                    (t, subject) => new {t, subject})
                .Join(DataProvider.GetInstance.Teachers, t => t.t.course.TeacherId, teacher => teacher.Id,
                    (t, teacher) => new {t, teacher})
                .Where(t => t.t.t.course.ClassId == classId)
                .Select((t,i) => new
                {
                    Id = i+1,
                    CourseId = t.t.t.course.Id,
                    ClassName = t.t.t.@class.Name,
                    SubjectName = t.t.subject.Name,
                    TeacherName = t.teacher.Name
                }).ToList();

            ConsoleTable.From(courses).Write();
            Console.Write("\tSelect course from list : ");
            CourseId = courses[Convert.ToInt32(Console.ReadLine())-1].CourseId;

            Console.WriteLine("\tStudents in class : ");
            var students = DataProvider.GetInstance.Students.Where(s => s.ClassId == classId).Select((s, i) => new
            {
                Id = i + 1,
                StudentId = s.Id,
                s.Name
            }).ToList();
            ConsoleTable.From(students).Write();
            Console.Write("\tSelect student from list : ");
            StudentId = students[Convert.ToInt32(Console.ReadLine())-1].StudentId;
            base.Input();
        }
    }
}