using System;
using FluentAssertions;
using Xunit;

namespace Vivelin.Text.Tests
{
    public class GraphemeClusterTests
    {
        [Theory]
        [InlineData("")]
        public void GraphemeClusterCannotBeInitializedEmpty(string value)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new GraphemeCluster(value);
            });
        }

        [Theory]
        [InlineData("abc")]
        public void GraphemeClusterCannotBeInitializedWithMoreThanOneGrapheme(string value)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new GraphemeCluster(value);
            });
        }

        [Theory]
        [InlineData("a")]
        [InlineData("☃")]
        [InlineData("🐱‍👤")]
        public void GraphemeClusterContainsStringRepresentation(string value)
        {
            var grapheme = new GraphemeCluster(value);
            grapheme.Representation.Should().Be(value);
        }

        [Theory]
        [InlineData("a", 1)]
        [InlineData("☃", 1)]
        [InlineData("👩🏻", 2)]
        [InlineData("🐱‍👤", 3)]
        public void GraphemeClusterContainsNumberOfCodePoints(string value, 
            int codePoints)
        {
            var grapheme = new GraphemeCluster(value);
            grapheme.CodePoints.Count.Should().Be(codePoints);
        }
    }
}