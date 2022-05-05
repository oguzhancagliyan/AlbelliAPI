using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Albelli.Core.Models.MongoEntities
{
    public class OrderDetailEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string OrderId { get; set; }

        public int Quantity { get; set; }

        public string ProductTypeId { get; set; }
    }
}
