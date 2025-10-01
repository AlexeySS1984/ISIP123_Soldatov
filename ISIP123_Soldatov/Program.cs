public enum Genre
{
    Fiction,
    NonFiction,
    Mystery,
    ScienceFiction,
    Biography
}

public class Book
{
    public int Id { get; private set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public Genre Genre { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }

    public Book(int id, string title, string author, Genre genre, int year, decimal price)
    {
        Id = id;
        Title = title;
        Author = author;
        Genre = genre;
        Year = year;
        Price = price;
    }

    public override string ToString()
    {
        return $"ID: {Id}, Название: {Title}, Автор: {Author}, Жанр: {Genre}, Год: {Year}, Цена: {Price:C}";
    }
}
class Program
{
    private static List<Book> books = new List<Book>();
    private static int nextId = 1;

    static void Main(string[] args)
    {
        AddTestBooks();

        while (true)
        {
            DisplayMenu();
            string choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    AddBook();
                    break;
                case "2":
                    //DeleteBook();
                    break;
                case "3":
                    //SearchBooks();
                    break;
                case "4":
                    //SortBooks();
                    break;
                case "5":
                    //DisplayMinMaxPriceBooks();
                    break;
                case "6":
                    //GroupByAuthor();
                    break;
                case "7":
                    Console.WriteLine("Выход из программы. До свидания!");
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                    break;
            }

            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private static void AddTestBooks()
    {
        books.Add(new Book(nextId++, "The Great Gatsby", "F. Scott Fitzgerald", Genre.Fiction, 1925, 10.99m));
        books.Add(new Book(nextId++, "1984", "George Orwell", Genre.ScienceFiction, 1949, 8.99m));
        books.Add(new Book(nextId++, "To Kill a Mockingbird", "Harper Lee", Genre.Fiction, 1960, 12.50m));
        books.Add(new Book(nextId++, "Sapiens", "Yuval Noah Harari", Genre.NonFiction, 2011, 15.00m));
        books.Add(new Book(nextId++, "The Da Vinci Code", "Dan Brown", Genre.Mystery, 2003, 9.99m));
    }

    private static void DisplayMenu()
    {
        Console.WriteLine("Система учета книг");
        Console.WriteLine("1. Добавить книгу");
        Console.WriteLine("2. Удалить книгу по ID");
        Console.WriteLine("3. Поиск книги (по названию, автору, или жанру)");
        Console.WriteLine("4. Отсортировать книги (по названиию или году)");
        Console.WriteLine("5. Отобразить самые дорогие и самые дешевые книги");
        Console.WriteLine("6. Сгруппировать книги по автору и количеству");
        Console.WriteLine("7. Выход");
        Console.Write("Введите ваш выбор: ");
    }
    private static void AddBook()
    {
        try
        {
            Console.Write("Введите название: ");
            string title = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Название не может быть пустым.");
                return;
            }

            Console.Write("Введите автора: ");
            string author = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(author))
            {
                Console.WriteLine("Автор не может быть пустым.");
                return;
            }

            Console.WriteLine("Доступные жанры: " + string.Join(", ", Enum.GetNames(typeof(Genre))));
            Console.Write("Введите жанр: ");
            string genreInput = Console.ReadLine()?.Trim();
            if (!Enum.TryParse<Genre>(genreInput, true, out Genre genre))
            {
                Console.WriteLine("Неверный жанр.");
                return;
            }

            Console.Write("Введите год: ");
            if (!int.TryParse(Console.ReadLine()?.Trim(), out int year) || year <= 0)
            {
                Console.WriteLine("Неверный год. Должен быть натуральным числом.");
                return;
            }

            Console.Write("Введите цену: ");
            if (!decimal.TryParse(Console.ReadLine()?.Trim(), out decimal price) || price < 0)
            {
                Console.WriteLine("Неверная цена. Должна быть неотрицательной.");
                return;
            }

            Book newBook = new Book(nextId++, title, author, genre, year, price);
            books.Add(newBook);
            Console.WriteLine("Книга добавлена успешно:");
            Console.WriteLine(newBook);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Возникла ошибка: {ex.Message}");
        }
    }
}