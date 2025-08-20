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
        [Fact]
        public void ToArray_ReturnsExpectedEntities()
        {
            var ec = new EntityCollection();
            ec.Entities.Add(new Entity("account") { Id = Guid.NewGuid() });
            ec.Entities.Add(new Entity("account") { Id = Guid.NewGuid() });

            var array = ec.ToArray();

            Assert.Equal(2, array.Length);
            Assert.Equal(ec.Entities[0].Id, array[0].Id);
            Assert.Equal(ec.Entities[1].Id, array[1].Id);
        }

        [Fact]
        public void ToArray_NullCollection_ReturnsEmptyArray()
        {
            EntityCollection ec = null;
            var array = ec.ToArray();

            Assert.NotNull(array);
            Assert.Empty(array);
        }

        [Fact]
        public void SingleOrDefault_MultipleMatches_ThrowsException()
        {
            var ec = new EntityCollection();
            ec.Entities.Add(new Entity("contact") { ["firstname"] = "John" });
            ec.Entities.Add(new Entity("contact") { ["firstname"] = "John" });

            Assert.Throws<InvalidOperationException>(() =>
                ec.SingleOrDefault(e => (string)e["firstname"] == "John"));
        }

        [Fact]
        public void Select_WithNullSelector_ThrowsArgumentNullException()
        {
            var ec = new EntityCollection();
            ec.Entities.Add(new Entity("account"));

            Assert.Throws<ArgumentNullException>(() => ec.Select<string>(null).ToList());
        }

        [Fact]
        public void ToTypedList_ConvertsToExpectedType()
        {
            var ec = new EntityCollection();
            var entity1 = new Entity("account") { Id = Guid.NewGuid() };
            var entity2 = new Entity("account") { Id = Guid.NewGuid() };
            ec.Entities.Add(entity1);
            ec.Entities.Add(entity2);

            var typedList = ec.ToTypedList<Entity>();

            Assert.Equal(2, typedList.Count);
            Assert.Equal(entity1.Id, typedList[0].Id);
            Assert.Equal(entity2.Id, typedList[1].Id);
        }
    }
}
