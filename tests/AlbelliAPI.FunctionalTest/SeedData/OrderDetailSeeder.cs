using Albelli.Core.Models.MongoEntities;
using Funda.FunctionalTests.SeedData;
using MongoDB.Bson;

namespace AlbelliAPI.FunctionalTest.SeedData;

public class OrderDetailSeeder : ISeeder
{
    public static string OrderDetailPk = ObjectId.GenerateNewId().ToString();

    public void Seed(AlbelliContext context)
    {
        OrderDetailEntity entity = new OrderDetailEntity
        {
            Id = OrderDetailPk,
            OrderId = OrderSeeder.OrderPk,
            ProductTypeId = ProductTypeSeeder.calendarPk
        };

        context.OrderDetails.InsertOne(entity);
    }
}