using Albelli.Core.Models.MongoEntities;

namespace Albelli.Core.Services
{
    public interface IProductTypeService
    {
        Task<List<ProductTypeEntity>> PopulateProductTypesAsync();
    }
}
