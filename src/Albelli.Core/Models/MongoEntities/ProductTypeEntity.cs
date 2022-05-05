using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Albelli.Core.Models.MongoEntities
{
    public class ProductTypeEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public double PackageWidth { get; set; }       
    }
}
