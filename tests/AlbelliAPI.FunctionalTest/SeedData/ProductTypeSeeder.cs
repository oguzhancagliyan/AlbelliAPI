using Albelli.Core.Models.MongoEntities;
using Funda.FunctionalTests.SeedData;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbelliAPI.FunctionalTest.SeedData
{
    public class ProductTypeSeeder : ISeeder
    {
        public static string photoBookPk = ObjectId.GenerateNewId().ToString();
        public static string calendarPk = ObjectId.GenerateNewId().ToString();
        public static string canvasPk = ObjectId.GenerateNewId().ToString();
        public static string cardsPk = ObjectId.GenerateNewId().ToString();
        public static string mugPk = ObjectId.GenerateNewId().ToString();

        public static List<ProductTypeEntity> productTypeEntities = new List<ProductTypeEntity>
            {
                new ProductTypeEntity
                {
                    Id = photoBookPk,
                    Name = "photoBook",
                    PackageWidth = 19,

                },
                new ProductTypeEntity
                {
                    Id = calendarPk,
                    Name = "calendar",
                    PackageWidth = 10,

                },
                new ProductTypeEntity
                {
                    Id = canvasPk,
                    Name = "canvas",
                    PackageWidth = 16,
                },
                new ProductTypeEntity
                {
                    Id = cardsPk,
                    Name = "cards",
                    PackageWidth = 4.7,

                },
                 new ProductTypeEntity
                {
                    Id = mugPk,
                    Name = "mug",
                    PackageWidth = 94,

                },
            };
        public void Seed(AlbelliContext context)
        {
            context.ProductTypes.InsertMany(productTypeEntities);
        }
    }
}
