using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreInventory
{
    // Перечисление для категорий товаров
    public enum Category
    {
        Food,
        Electronics,
        Clothes,
        Books,
        Toys  // Минимум 3, но добавил 5 для разнообразия
    }

    // Класс для товара
    public class Product
    {
        public string Code { get; private set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool InStock => Quantity > 0;
        public Category Category { get; set; }

        public Product(string code, string name, decimal price, int quantity, Category category)
        {
            Code = code;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }

        public override string ToString()
        {
            return $"Код: {Code}\nНазвание: {Name}\nЦена: {Price:C}\nКоличество: {Quantity}\nВ наличии: {(InStock ? "Да" : "Нет")}\nКатегория: {Category}\n";
        }
    }

    class Program
    {
        private static List<Product> products = new List<Product>();
        private static int nextId = 1; // Для автоматической генерации кода, начиная с "1"

        static void Main(string[] args)
        {
            // Заполнение тестовыми данными
            AddTestData();

            // Главный цикл программы
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Система учета товаров в магазине");
                Console.WriteLine("1. Добавить товар");
                Console.WriteLine("2. Удалить товар");
                Console.WriteLine("3. Заказать поставку товара");
                Console.WriteLine("4. Продать товар");
                Console.WriteLine("5. Поиск товаров");
                Console.WriteLine("6. Выход");
                Console.Write("Выберите команду: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1: AddProduct(); break;
                        case 2: RemoveProduct(); break;
                        case 3: OrderSupply(); break;
                        case 4: SellProduct(); break;
                        case 5: SearchProducts(); break;
                        case 6: return;
                        default: Console.WriteLine("Неверный выбор. Нажмите Enter для продолжения."); Console.ReadLine(); break;
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Нажмите Enter для продолжения.");
                    Console.ReadLine();
                }
            }
        }

        private static void AddTestData()
        {
            products.Add(new Product(GenerateCode(), "Яблоки", 50.0m, 100, Category.Food));
            products.Add(new Product(GenerateCode(), "Смартфон", 15000.0m, 20, Category.Electronics));
            products.Add(new Product(GenerateCode(), "Футболка", 800.0m, 50, Category.Clothes));
            products.Add(new Product(GenerateCode(), "Книга по C#", 1200.0m, 30, Category.Books));
            products.Add(new Product(GenerateCode(), "Конструктор Lego", 2500.0m, 15, Category.Toys));
        }

        private static string GenerateCode()
        {
            return "1" + nextId++;
        }

        private static void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("Добавление товара");

            string name = GetValidString("Название: ");
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Название не может быть пустым. Операция отменена.");
                Console.ReadLine();
                return;
            }

            decimal price = GetValidDecimal("Цена: ");
            if (price <= 0)
            {
                Console.WriteLine("Цена должна быть положительной. Операция отменена.");
                Console.ReadLine();
                return;
            }

            int quantity = GetValidInt("Количество: ");
            if (quantity < 0)
            {
                Console.WriteLine("Количество не может быть отрицательным. Операция отменена.");
                Console.ReadLine();
                return;
            }

            Category category = GetValidCategory();

            string code = GenerateCode();
            products.Add(new Product(code, name, price, quantity, category));
            Console.WriteLine("Товар добавлен успешно.");
            Console.ReadLine();
        }

        private static void RemoveProduct()
        {
            Console.Clear();
            Console.WriteLine("Удаление товара");
            string code = GetValidString("Введите код товара: ");
            Product product = products.FirstOrDefault(p => p.Code == code);
            if (product != null)
            {
                products.Remove(product);
                Console.WriteLine("Товар удален успешно.");
            }
            else
            {
                Console.WriteLine("Товар не найден.");
            }
            Console.ReadLine();
        }

        private static void OrderSupply()
        {
            Console.Clear();
            Console.WriteLine("Заказ поставки");
            string code = GetValidString("Введите код товара: ");
            Product product = products.FirstOrDefault(p => p.Code == code);
            if (product != null)
            {
                int amount = GetValidInt("Введите количество для поставки: ");
                if (amount > 0)
                {
                    product.Quantity += amount;
                    Console.WriteLine("Поставка заказана успешно.");
                }
                else
                {
                    Console.WriteLine("Количество должно быть положительным.");
                }
            }
            else
            {
                Console.WriteLine("Товар не найден.");
            }
            Console.ReadLine();
        }

        private static void SellProduct()
        {
            Console.Clear();
            Console.WriteLine("Продажа товара");
            string code = GetValidString("Введите код товара: ");
            Product product = products.FirstOrDefault(p => p.Code == code);
            if (product != null)
            {
                if (!product.InStock)
                {
                    Console.WriteLine("Товар отсутствует на складе.");
                    Console.ReadLine();
                    return;
                }

                int amount = GetValidInt("Введите количество для продажи: ");
                if (amount > 0 && amount <= product.Quantity)
                {
                    product.Quantity -= amount;
                    Console.WriteLine("Товар продан успешно.");
                }
                else if (amount > product.Quantity)
                {
                    Console.WriteLine("Недостаточно товара на складе.");
                }
                else
                {
                    Console.WriteLine("Количество должно быть положительным.");
                }
            }
            else
            {
                Console.WriteLine("Товар не найден.");
            }
            Console.ReadLine();
        }

        private static void SearchProducts()
        {
            Console.Clear();
            Console.WriteLine("Поиск товаров");
            Console.WriteLine("1. По коду");
            Console.WriteLine("2. По названию");
            Console.WriteLine("3. По категории");
            Console.Write("Выберите тип поиска: ");

            if (int.TryParse(Console.ReadLine(), out int searchType))
            {
                IEnumerable<Product> results = null;

                switch (searchType)
                {
                    case 1:
                        string code = GetValidString("Введите код: ");
                        results = products.Where(p => p.Code == code);
                        break;
                    case 2:
                        string name = GetValidString("Введите название (или часть): ");
                        results = products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
                        break;
                    case 3:
                        Category category = GetValidCategory();
                        results = products.Where(p => p.Category == category);
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        Console.ReadLine();
                        return;
                }

                if (results.Any())
                {
                    Console.WriteLine("Результаты поиска:");
                    foreach (var product in results)
                    {
                        Console.WriteLine(product);
                        Console.WriteLine("-------------------");
                    }
                }
                else
                {
                    Console.WriteLine("Товары не найдены.");
                }
            }
            else
            {
                Console.WriteLine("Неверный ввод.");
            }
            Console.ReadLine();
        }

        // Вспомогательные методы для валидации ввода
        private static string GetValidString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        private static decimal GetValidDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal value))
                {
                    return value;
                }
                Console.WriteLine("Неверный формат. Попробуйте снова.");
            }
        }

        private static int GetValidInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    return value;
                }
                Console.WriteLine("Неверный формат. Попробуйте снова.");
            }
        }

        private static Category GetValidCategory()
        {
            Console.WriteLine("Выберите категорию:");
            var categories = Enum.GetValues(typeof(Category));
            for (int i = 0; i < categories.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {categories.GetValue(i)}");
            }

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int catIndex) && catIndex > 0 && catIndex <= categories.Length)
                {
                    return (Category)categories.GetValue(catIndex - 1);
                }
                Console.WriteLine("Неверный выбор. Попробуйте снова.");
            }
        }
    }
}