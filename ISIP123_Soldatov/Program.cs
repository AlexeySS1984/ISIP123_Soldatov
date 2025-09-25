using System;
using System.Collections.Generic;

public class Stats
{
    public int WordCount { get; set; }
    public string Shortest { get; set; }
    public int Sentences { get; set; }
    public int Vowels { get; set; }
    public int Consonants { get; set; }
    public string Longest { get; set; }
    public Dictionary<char, int> Frequency { get; set; }
}

class Program
{
    static void Main()
    {
        List<Stats> history = new List<Stats>();
        bool running = true;
        while (running)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1 - Ввести новый текст");
            Console.WriteLine("2 - Вывести статистику по прошлым текстам");
            Console.WriteLine("3 - Выход");
            string input = Console.ReadLine();
            if (input == "3")
            {
                running = false;
                continue;
            }
            if (input == "2")
            {
                if (history.Count == 0)
                {
                    Console.WriteLine("Нет сохраненной статистики.");
                    continue;
                }
                for (int i = 0; i < history.Count; i++)
                {
                    Console.WriteLine($"Статистика {i + 1}:");
                    PrintStats(history[i]);
                }
                continue;
            }
            // Ввод нового текста
            string text = "";
            while (text.Length < 100)
            {
                Console.WriteLine("Введите текст (минимум 100 символов):");
                text = Console.ReadLine();
                if (text.Length < 100)
                {
                    Console.WriteLine("Текст слишком короткий. Попробуйте снова.");
                }
            }
            Stats originalStats = ComputeStats(text);
            Console.WriteLine("Статистика для исходного текста:");
            PrintStats(originalStats);
            history.Add(originalStats);
            Console.WriteLine("Хотите удалить некоторые буквы? (y/n):");
            string delChoice = Console.ReadLine().ToLower();
            if (delChoice == "y")
            {
                Console.WriteLine("Введите буквы для удаления (например, аеи):");
                string delInput = Console.ReadLine();
                HashSet<char> delSet = new HashSet<char>();
                foreach (char ch in delInput.ToLower())
                {
                    if (char.IsLetter(ch))
                    {
                        delSet.Add(ch);
                    }
                }
                // Удаление указанных букв из текста (регистронезависимо)
                string modifiedText = "";
                foreach (char c in text)
                {
                    char lc = char.ToLower(c);
                    if (char.IsLetter(c) && delSet.Contains(lc))
                    {
                        continue; // Пропускаем (удаляем) эту букву
                    }
                    modifiedText += c;
                }
                Console.WriteLine("Модифицированный текст:");
                Console.WriteLine(modifiedText);
                Stats modStats = ComputeStats(modifiedText);
                Console.WriteLine("Статистика для модифицированного текста:");
                PrintStats(modStats);
                history.Add(modStats);
            }
            Console.WriteLine("Обработка завершена.");
        }
    }

    static Stats ComputeStats(string text)
    {
        // Множество русских гласных букв (в нижнем регистре)
        HashSet<char> rusVowels = new HashSet<char> { 'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я' };

        // Подсчет количества предложений (по знакам . ! ?)
        int sentences = 0;
        foreach (char c in text)
        {
            if (c == '.' || c == '!' || c == '?')
            {
                sentences++;
            }
        }

        // Извлечение всех слов (последовательностей букв, игнорируя пунктуацию)
        List<string> allWords = new List<string>();
        string currentWord = "";
        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                currentWord += c;
            }
            else
            {
                if (currentWord.Length > 0)
                {
                    allWords.Add(currentWord);
                    currentWord = "";
                }
            }
        }
        if (currentWord.Length > 0)
        {
            allWords.Add(currentWord);
        }

        // Множества союзов и чисел (в нижнем регистре)
        HashSet<string> unions = new HashSet<string> { "и", "но", "а", "да", "или", "же", "ли", "то" };
        HashSet<string> numbers = new HashSet<string> { "один", "два", "две", "три", "четыре", "четверых", "пять", "шесть", "семь", "восемь", "девять", "десять", "первый", "первое", "xix" };

        // Фильтрация валидных слов (исключая союзы и числа)
        List<string> validWords = new List<string>();
        foreach (string word in allWords)
        {
            string lowerWord = word.ToLower();
            // Проверка, является ли слово числом из цифр
            bool isDigitNumber = true;
            for (int k = 0; k < lowerWord.Length; k++)
            {
                if (!(lowerWord[k] >= '0' && lowerWord[k] <= '9'))
                {
                    isDigitNumber = false;
                    break;
                }
            }
            if (isDigitNumber)
            {
                continue;
            }
            if (unions.Contains(lowerWord))
            {
                continue;
            }
            if (numbers.Contains(lowerWord))
            {
                continue;
            }
            validWords.Add(word);
        }

        int wordCount = validWords.Count;

        // Поиск самого короткого и самого длинного слова среди валидных
        string shortestWord = "";
        int minLength = int.MaxValue;
        string longestWord = "";
        int maxLength = -1;
        foreach (string w in validWords)
        {
            int len = w.Length;
            if (len < minLength)
            {
                minLength = len;
                shortestWord = w;
            }
            if (len > maxLength)
            {
                maxLength = len;
                longestWord = w;
            }
        }
        if (wordCount == 0)
        {
            shortestWord = "нет слов";
            longestWord = "нет слов";
        }

        // Подсчет частоты букв и гласных/согласных (только буквы, регистронезависимо)
        Dictionary<char, int> frequency = new Dictionary<char, int>();
        int vowelsCount = 0;
        int consonantsCount = 0;
        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                char lowerC = char.ToLower(c);
                if (!frequency.ContainsKey(lowerC))
                {
                    frequency[lowerC] = 0;
                }
                frequency[lowerC]++;
                if (rusVowels.Contains(lowerC))
                {
                    vowelsCount++;
                }
                else
                {
                    consonantsCount++;
                }
            }
        }

        return new Stats
        {
            WordCount = wordCount,
            Shortest = shortestWord,
            Sentences = sentences,
            Vowels = vowelsCount,
            Consonants = consonantsCount,
            Longest = longestWord,
            Frequency = frequency
        };
    }

    static void PrintStats(Stats s)
    {
        Console.WriteLine($"Количество слов в тексте (не считая союзы и числа): {s.WordCount}");
        Console.WriteLine($"Самое короткое слово: {s.Shortest}");
        Console.WriteLine($"Количество предложений: {s.Sentences}");
        Console.WriteLine($"Количество гласных букв: {s.Vowels}");
        Console.WriteLine($"Количество согласных букв: {s.Consonants}");
        Console.WriteLine($"Самое длинное слово: {s.Longest}");
        Console.WriteLine("Статистика по частоте встречаемости каждой буквы:");
        // Сложный фрагмент: сортировка частот букв по алфавиту без использования LINQ
        // Создаем список пар ключ-значение для возможности сортировки
        List<KeyValuePair<char, int>> freqList = new List<KeyValuePair<char, int>>();
        foreach (KeyValuePair<char, int> kv in s.Frequency)
        {
            freqList.Add(kv);
        }
        // Пузырьковая сортировка по ключу (букве) - простой алгоритм сортировки для небольшого количества элементов
        int n = freqList.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (freqList[j].Key > freqList[j + 1].Key)
                {
                    // Обмен элементов
                    KeyValuePair<char, int> temp = freqList[j];
                    freqList[j] = freqList[j + 1];
                    freqList[j + 1] = temp;
                }
            }
        }
        // Вывод отсортированных частот
        foreach (KeyValuePair<char, int> kv in freqList)
        {
            Console.WriteLine($"{kv.Key}: {kv.Value}");
        }
        Console.WriteLine();
    }
}