namespace LibraryApp.API.Models
{
    public class LibraryTable
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string AuthorsName { get; set; }
        public bool? IsBorrowed { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? BorrowerUser { get; set; }
    }
}
