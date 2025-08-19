using FluentDynamics.QueryBuilder;
using Microsoft.Xrm.Sdk;
using Xunit;

namespace FluentDynamics.QueryBuilder.Tests
{
    public class DynamicsExtensionsTests
    {
        [Fact]
        public void ToList_Null_ReturnsEmpty()
        {
            EntityCollection ec = null;
            var list = ec.ToList();
            Assert.Empty(list);
        }

        [Fact]
        public void TryGet_ReturnsValueOrDefault()
        {
            var e = new Entity("account")
            {
                ["name"] = "Test Account",
                ["revenue"] = 123m
            };

            var name = e.TryGet<string>("name");
            var revenue = e.TryGet<decimal>("revenue");
            var missing = e.TryGet<int>("missing", 42);
            var wrongType = e.TryGet<int>("name", -1);

            Assert.Equal("Test Account", name);
            Assert.Equal(123m, revenue);
            Assert.Equal(42, missing);
            Assert.Equal(-1, wrongType);
        }

        [Fact]
        public void LinqLikeExtensions_Work()
        {
            var ec = new EntityCollection();
            ec.Entities.Add(new Entity("contact") { ["firstname"] = "Alice" });
            ec.Entities.Add(new Entity("contact") { ["firstname"] = "Bob" });

            var first = ec.FirstOrDefault(e => (string)e["firstname"] == "Alice");
            var singleOrNull = ec.SingleOrDefault(e => (string)e["firstname"] == "Alice"); // Tek eşleşme
            var whereResult = ec.Where(e => ((string)e["firstname"]).StartsWith("B")).ToList();

            Assert.NotNull(first);
            Assert.NotNull(singleOrNull);
            Assert.Single(whereResult);
        }
    }
}