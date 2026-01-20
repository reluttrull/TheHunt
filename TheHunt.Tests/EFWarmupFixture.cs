using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TheHunt.Common.Data;

namespace TheHunt.Tests
{
    public class EfWarmupFixture
    {
        public EfWarmupFixture()
        {
            var options = new DbContextOptionsBuilder<GameContext>()
                .UseInMemoryDatabase("Warmup")
                .Options;

            using var context = new GameContext(options);
            context.Database.EnsureCreated();
        }
    }

    [CollectionDefinition("EF")]
    public class EfCollection : ICollectionFixture<EfWarmupFixture> { }
}
