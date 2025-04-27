using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace StorageApi.Domain
{
    public class Item
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public byte[] Picture { get; set; }

        public string PartNumber { get; set; }
        
        public string Category { get; set; }

        public string Place { get; set; }

        public string Description { get; set; }

        public string Supplier  { get; set; }

        public ushort Quantity { get; set; }

        public Item(string name, byte[] picture, string partNumber, string category, string place, string description, string supplier, ushort quantity)
        {
            Name = name;
            Picture = picture;
            PartNumber = partNumber;
            Category = category;
            Place = place;
            Description = description;
            Supplier = supplier;
            Quantity = quantity;
        }
    }
}
