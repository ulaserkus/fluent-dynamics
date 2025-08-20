using FluentDynamics.QueryBuilder.Extensions;
using Microsoft.Xrm.Sdk;
using Xunit;

namespace FluentDynamics.QueryBuilder.Tests.Extensions
{
    public class EntityExtensionsTests
    {
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
            var missing = e.TryGet("missing", 42);
            var wrongType = e.TryGet("name", -1);

            Assert.Equal("Test Account", name);
            Assert.Equal(123m, revenue);
            Assert.Equal(42, missing);
            Assert.Equal(-1, wrongType);
        }
    }
}
