using FluentAssertions;
using Infraestructura;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Base.Test
{
    public abstract class BaseRepositoryTestCase<TEntity>
    {
        protected DbContextOptions<CablemodemContext> Options { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            this.Options = new DbContextOptionsBuilder<CablemodemContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        public void AreEquals(object objectA, object objectB, params string[] ignoreList)
        {
            TestComprarer.Equal(objectA, objectB, ignoreList).Should().BeTrue();
        }

        public abstract TEntity CreateEntity();
    }
}
