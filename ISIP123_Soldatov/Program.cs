using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreInventory
{
    public enum Category
    {
        Food,
        Electronics,
        Clothing
    }

    public class Product
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool InStock => Quantity > 0;
        public Category Category { get; set; }

        public override string ToString()
        {
            return $"Код: {Code}, Название: {Name}, Цена: {Price}, Количество: {Quantity}, В наличии: {(InStock ? "Да" : "Нет")}, Категория: {Category}";
        }
    }

    public class Sale
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }

    class Program
    {
        private static List<Product> products = new List<Product>();
        private static Stack<Sale> saleHistory = new Stack<Sale>();
        private static List<Sale> allSales = new List<Sale>();
        private static int nextCode = 1001;

        static void Main(string[] args)
        {
            InitializeTestData();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Меню:");
                Console.WriteLine("1. Добавить товар");
                Console.WriteLine("2. Удалить товар");
                Console.WriteLine("3. Заказать поставку товара");
                Console.WriteLine("4. Продать товар");
                Console.WriteLine("5. Поиск товаров");
                Console.WriteLine("6. Отменить последнюю продажу");
                Console.WriteLine("7. Отчёт о продажах");
                Console.WriteLine("8. Выход");
                Console.Write("Выберите опцию: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Неверный ввод. Нажмите Enter для продолжения.");
                    Console.ReadLine();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        AddProduct();
                        break;
                    case 2:
                        RemoveProduct();
                        break;
                    case 3:
                        OrderSupply();
                        break;
                    case 4:
                        SellProduct();
                        break;
                    case 5:
                        SearchProducts();
                        break;
                    case 6:
                        UndoLastSale();
                        break;
                    case 7:
                        SalesReport();
                        break;
                    case 8:
                        return;
                    default:
                        Console.WriteLine("Неверная опция. Нажмите Enter для продолжения.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void InitializeTestData()
        {
            products.Add(new Product { Code = (nextCode++).ToString(), Name = "Яблоко", Price = 50m, Quantity = 100, Category = Category.Food });
            products.Add(new Product { Code = (nextCode++).ToString(), Name = "Смартфон", Price = 20000m, Quantity = 10, Category = Category.Electronics });
            products.Add(new Product { Code = (nextCode++).ToString(), Name = "Футболка", Price = 1000m, Quantity = 50, Category = Category.Clothing });
            products.Add(new Product { Code = (nextCode++).ToString(), Name = "Хлеб", Price = 30m, Quantity = 200, Category = Category.Food });
            products.Add(new Product { Code = (nextCode++).ToString(), Name = "Наушники", Price = 5000m, Quantity = 20, Category = Category.Electronics });
        }

        private static void AddProduct()
        {
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Название не может быть пустым.");
                Console.ReadLine();
                return;
            }

            Console.Write("Введите цену: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
            {
                Console.WriteLine("Цена должна быть положительным числом.");
                Console.ReadLine();
                return;
            }

            Console.Write("Введите количество: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
            {
                Console.WriteLine("Количество не может быть отрицательным.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Выберите категорию:");
            foreach (var cat in Enum.GetValues(typeof(Category)))
            {
                Console.WriteLine($"{(int)cat + 1}. {cat}");
            }
            if (!int.TryParse(Console.ReadLine(), out int catChoice) || !Enum.IsDefined(typeof(Category), catChoice - 1))
            {
                Console.WriteLine("Неверная категория.");
                Console.ReadLine();
                return;
            }
            Category category = (Category)(catChoice - 1);

            products.Add(new Product { Code = (nextCode++).ToString(), Name = name, Price = price, Quantity = quantity, Category = category });
            Console.WriteLine("Товар добавлен.");
            Console.ReadLine();
        }

        private static void RemoveProduct()
        {
            Console.Write("Введите код товара для удаления: ");
            string code = Console.ReadLine().Trim();
            var product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар не найден.");
            }
            else
            {
                products.Remove(product);
                Console.WriteLine("Товар удалён.");
            }
            Console.ReadLine();
        }

        private static void OrderSupply()
        {
            Console.Write("Введите код товара: ");
            string code = Console.ReadLine().Trim();
            var product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар не найден.");
                Console.ReadLine();
                return;
            }

            Console.Write("Введите количество для поставки: ");
            if (!int.TryParse(Console.ReadLine(), out int addQuantity) || addQuantity <= 0)
            {
                Console.WriteLine("Количество должно быть положительным.");
                Console.ReadLine();
                return;
            }

            product.Quantity += addQuantity;
            Console.WriteLine("Поставка заказана. Новое количество: " + product.Quantity);
            Console.ReadLine();
        }

        private static void SellProduct()
        {
            Console.Write("Введите код товара: ");
            string code = Console.ReadLine().Trim();
            var product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар не найден.");
                Console.ReadLine();
                return;
            }

            if (!product.InStock)
            {
                Console.WriteLine("Товар отсутствует на складе.");
                Console.ReadLine();
                return;
            }

            Console.Write("Введите количество для продажи: ");
            if (!int.TryParse(Console.ReadLine(), out int sellQuantity) || sellQuantity <= 0 || sellQuantity > product.Quantity)
            {
                Console.WriteLine("Неверное количество. Доступно: " + product.Quantity);
                Console.ReadLine();
                return;
            }

            product.Quantity -= sellQuantity;
            decimal total = sellQuantity * product.Price;
            Sale sale = new Sale { Product = product, Quantity = sellQuantity, Total = total };
            allSales.Add(sale);
            saleHistory.Push(sale);
            Console.WriteLine($"Продано {sellQuantity} шт. на сумму {total}. Остаток: {product.Quantity}");
            Console.ReadLine();
        }

        private static void SearchProducts()
        {
            Console.WriteLine("Поиск по:");
            Console.WriteLine("1. Коду");
            Console.WriteLine("2. Названию");
            Console.WriteLine("3. Категории");
            if (!int.TryParse(Console.ReadLine(), out int searchType))
            {
                Console.WriteLine("Неверный ввод.");
                Console.ReadLine();
                return;
            }

            IEnumerable<Product> results = null;

            switch (searchType)
            {
                case 1:
                    Console.Write("Введите код: ");
                    string code = Console.ReadLine().Trim();
                    results = products.Where(p => p.Code == code);
                    break;
                case 2:
                    Console.Write("Введите название (или часть): ");
                    string name = Console.ReadLine().Trim().ToLower();
                    results = products.Where(p => p.Name.ToLower().Contains(name));
                    break;
                case 3:
                    Console.WriteLine("Выберите категорию:");
                    foreach (var cat in Enum.GetValues(typeof(Category)))
                    {
                        Console.WriteLine($"{(int)cat + 1}. {cat}");
                    }
                    if (!int.TryParse(Console.ReadLine(), out int catChoice) || !Enum.IsDefined(typeof(Category), catChoice - 1))
                    {
                        Console.WriteLine("Неверная категория.");
                        Console.ReadLine();
                        return;
                    }
                    Category category = (Category)(catChoice - 1);
                    results = products.Where(p => p.Category == category);
                    break;
                default:
                    Console.WriteLine("Неверная опция.");
                    Console.ReadLine();
                    return;
            }

            if (results.Any())
            {
                foreach (var p in results)
                {
                    Console.WriteLine(p.ToString());
                }
            }
            else
            {
                Console.WriteLine("Товары не найдены.");
            }
            Console.ReadLine();
        }

        private static void UndoLastSale()
        {
            if (saleHistory.Count == 0)
            {
                Console.WriteLine("Нет продаж для отмены.");
                Console.ReadLine();
                return;
            }

            Sale lastSale = saleHistory.Pop();
            lastSale.Product.Quantity += lastSale.Quantity;
            allSales.RemoveAt(allSales.Count - 1); // Assuming added in order
            Console.WriteLine($"Отменена продажа: {lastSale.Quantity} шт. товара {lastSale.Product.Name}. Новое количество: {lastSale.Product.Quantity}");
            Console.ReadLine();
        }

        private static void SalesReport()
        {
            if (allSales.Count == 0)
            {
                Console.WriteLine("Нет продаж.");
                Console.ReadLine();
                return;
            }

            var groupedSales = allSales.GroupBy(s => s.Product);

            Console.WriteLine("Отчёт о продажах:");
            decimal grandTotal = 0;
            foreach (var group in groupedSales)
            {
                int totalQuantity = group.Sum(s => s.Quantity);
                decimal totalSum = group.Sum(s => s.Total);
                Console.WriteLine($"{group.Key.Name} (Код: {group.Key.Code}): {totalQuantity} шт., сумма: {totalSum}");
                grandTotal += totalSum;
            }
            Console.WriteLine($"Общая сумма продаж: {grandTotal}");
            Console.ReadLine();
        }
    }
}