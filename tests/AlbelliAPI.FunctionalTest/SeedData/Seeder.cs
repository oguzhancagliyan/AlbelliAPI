using Albelli.Core.Models.MongoEntities;
using Funda.FunctionalTests.SeedData;
using System;
using System.Linq;

namespace AlbelliAPI.FunctionalTest.SeedData;

public static class Seeder
{
    public static void Seed(AlbelliContext context)
    {
        var installers = typeof(OrderSeeder).Assembly.ExportedTypes
            .Where(m => typeof(ISeeder).IsAssignableFrom(m) && !m.IsInterface && !m.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<ISeeder>()
            .ToList();

        installers.ForEach(m => m.Seed(context));
    }
}