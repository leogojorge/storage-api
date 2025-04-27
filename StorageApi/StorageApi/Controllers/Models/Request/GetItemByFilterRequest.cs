namespace StorageApi.Controllers.Models.Request
{
    public class GetItemByFilterRequest : PaginatedRequest
    {
        public string NameAndDescription { get; set; }
        
        public string PartNumber { get; set; }

        public string Category { get; set; }

        public string Place { get; set; }

        public string Supplier { get; set; }
    }
}
