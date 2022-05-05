using Albelli.Core.Models.MongoEntities;
using Funda.FunctionalTests.SeedData;
using MongoDB.Bson;

namespace AlbelliAPI.FunctionalTest.SeedData;

public class OrderSeeder : ISeeder
{
    public static string OrderPk = ObjectId.GenerateNewId().ToString();
    public void Seed(AlbelliContext context)
    {
        var order = new OrderEntity
        {
            Id = OrderPk,
            BinWidth = 10
        };

        context.Orders.InsertOne(order);
    }
}