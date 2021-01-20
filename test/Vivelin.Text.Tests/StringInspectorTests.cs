using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Vivelin.Text.Tests
{
    public class StringInspectorTests
    {
        [Theory]
        [InlineData("abc", "a", "b", "c")]
        [InlineData("á", "á")]
        [InlineData("a\u0301", "á")]
        [InlineData("‏abc", "\u200F", "a", "b", "c")]
        [InlineData("👻👩‍👩‍👦‍👦", "👻", "👩‍👩‍👦‍👦")]
        public void StringInspectorCanReadGraphemesFromString(
            string value, params string[] expectedGraphemes)
        {
            var graphemes = value.GetGraphemes();

            graphemes.Select(x => x.Representation)
                .Should().BeEquivalentTo(expectedGraphemes);
        }
    }
}
