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
}