using MongoDB.Driver;

namespace Albelli.Core.Models.MongoEntities;

public class AlbelliContext
{
    private readonly IMongoDatabase _database = null;
    private readonly IMongoClient _client;

    public AlbelliContext(IMongoClient client)
    {
        _client = client;
        _database = _client.GetDatabase("AlbelliDb");
    }

    public IMongoCollection<OrderDetailEntity> OrderDetails => _database.GetCollection<OrderDetailEntity>("OrderDetail");

    public IMongoCollection<ProductTypeEntity> ProductTypes => _database.GetCollection<ProductTypeEntity>("ProductType");

    public IMongoCollection<OrderEntity> Orders => _database.GetCollection<OrderEntity>("Order");

}