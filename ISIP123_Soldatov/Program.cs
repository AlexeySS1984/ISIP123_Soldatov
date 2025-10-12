using System;
using System.Collections.Generic;

namespace UniversityManagementSystem
{
    // Абстрактный класс Person для демонстрации абстракции и наследования
    public abstract class Person
    {
        // Инкапсуляция: приватные поля
        private string name;
        private int age;
        private string contactInfo;

        // Публичные свойства для доступа к приватным полям
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

        // Конструктор
        protected Person(string name, int age, string contactInfo)
        {
            Name = name;
            Age = age;
            ContactInfo = contactInfo;
        }

        // Абстрактный метод для полиморфизма: каждый подкласс реализует свой способ отображения информации
        public abstract void DisplayInfo();
    }
}