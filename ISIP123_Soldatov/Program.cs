using System;
using System.Collections.Generic;

namespace UniversityManagementSystem
{
    public abstract class Person
    {
        private string name;
        private int age;
        private string contactInfo;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public string ContactInfo
        {
            get { return contactInfo; }
            set { contactInfo = value; }
        }

        protected Person(string name, int age, string contactInfo)
        {
            Name = name;
            Age = age;
            ContactInfo = contactInfo;
        }

        public abstract void DisplayInfo();
    }

    public class Student : Person
    {
        private List<Course> enrolledCourses = new List<Course>();

        public Student(string name, int age, string contactInfo) : base(name, age, contactInfo) { }

        public void EnrollInCourse(Course course)
        {
            if (!enrolledCourses.Contains(course))
            {
                enrolledCourses.Add(course);
                course.AddStudent(this);
            }
        }

        public void DisplayEnrolledCourses()
        {
            Console.WriteLine($"Курсы, на которые записан {Name}:");
            foreach (var course in enrolledCourses)
            {
                Console.WriteLine($"- {course.Name}");
            }
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Студент: {Name}, Возраст: {Age}, Контакт: {ContactInfo}");
            DisplayEnrolledCourses();
        }
    }
    public class Teacher : Person
    {
        private List<Course> taughtCourses = new List<Course>();

        public Teacher(string name, int age, string contactInfo) : base(name, age, contactInfo) { }

        public void AssignToCourse(Course course)
        {
            if (!taughtCourses.Contains(course))
            {
                taughtCourses.Add(course);
                course.AssignTeacher(this);
            }
        }

        public void DisplayTaughtCourses()
        {
            Console.WriteLine($"Курсы, которые ведет {Name}:");
            foreach (var course in taughtCourses)
            {
                Console.WriteLine($"- {course.Name}");
            }
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Преподаватель: {Name}, Возраст: {Age}, Контакт: {ContactInfo}");
            DisplayTaughtCourses();
        }
    }
    public class Course
    {
        private string name;
        private Teacher teacher;
        private List<Student> students = new List<Student>();

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Teacher Teacher => teacher;

        public Course(string name)
        {
            Name = name;
        }

        public void AssignTeacher(Teacher teacher)
        {
            this.teacher = teacher;
        }

        public void AddStudent(Student student)
        {
            if (!students.Contains(student))
            {
                students.Add(student);
            }
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Курс: {Name}");
            if (teacher != null)
            {
                Console.WriteLine($"Преподаватель: {teacher.Name}");
            }
            else
            {
                Console.WriteLine("Преподаватель не назначен.");
            }
            Console.WriteLine("Записанные студенты:");
            foreach (var student in students)
            {
                Console.WriteLine($"- {student.Name}");
            }
        }
    }
    public class University
    {
        // Инкапсуляция: приватные списки
        private List<Student> students = new List<Student>();
        private List<Teacher> teachers = new List<Teacher>();
        private List<Course> courses = new List<Course>();

        public void AddStudent(Student student)
        {
            students.Add(student);
        }

        public void AddTeacher(Teacher teacher)
        {
            teachers.Add(teacher);
        }

        public void AddCourse(Course course)
        {
            courses.Add(course);
        }

        public void DisplayAllStudents()
        {
            Console.WriteLine("Все студенты:");
            foreach (var student in students)
            {
                student.DisplayInfo();
                Console.WriteLine();
            }
        }

        public void DisplayAllTeachers()
        {
            Console.WriteLine("Все преподаватели:");
            foreach (var teacher in teachers)
            {
                teacher.DisplayInfo();
                Console.WriteLine();
            }
        }

        public void DisplayAllCourses()
        {
            Console.WriteLine("Все курсы:");
            foreach (var course in courses)
            {
                course.DisplayInfo();
                Console.WriteLine();
            }
        }

        public Student FindStudent(string name)
        {
            return students.Find(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public Teacher FindTeacher(string name)
        {
            return teachers.Find(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public Course FindCourse(string name)
        {
            return courses.Find(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            University university = new University();
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("Система управления университетом");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Просмотреть информацию о студенте");
                Console.WriteLine("3. Записать студента на курс");
                Console.WriteLine("4. Просмотреть курсы студента");
                Console.WriteLine("5. Добавить преподавателя");
                Console.WriteLine("6. Просмотреть информацию о преподавателе");
                Console.WriteLine("7. Назначить преподавателя на курс");
                Console.WriteLine("8. Добавить курс");
                Console.WriteLine("9. Просмотреть информацию о курсе");
                Console.WriteLine("10. Просмотреть студентов на курсе");
                Console.WriteLine("11. Просмотреть всех студентов");
                Console.WriteLine("12. Просмотреть всех преподавателей");
                Console.WriteLine("13. Просмотреть все курсы");
                Console.WriteLine("14. Выход");
                Console.Write("Выберите опцию: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStudent(university);
                        break;
                    case "2":
                        ViewStudentInfo(university);
                        break;
                    case "3":
                        EnrollStudentInCourse(university);
                        break;
                    case "4":
                        ViewStudentCourses(university);
                        break;
                    case "5":
                        AddTeacher(university);
                        break;
                    case "6":
                        ViewTeacherInfo(university);
                        break;
                    case "7":
                        AssignTeacherToCourse(university);
                        break;
                    case "8":
                        AddCourse(university);
                        break;
                    case "9":
                        ViewCourseInfo(university);
                        break;
                    case "10":
                        ViewStudentsInCourse(university);
                        break;
                    case "11":
                        university.DisplayAllStudents();
                        break;
                    case "12":
                        university.DisplayAllTeachers();
                        break;
                    case "13":
                        university.DisplayAllCourses();
                        break;
                    case "14":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Неверная опция. Попробуйте снова.");
                        break;
                }

                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private static void AddStudent(University university)
        {
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите возраст: ");
            int age = int.Parse(Console.ReadLine());
            Console.Write("Введите контактную информацию: ");
            string contact = Console.ReadLine();
            Student student = new Student(name, age, contact);
            university.AddStudent(student);
            Console.WriteLine("Студент добавлен.");
        }

        private static void ViewStudentInfo(University university)
        {
            Console.Write("Введите имя студента: ");
            string name = Console.ReadLine();
            Student student = university.FindStudent(name);
            if (student != null)
            {
                student.DisplayInfo();
            }
            else
            {
                Console.WriteLine("Студент не найден.");
            }
        }

        private static void EnrollStudentInCourse(University university)
        {
            Console.Write("Введите имя студента: ");
            string studentName = Console.ReadLine();
            Student student = university.FindStudent(studentName);
            if (student == null)
            {
                Console.WriteLine("Студент не найден.");
                return;
            }

            Console.Write("Введите название курса: ");
            string courseName = Console.ReadLine();
            Course course = university.FindCourse(courseName);
            if (course == null)
            {
                Console.WriteLine("Курс не найден.");
                return;
            }

            student.EnrollInCourse(course);
            Console.WriteLine("Студент записан на курс.");
        }

        private static void ViewStudentCourses(University university)
        {
            Console.Write("Введите имя студента: ");
            string name = Console.ReadLine();
            Student student = university.FindStudent(name);
            if (student != null)
            {
                student.DisplayEnrolledCourses();
            }
            else
            {
                Console.WriteLine("Студент не найден.");
            }
        }

        private static void AddTeacher(University university)
        {
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите возраст: ");
            int age = int.Parse(Console.ReadLine());
            Console.Write("Введите контактную информацию: ");
            string contact = Console.ReadLine();
            Teacher teacher = new Teacher(name, age, contact);
            university.AddTeacher(teacher);
            Console.WriteLine("Преподаватель добавлен.");
        }

        private static void ViewTeacherInfo(University university)
        {
            Console.Write("Введите имя преподавателя: ");
            string name = Console.ReadLine();
            Teacher teacher = university.FindTeacher(name);
            if (teacher != null)
            {
                teacher.DisplayInfo();
            }
            else
            {
                Console.WriteLine("Преподаватель не найден.");
            }
        }

        private static void AssignTeacherToCourse(University university)
        {
            Console.Write("Введите имя преподавателя: ");
            string teacherName = Console.ReadLine();
            Teacher teacher = university.FindTeacher(teacherName);
            if (teacher == null)
            {
                Console.WriteLine("Преподаватель не найден.");
                return;
            }

            Console.Write("Введите название курса: ");
            string courseName = Console.ReadLine();
            Course course = university.FindCourse(courseName);
            if (course == null)
            {
                Console.WriteLine("Курс не найден.");
                return;
            }

            teacher.AssignToCourse(course);
            Console.WriteLine("Преподаватель назначен на курс.");
        }

        private static void AddCourse(University university)
        {
            Console.Write("Введите название курса: ");
            string name = Console.ReadLine();
            Course course = new Course(name);
            university.AddCourse(course);
            Console.WriteLine("Курс добавлен.");
        }

        private static void ViewCourseInfo(University university)
        {
            Console.Write("Введите название курса: ");
            string name = Console.ReadLine();
            Course course = university.FindCourse(name);
            if (course != null)
            {
                course.DisplayInfo();
            }
            else
            {
                Console.WriteLine("Курс не найден.");
            }
        }

        private static void ViewStudentsInCourse(University university)
        {
            Console.Write("Введите название курса: ");
            string name = Console.ReadLine();
            Course course = university.FindCourse(name);
            if (course != null)
            {
                course.DisplayInfo(); 
            }
            else
            {
                Console.WriteLine("Курс не найден.");
            }
        }
    }
}