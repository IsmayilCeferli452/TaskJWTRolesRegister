using Service.DTOs.Admin.Authors;

namespace Service.DTOs.Admin.Books
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Authors { get; set; }
    }
}
