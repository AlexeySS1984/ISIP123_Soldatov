Console.WriteLine("Введите количество операций (от 2 до 40):");
int n;
while (!int.TryParse(Console.ReadLine(), out n) || n < 2 || n > 40)
{
    Console.WriteLine("Ошибка! Введите число от 2 до 40:");
}

string[] names = new string[n];
int[] prices = new int[n];

// Ввод данных
for (int i = 0; i < n; i++)
{
    Console.WriteLine($"Введите данные для операции {i + 1} (формат: Название;Сумма):");
    string input = Console.ReadLine();
    string[] parts = input.Split(';');
    while (parts.Length != 2 || !int.TryParse(parts[1].Trim(), out int price))
    {
        Console.WriteLine("Ошибка! Формат: Название;Сумма. Повторите ввод:");
        input = Console.ReadLine();
        parts = input.Split(';');
    }
    names[i] = parts[0].Trim();
    prices[i] = int.Parse(parts[1].Trim());
}

while (true)
{
    Console.WriteLine("\nМеню:");
    Console.WriteLine("1. Вывод данных");
    Console.WriteLine("2. Статистика (среднее, максимальное, минимальное, сумма)");
    Console.WriteLine("3. Сортировка по цене (пузырьковая сортировка)");
    Console.WriteLine("4. Конвертация валюты");
    Console.WriteLine("5. Поиск по названию");
    Console.WriteLine("0. Выход");
    Console.Write("Выберите пункт меню: ");

    string choice = Console.ReadLine();

    if (choice == "0") break;

    switch (choice)
    {
        case "1": // Вывод данных
            Console.WriteLine("\nСписок трат:");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"{names[i]}: {prices[i]} руб.");
            }
            break;

        case "2": // Статистика
            if (n > 0)
            {
                int sum = 0, max = prices[0], min = prices[0];
                foreach (int price in prices)
                {
                    sum += price;
                    if (price > max) max = price;
                    if (price < min) min = price;
                }
                double avg = (double)sum / n;
                Console.WriteLine($"\nСтатистика:");
                Console.WriteLine($"Сумма: {sum} руб.");
                Console.WriteLine($"Среднее: {avg:F2} руб.");
                Console.WriteLine($"Максимальное: {max} руб.");
                Console.WriteLine($"Минимальное: {min} руб.");
            }
            break;

        case "3": // Пузырьковая сортировка
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (prices[j] > prices[j + 1])
                    {
                        // Swap prices
                        int tempPrice = prices[j];
                        prices[j] = prices[j + 1];
                        prices[j + 1] = tempPrice;
                        // Swap names
                        string tempName = names[j];
                        names[j] = names[j + 1];
                        names[j + 1] = tempName;
                    }
                }
            }
            Console.WriteLine("\nДанные отсортированы по цене.");
            break;

        case "4": // Конвертация валюты
            Console.WriteLine("\nВыберите валюту или введите курс:");
            Console.WriteLine("1. USD (курс 90)");
            Console.WriteLine("2. EUR (курс 100)");
            Console.WriteLine("3. Ввести свой курс");
            string currencyChoice = Console.ReadLine();
            double rate = 1;

            if (currencyChoice == "1") rate = 90;
            else if (currencyChoice == "2") rate = 100;
            else if (currencyChoice == "3")
            {
                Console.Write("Введите курс валюты (1 валюта = X рублей): ");
                while (!double.TryParse(Console.ReadLine(), out rate) || rate <= 0)
                {
                    Console.Write("Ошибка! Введите положительное число: ");
                }
            }
            else
            {
                Console.WriteLine("Неверный выбор, используется курс 1:1.");
            }

            Console.WriteLine("\nТраты в выбранной валюте:");
            for (int i = 0; i < n; i++)
            {
                double converted = prices[i] / rate;
                Console.WriteLine($"{names[i]}: {converted:F2} валюты");
            }
            break;

        case "5": // Поиск по названию
            Console.Write("Введите название для поиска: ");
            string search = Console.ReadLine().ToLower();
            bool found = false;
            Console.WriteLine("\nРезультаты поиска:");
            for (int i = 0; i < n; i++)
            {
                if (names[i].ToLower().Contains(search))
                {
                    Console.WriteLine($"{names[i]}: {prices[i]} руб.");
                    found = true;
                }
            }
            if (!found) Console.WriteLine("Ничего не найдено.");
            break;

        default:
            Console.WriteLine("Неверный выбор. Попробуйте снова.");
            break;
    }
}
