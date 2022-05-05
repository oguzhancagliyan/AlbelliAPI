using AlbelliAPI.FunctionalTest.Fixtures;
using Xunit;

namespace AlbelliAPI.FunctionalTest.CollectionDefinitions;

[CollectionDefinition(nameof(FunctionalTestCollection))]
public class FunctionalTestCollection : ICollectionFixture<TestServerFixture>, ICollectionFixture<MongoDbFixture>, ICollectionFixture<RedisFixture>
{
}
