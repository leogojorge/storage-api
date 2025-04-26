namespace StorageApi.Controllers.Models
{
    public class GetItemByFilterRequest : PaginatedRequest
    {
        public string NameAndDescription { get; set; }
    }
}
