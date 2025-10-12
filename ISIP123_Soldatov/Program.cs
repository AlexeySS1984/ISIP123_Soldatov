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
}