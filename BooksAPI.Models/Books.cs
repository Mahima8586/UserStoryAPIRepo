namespace BooksAPI.Models
{
    public class Books
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public DateTime PublishedDate { get; set; }
        public int Pages { get; set; }
        public string Description { get; set; }
    }
}
