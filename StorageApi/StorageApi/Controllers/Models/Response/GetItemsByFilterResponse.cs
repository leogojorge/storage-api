using StorageApi.Domain;

namespace StorageApi.Controllers.Models.Response
{
    public class GetItemsByFilterResponse
    {
        public List<Item> Items { get; set; }
    }
}
