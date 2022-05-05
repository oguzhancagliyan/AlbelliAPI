using Albelli.Core.Models.MongoEntities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albelli.Core.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly AlbelliContext _context;

        public ProductTypeService(AlbelliContext context)
        {
            _context = context;
        }

        public async Task<List<ProductTypeEntity>> PopulateProductTypesAsync()
        {
            List<ProductTypeEntity> productTypeEntities = new List<ProductTypeEntity>
            {
                new ProductTypeEntity
                {
                    Name = "photoBook",
                    PackageWidth = 19,

                },
                new ProductTypeEntity
                {
                    Name = "calendar",
                    PackageWidth = 10,

                },
                new ProductTypeEntity
                {
                    Name = "canvas",
                    PackageWidth = 16,
                },
                new ProductTypeEntity
                {
                    Name = "cards",
                    PackageWidth = 4.7,

                },
                 new ProductTypeEntity
                {
                    Name = "mug",
                    PackageWidth = 94,

                },
            };

            await _context.ProductTypes.DeleteManyAsync(Builders<ProductTypeEntity>.Filter.Empty);

            await _context.ProductTypes.InsertManyAsync(productTypeEntities);

            return productTypeEntities;
        }
    }
}
