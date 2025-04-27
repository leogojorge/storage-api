namespace StorageApi.Controllers.Models
{
    public class PaginatedResponse
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public long ItemCount { get; set; }
    }
}
