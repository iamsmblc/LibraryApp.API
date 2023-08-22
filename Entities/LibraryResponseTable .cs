namespace LibraryApp.API.Models
{
    public class LibraryResponseTable
    {
        public int Id { get; set; }
        public string? BookName { get; set; }
        public string? AuthorsName { get; set; }
        public string? BorrowedMessage { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool? IsBorrowed { get; set; }
        public string? BorrowerUser { get; set; }
        public int? OrderedId { get; set; }
    }
}