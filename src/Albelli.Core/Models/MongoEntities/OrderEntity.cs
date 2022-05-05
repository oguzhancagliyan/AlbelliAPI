using Albelli.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Albelli.Core.Models.MongoEntities;

public class OrderEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public double BinWidth { get; set; }
}