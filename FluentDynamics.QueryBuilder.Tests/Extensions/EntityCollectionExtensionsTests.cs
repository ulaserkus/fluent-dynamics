using FluentDynamics.QueryBuilder.Extensions;
using Microsoft.Xrm.Sdk;
using Xunit;

namespace FluentDynamics.QueryBuilder.Tests.Extensions
{
    public class EntityCollectionExtensionsTests
    {

        [Fact]
        public void ToList_Null_ReturnsEmpty()
        {
            EntityCollection ec = null;
            var list = ec.ToList();
            Assert.Empty(list);
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
