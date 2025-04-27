namespace StorageApi.Controllers.Models.Request
{
    public class GetItemByFilterRequest : PaginatedRequest
    {
        public string NameAndDescription { get; set; }
    }
}
