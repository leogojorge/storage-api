namespace StorageApi.Controllers.Models
{
    public class PaginatedResponse
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int ItemCount { get; set; }
    }
}
