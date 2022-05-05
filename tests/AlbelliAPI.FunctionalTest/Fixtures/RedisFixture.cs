using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules.Databases;
using System.Threading.Tasks;
using Xunit;

namespace AlbelliAPI.FunctionalTest.Fixtures;

public class RedisFixture : IAsyncLifetime
{
    private readonly RedisTestcontainer _redisTestContainer;

    public RedisFixture()
    {
        var redisContainerBuilder = new TestcontainersBuilder<RedisTestcontainer>()
            .WithImage("redis:6.2.3")
            .WithName("redis-api")
            .WithCleanUp(true)
            .WithPortBinding(6379, 6379);

        _redisTestContainer = redisContainerBuilder.Build();
    }

    public async Task InitializeAsync()
    {
        await _redisTestContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _redisTestContainer.StopAsync();
        await _redisTestContainer.DisposeAsync();
    }
}