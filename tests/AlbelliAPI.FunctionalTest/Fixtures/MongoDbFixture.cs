using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules;
using DotNet.Testcontainers.Containers.Modules.Abstractions;
using Xunit;

namespace AlbelliAPI.FunctionalTest.Fixtures;

public class MongoDbFixture : IAsyncLifetime
{
    private readonly TestcontainersContainer _mongoDbContainer;
    private readonly bool _skipContainer;

    public MongoDbFixture()
    {
        var mongoBuilder = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage("mongo:4.4.1")
            .WithCleanUp(true)
            .WithName("mongo-test")
            .WithEnvironment("MONGO_INITDB_ROOT_USERNAME", "albelli")
            .WithEnvironment("MONGO_INITDB_ROOT_PASSWORD", "albelli")
            .WithEnvironment("MONGO_INITDB_DATABASE", "db")
            //.WithEnvironment("DOCKER_HOST", "unix:///var/run/docker.sock")
            .WithEnvironment("DEBUG", "1")
            .WithPortBinding(27017, 27017)
            .WithExposedPort(27017);

        _mongoDbContainer = mongoBuilder.Build();
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _mongoDbContainer.StopAsync();
        await _mongoDbContainer.DisposeAsync();
    }
}