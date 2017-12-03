using System;
using System.Linq;
using ConsoleTables;
using StudentManager.Controllers;

namespace StudentManager.Views
{
    public class MenuSearch :Menu
    {
        public MenuSearch(DataProvider data) : base(data)
        {
        }

        public override void DrawMenu()
        {
            int choice;
            do
            {
                Console.Clear();
                Console.WriteLine("Student Manager Search Data");
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine("1 |\tTim theo ten hoc sinh");
                Console.WriteLine("2 |\tTim theo lop");
                Console.WriteLine("3 |\tExit");
                Console.Write("\nChon thao tac : ");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        SearchByStudentName();
                        break;
                    case 2:
                        SearchByClassName();
                        break;
                    case 3:
                        break;
                }
            } while (choice != 3);
        }

        private void SearchByClassName()
        {
            Console.Clear();
            Console.WriteLine("Student Manager Search Data");
            Console.WriteLine("-----------------------------------------------------");
            Console.Write("\tNhap ten lop hoc : ");
            var className = Convert.ToString(Console.ReadLine());
            var searchedClass = Data.Classes
                .Join(Data.Teachers, c => c.TeacherId, teacher => teacher.Id, (c, teacher) => new {c, teacher})
                .Where(t => t.c.Name.ToLower().Contains(className))
                .Select((t,i) => new
                {
                    Id = i+1,
                    t.c.Name,
                    MainTeacher = t.teacher.Name,
                    ClassId = t.teacher.Id
                }).ToList();
            ConsoleTable.From(searchedClass).Write();

            Console.Write("\tChon lop (theo stt) : ");
            var classIndex = Convert.ToInt32(Console.ReadLine());
            var selectedClass = searchedClass.Single(c => c.Id == classIndex);
            Console.WriteLine($"Danh sach khoa hoc trong lop {selectedClass.Name}");
            var courses = Data.Courses
                .Join(Data.Subjects, course => course.SubjectId, subject => subject.Id,
                    (course, subject) => new {course, subject})
                .Join(Data.Teachers, t => t.course.TeacherId, teacher => teacher.Id,
                    (t, teacher) => new {t, teacher})
                .Where(t => selectedClass.ClassId == t.t.course.ClassId)
                .Select((t,i) => new
                {
                    Id = i+1,
                    Subject = t.t.subject.Name,
                    Teacher = t.teacher.Name,
                    CourseId = t.t.course.Id
                }).ToList();
            ConsoleTable.From(courses).Write();
            Console.Write("\tChon khoa hoc (theo stt) : ");
            var selectedCourseIndex = Convert.ToInt32(Console.ReadLine());
            var selectedCourse = courses.Single(c => c.Id == selectedCourseIndex);
            Console.WriteLine($"Bang diem cua khoa hoc {selectedCourse.Subject} :");
            ConsoleTable.From(Data.Scores
                .Join(Data.Students, score => score.StudentId, student => student.Id,
                    (score, student) => new {score, student})
                .Where(t => selectedCourse.CourseId == t.score.CourseId)
                .Select((t,i) => new
                {
                    Id = i+1,
                    StudentId = t.student.Id,
                    t.student.Name,
                    t.score.StudentScore
                })).Write();
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }

        private void SearchByStudentName()
        {
            Console.Clear();
            Console.WriteLine("Student Manager Search Data");
            Console.WriteLine("-----------------------------------------------------");
            Console.Write("\tNhap ten hoc sinh : ");
            var studentName = Convert.ToString(Console.ReadLine());
            var searchResultOnStudentName = Data.Students
                .Join(Data.Classes, student => student.ClassId, c => c.Id, (student, c) => new {student, c})
                .Where(t => t.student.Name.ToLower().Contains(studentName))
                .Select((t, i) => new
                {
                    Id = i + 1,
                    StudentId = t.student.Id,
                    t.student.Name,
                    ClassName = t.c.Name,
                    t.student.Address,
                    t.student.BirthDate
                }).ToList();
            ConsoleTable.From(searchResultOnStudentName).Write();
            if (searchResultOnStudentName.Count == 0)
            {
                Console.WriteLine("Khong tim thay hoc sinh nao theo yeu cau!");
                Console.WriteLine("Press any key to continue ...");
                Console.ReadKey();
                return;
            }
            Console.Write("\tChon 1 hoc sinh (stt) : ");
            var id = Convert.ToInt32(Console.ReadLine());
            var selectedStudent = searchResultOnStudentName.Single(s => s.Id == id);
            Console.WriteLine($"\tBang diem cua {selectedStudent.Name} : ");
            ConsoleTable.From(Data.Scores
                .Join(Data.Courses, score => score.CourseId, course => course.Id,
                    (score, course) => new {score, course})
                .Join(Data.Subjects, t => t.course.SubjectId, subject => subject.Id,
                    (t, subject) => new {t, subject})
                .Where(t => t.t.score.StudentId == selectedStudent.StudentId)
                .Select((t,i) => new
                {
                    Id = i+1,
                    t.subject.Name,
                    t.t.score.StudentScore
                })).Write();
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();

        }
    }
}