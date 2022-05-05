using Albelli.Core.Models.MongoEntities;

namespace Funda.FunctionalTests.SeedData;

public interface ISeeder
{
    void Seed(AlbelliContext context);
}