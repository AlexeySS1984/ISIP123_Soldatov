namespace LibraryManagement
{
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
    }
}