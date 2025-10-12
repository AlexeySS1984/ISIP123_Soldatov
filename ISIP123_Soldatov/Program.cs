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
    // Класс Teacher, наследующий от Person
    public class Teacher : Person
    {
        // Инкапсуляция: приватный список курсов, которые ведет преподаватель
        private List<Course> taughtCourses = new List<Course>();

        public Teacher(string name, int age, string contactInfo) : base(name, age, contactInfo) { }

        // Метод для назначения на курс
        public void AssignToCourse(Course course)
        {
            if (!taughtCourses.Contains(course))
            {
                taughtCourses.Add(course);
                course.AssignTeacher(this);
            }
        }

        // Метод для просмотра курсов преподавателя
        public void DisplayTaughtCourses()
        {
            Console.WriteLine($"Курсы, которые ведет {Name}:");
            foreach (var course in taughtCourses)
            {
                Console.WriteLine($"- {course.Name}");
            }
        }

        // Полиморфизм: переопределение метода DisplayInfo
        public override void DisplayInfo()
        {
            Console.WriteLine($"Преподаватель: {Name}, Возраст: {Age}, Контакт: {ContactInfo}");
            DisplayTaughtCourses();
        }
    }
}