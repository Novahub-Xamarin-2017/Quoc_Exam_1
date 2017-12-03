using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables;
using StudentManager.Controllers;

namespace StudentManager.Views
{
    public class MenuReport :Menu
    {
        public MenuReport(DataProvider data) : base(data)
        {
        }

        public override void DrawMenu()
        {
            int choice;
            do
            {
                Console.Clear();
                Console.WriteLine("Student Manager Report");
                Console.WriteLine("-----------------------------------------------------");
                Console.WriteLine("1 |\tDanh sach hoc sinh gioi");
                Console.WriteLine("2 |\tDiem trung binh theo lop");
                Console.WriteLine("3 |\tTop 100 hoc sinh");
                Console.WriteLine("4 |\tTop 3 lop co so luong hoc sinh gioi nhieu nhat");
                Console.WriteLine("5 |\tTop 3 lop co diem trung binh cao nhat");
                Console.WriteLine("6 |\tTop 3 giao vien");
                Console.WriteLine("7 |\tExit");
                Console.Write("\nChon thao tac : ");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        ShowGoodStudents();
                        break;
                    case 2:
                        ShowClassAverageScore();
                        break;
                    case 3:
                        ShowTop100Students();
                        break;
                    case 4:
                        ShowTop3ClassWithHighestGoodStudentCount();
                        break;
                    case 5:
                        ShowTop3ClassWithHighestAverageScore();
                        break;
                    case 6:
                        ShowTop3Teachers();
                        break;

                }
            } while (choice != 3);
        }

        private void ShowTop3Teachers()
        {
            Console.Clear();
            Console.WriteLine("Student Manager Report");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("1 |\tGiao vien co khoa hoc nhieu nhat");
            Console.WriteLine("2 |\tGiao vien co nhieu hoc sinh nhat");
            Console.WriteLine("3 |\tGiao vien co diem trung binh cao nhat");
            Console.Write("\nChon thao tac : ");
            var choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    ShowTeacherWithHighestCourseCount();
                    break;
                case 2:
                    ShowTeacherWithHighestStudentCount();
                    break;
                case 3:
                    ShowTeacherWithHighestAverageScore();
                    break;
            }
        }

        private void ShowTeacherWithHighestAverageScore()
        {
            Console.WriteLine("3. Top 3 giao vien co diem trung binh cao nhat");
            ConsoleTable.From(Data.Teachers.Join(Data.Subjects, teacher => teacher.SubjectId, subject => subject.Id,
                (teacher, subject) => new
                {
                    teacher.Name,
                    Subject = subject.Name,
                    HighestAverageScore = GetHighestAverageScore(teacher.Id)
                }).OrderByDescending(o => o.HighestAverageScore).Take(3)).Write();
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }

        private double GetHighestAverageScore(int teacherId)
        {
            try
            {
                return Data.Courses.Where(c => c.TeacherId == teacherId).Average(c =>
                    Data.Scores.Where(s => s.CourseId == c.Id).Average(s => s.StudentScore));
            }
            catch (Exception e)
            {
                return 0.0;
                throw;
            }
            
        }

        private void ShowTeacherWithHighestStudentCount()
        {
            Console.WriteLine("2. Top 3 giao vien co so luong sinh vien hoc nhieu nhat");
            ConsoleTable.From(Data.Teachers.Join(Data.Classes, teacher => teacher.Id, c => c.TeacherId, (teacher, c) =>
                new
                {
                    teacher.Name,
                    StudentCount = Data.Students.Count(s => s.ClassId == c.Id)
                }).OrderByDescending(s => s.StudentCount).Take(3)).Write();
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }
        

        private void ShowTeacherWithHighestCourseCount()
        {
            Console.WriteLine("1. Top 3 giao vien co so luong khoa hoc nhieu nhat: ");
            ConsoleTable.From(Data.Teachers.GroupJoin(Data.Courses, teacher => teacher.Id, course => course.TeacherId,
                (teacher, g) => new
                {
                    teacher.Name,
                    count = g.Count()
                }).OrderByDescending(t => t.count).Take(3)
                ).Write();
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }

        private void ShowTop3ClassWithHighestAverageScore()
        {
            ConsoleTable.From(Data.Classes.Join(Data.Teachers, c => c.TeacherId, teacher => teacher.Id, (c, teacher) =>
                new
                {
                    c.Name,
                    Teacher = teacher.Name,
                    AverageScore = Data.Students.Join(Data.Scores, student => student.Id, score => score.StudentId,
                            (student, score) => new {student, score})
                        .Where(@t => @t.student.ClassId == c.Id)
                        .Select(@t => new
                        {
                            avgScore = GetAverageScore(@t.student.Id)
                        })
                        .Average(o => o.avgScore)
                })
                .OrderByDescending(c => c.AverageScore)
                .Take(3)
                ).Write();
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }

        private void ShowTop3ClassWithHighestGoodStudentCount()
        {
            var top3Classes = from c in Data.Classes
                join teacher in Data.Teachers on c.TeacherId equals teacher.Id
                select new
                {
                    c.Name,
                    Teacher = teacher.Name,
                    GoodStudentCount = Data.Students
                        .Join(Data.Scores, student => student.Id, score => score.StudentId,
                            (student, score) => new {student, score})
                        .Where(@t => @t.student.ClassId == c.Id)
                        .Select(@t => new
                        {
                            avgScore = GetAverageScore(@t.student.Id)
                        })
                        .Count()
                };
            ConsoleTable.From(top3Classes.OrderByDescending(c => c.GoodStudentCount).Take(3)).Write();
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }

        private void ShowTop100Students()
        {
            Console.Clear();
            Console.WriteLine("Student Manager Report");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("\nTop 100 sinh vien");
            ConsoleTable.From(Data.Students
                    .Join(Data.Classes, student => student.ClassId, c => c.Id,(student, c) => new {student, c}) 
                    .Select((s,i) => new
                    {
                        Id = i+1,
                        s.student.Name,
                        Class = s.c.Name,
                        AvegareScore = GetAverageScore(s.student.Id)
                    })
                    .OrderByDescending(s => s.AvegareScore)
                    .ThenBy(s => s.Class)
                    .ThenBy(s=> s.Name)
                ).Write();
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }

        private void ShowClassAverageScore()
        {
            Console.Clear();
            Console.WriteLine("Student Manager Report");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("\nDanh sach lop hoc");
            var classes = (from c in Data.Classes
                join teacher in Data.Teachers on c.TeacherId equals teacher.Id
                select new
                {
                    c.Id,
                    c.Name,
                    Teacher = teacher.Name
                }).ToList();
            ConsoleTable.From(classes).Write();
            Console.Write("\tChon lop : ");
            var selectedClassId = Convert.ToInt32(Console.ReadLine());
            var selectedClass = classes.Single(c => c.Id == selectedClassId);
            Console.WriteLine($"Bang diem trung binh cua lop {selectedClass.Name}");
            ConsoleTable.From(
                from student in Data.Students
                where selectedClass.Id == student.ClassId
                select new
                {
                    StudentId = student.Id,
                    student.Name,
                    AverageScore = GetAverageScore(student.Id)
                }
            ).Write();
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();

        }

        private void ShowGoodStudents()
        {
            Console.Clear();
            Console.WriteLine("Student Manager Report");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("\nDanh sach hoc sinh gioi");
            ConsoleTable.From(GetGoodStudents()).Write();
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }

        private IEnumerable<object> GetGoodStudents()
        {
            return Data.Students.GroupJoin(Data.Scores, student => student.Id, score => score.StudentId,
                    (student, g) => new
                    {
                        StudentId = student.Id,
                        student.Name,
                        AverageScore = GetAverageScore(student.Id)
                    })
                .OrderByDescending(o => o.AverageScore)
                .TakeWhile(o => o.AverageScore >= 8);
        }

        private double GetAverageScore(int studentId)
        {
            var scores = (from score in Data.Scores
                join course in Data.Courses on score.CourseId equals course.Id
                join subject in Data.Subjects on course.SubjectId equals subject.Id
                where score.StudentId == studentId
                select new
                {
                    score.StudentScore,
                    subject.CoefficientNumber
                }).ToList();
            if (scores.Count == 0)
            {
                return 0.0;
            }
            return scores.Sum(s => s.CoefficientNumber * s.StudentScore) / scores.Sum(s => s.CoefficientNumber);
        }
    }
}