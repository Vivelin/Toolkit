using System;
using FluentAssertions;
using Xunit;

namespace Vivelin.Toolkit.Tests
{
    public class FileSizeTests
    {
        [Fact]
        public void EqualFileSizesAreEqual()
        {
            var a = new FileSize(656518130L);
            var b = new FileSize(656518130L);
            Assert.Equal(a, b);
        }

        [Fact]
        public void EqualFileSizesAreEqual_ObjectEquals()
        {
            var a = new FileSize(656518130L);
            var b = new FileSize(656518130L);
            object.Equals(a, b).Should().BeTrue();
        }

        [Fact]
        public void EqualFileSizesAreEqual_Operator()
        {
            var a = new FileSize(656518130L);
            var b = new FileSize(656518130L);
            Assert.True(a == b);
        }

        [Fact]
        public void DifferentFileSizesAreNotEqual()
        {
            var a = new FileSize(656518130L);
            var b = new FileSize(1041788473L);
            Assert.NotEqual(a, b);
        }

        [Fact]
        public void DifferentFileSizesAreNotEqual_ObjectEquals()
        {
            var a = new FileSize(656518130L);
            var b = new FileSize(1041788473L);
            object.Equals(a, b).Should().BeFalse();
        }

        [Fact]
        public void DifferentFileSizesAreNotEqual_Operator()
        {
            var a = new FileSize(656518130L);
            var b = new FileSize(1041788473L);
            Assert.True(a != b);
        }

        [Fact]
        public void FileSizesAreNotEqualToOtherObjects()
        {
            var a = new FileSize(656518130L);
            var b = "";
            Assert.False(a.Equals(b));
        }

        [Fact]
        public void EqualFileSizesHaveEqualHashCodes()
        {
            var a = new FileSize(656518130L);
            var b = new FileSize(656518130L);
            Assert.Equal(
                a.GetHashCode(),
                b.GetHashCode());
        }

        [Fact]
        public void DifferentFileSizesHaveDifferentHashCodes()
        {
            var a = new FileSize(656518130L);
            var b = new FileSize(1041788473L);
            Assert.NotEqual(
                a.GetHashCode(),
                b.GetHashCode());
        }

        [Fact]
        public void FileSizesCompareCorrectly()
        {
            var large = new FileSize(long.MaxValue);
            var large2 = new FileSize(long.MaxValue);
            var small = new FileSize(0L);
            var small2 = new FileSize(0L);
            Assert.True(large > small);
            Assert.True(large >= small);
            Assert.True(large >= large2);
            Assert.True(small < large);
            Assert.True(small <= large);
            Assert.True(small <= small2);
        }
    }
}
