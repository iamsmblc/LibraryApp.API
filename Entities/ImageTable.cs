namespace LibraryApp.API.Models
{
    public class ImageTable
    {
        public int ImageId { get; set; }
        public int? LibraryTableId { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
       
        
    }
}
