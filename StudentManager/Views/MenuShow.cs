using System;
using System.Linq;
using ConsoleTables;
using StudentManager.Controllers;

namespace StudentManager.Views
{
    public class MenuShow : Menu
    {
        public MenuShow(DataProvider data) : base(data)
        {
        }

        public override void DrawMenu()
        {
            int choice;
            do
            {
                Console.Clear();
                Console.WriteLine("Student Manager Show Data");
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine("1 |\tHien thi danh sach mon hoc");
                Console.WriteLine("2 |\tHien thi danh sach giao vien");
                Console.WriteLine("3 |\tHien thi danh sach lop hoc");
                Console.WriteLine("4 |\tHien thi danh sach sinh vien theo lop");
                Console.WriteLine("5 |\tExit");
                Console.Write("\nChon thao tac : ");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        ConsoleTable.From(Data.Subjects).Write();
                        Console.ReadKey();
                        break;
                    case 2:
                        ShowTeachers();
                        break;
                    case 3:
                        ShowClasses();
                        break;
                    case 4:
                        ShowStudents();
                        break;
                    case 5:
                        break;
                }
            } while (choice != 5);
        }


        private void ShowStudents()
        {
            var students = Data.Students.Join(Data.Classes, student => student.ClassId, c => c.Id, (student, c) => new
            {
                student.Id,
                StudentName = student.Name,
                ClassName = c.Name,
                student.Address,
                student.BirthDate
            }).ToList();
            ConsoleTable.From(students).Write();

            Console.WriteLine("Chon 1 hoc sinh (Id) : ");
            var studentId = Convert.ToInt32(Console.ReadLine());
            var studentScores = Data.Scores
                .Join(Data.Students, score => score.StudentId, student => student.Id,
                    (score, student) => new {score, student})
                .Join(Data.Courses, t => t.score.CourseId, course => course.Id, (t, course) => new {t, course})
                .Join(Data.Subjects, t => t.course.SubjectId, subject => subject.Id,
                    (t, subject) => new {t, subject})
                .Where(t => studentId == t.t.t.student.Id)
                .Select((t,i) => new
                {
                    Id = i+1,
                    SubectName = t.subject.Name,
                    t.t.t.score.StudentScore,
                }).ToList();
            ConsoleTable.From(studentScores).Write();
            Console.ReadKey();
        }

        private void ShowClasses()
        {
            var classes = Data.Classes
                .Join(Data.Teachers, dataClass => dataClass.TeacherId, teacher => teacher.Id,
                    (dataClass, teacher) => new {dataClass, teacher})
                .GroupJoin(Data.Students, t => t.dataClass.Id, student => student.ClassId, (t, g) => new
                {
                    t.dataClass.Id,
                    ClassName = t.dataClass.Name,
                    MainTeacher = t.teacher.Name,
                    StudentCount = g.Count()
                });
            ConsoleTable.From(classes).Write();

            Console.Write("Chon 1 lop (ID) : ");
            var classId = Convert.ToInt32(Console.ReadLine());
            var courses = Data.Courses
                .Join(Data.Classes, course => course.ClassId, c => c.Id, (course, c) => new {course, c})
                .Join(Data.Subjects, t => t.course.SubjectId, subject => subject.Id,
                    (t, subject) => new {t, subject})
                .Join(Data.Teachers, t => t.t.course.TeacherId, teacher => teacher.Id,
                    (t, teacher) => new {t, teacher})
                .Where(t => classId == t.t.t.c.Id)
                .Select((t, i) => new
                {
                    Id = i + 1,
                    CourseId = t.t.t.course.Id,
                    ClassName = t.t.t.c.Name,
                    SubjectName = t.t.subject.Name,
                    TeacherName = t.teacher.Name
                }).ToList();
            ConsoleTable.From(courses).Write();

            Console.Write("Chon mot khoa hoc : ");
            var courseId = courses[Convert.ToInt32(Console.ReadLine()) - 1].CourseId;
            var courseScore = Data.Students
                .Join(Data.Scores, student => student.Id, score => score.StudentId,
                    (student, score) => new { student, score })
                .Where(t => t.score.Id == courseId)
                .Select((t, i) => new
                {
                    Id = i + 1,
                    t.student.Name,
                    t.score.StudentScore
                });
            ConsoleTable.From(courseScore).Write();
            Console.ReadKey();
        }

        private void ShowTeachers()
        {
            var teachers = from teacher in Data.Teachers
                join dataClass in Data.Classes on teacher.Id equals dataClass.TeacherId
                join subject in Data.Subjects on teacher.SubjectId equals subject.Id
                join student in Data.Students on dataClass.Id equals student.ClassId
                               into g
                           select new
                           {
                               teacher.Id,
                               TeacherName = teacher.Name,
                               teacher.BirthDate,
                               ClassName = dataClass.Name,
                               SubjectName = subject.Name,
                               StudentCount = g.Count()
                           };
            ConsoleTable.From(teachers).Write();

            Console.Write("Chon mot ong giao vien : ");
            var teacherId = Convert.ToInt32(Console.ReadLine());
            var courses = Data.Courses
                .Join(Data.Classes, course => course.ClassId, c => c.Id, (course, c) => new {course, c})
                .Join(Data.Subjects, t => t.course.SubjectId, subject => subject.Id,
                    (t, subject) => new {t, subject})
                .Join(Data.Teachers, t => t.t.course.TeacherId, teacher => teacher.Id,
                    (t, teacher) => new {t, teacher})
                .Where(t => teacherId == t.teacher.Id)
                .Select((t,i) => new
                {
                    Id = i+1,
                    CourseId = t.t.t.course.Id,
                    ClassName = t.t.t.c.Name,
                    SubjectName = t.t.subject.Name,
                    TeacherName = t.teacher.Name
                }).ToList();
            ConsoleTable.From(courses).Write();

            Console.Write("Chon mot khoa hoc : ");
            var courseId = courses.ToList()[Convert.ToInt32(Console.ReadLine()) - 1].CourseId;
            var courseScore = Data.Students
                .Join(Data.Scores, student => student.Id, score => score.StudentId,
                    (student, score) => new {student, score})
                .Where(t => t.score.Id == courseId)
                .Select((t,i) => new
                {
                    Id = i+1,
                    t.student.Name,
                    t.score.StudentScore
                });
            ConsoleTable.From(courseScore).Write();
            Console.ReadKey();
        }
    }
}