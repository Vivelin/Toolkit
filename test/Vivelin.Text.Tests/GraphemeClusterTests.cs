using System;
using System.Linq;
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
        [InlineData("A")]
        [InlineData("☃")]
        [InlineData("🐱‍👤")]
        public void GraphemeClusterContainsStringRepresentation(string value)
        {
            var grapheme = new GraphemeCluster(value);
            grapheme.Representation.Should().Be(value);
        }

        [Theory]
        [InlineData("A", 1)]
        [InlineData("☃", 1)]
        [InlineData("👩🏻", 2)]
        [InlineData("🐱‍👤", 3)]
        public void GraphemeClusterContainsNumberOfCodePoints(string value, 
            int codePoints)
        {
            var grapheme = new GraphemeCluster(value);
            grapheme.CodePoints.Count.Should().Be(codePoints);
        }

        [Theory]
        [InlineData("A", 0x41)]
        [InlineData("☃", 0x2603)]
        [InlineData("👩🏻", 0x1F469, 0x1F3FB)]
        [InlineData("🐱‍👤", 0x1F431, 0x200D, 0x1F464)]
        public void GraphemeClusterContainsCodePoints(string value,
            params int[] codePoints)
        {
            var grapheme = new GraphemeCluster(value);
            grapheme.CodePoints.Select(x => x.Value)
                .Should().BeEquivalentTo(codePoints);
        }
    }
}