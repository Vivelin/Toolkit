using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Vivelin.Text.Tests
{
    public class CodePointTests
    {
        [Theory]
        [InlineData(0x41, "A")]
        [InlineData(0x200D, "\u200D")]
        public void CodePointHasStringRepresentation(int value, string representation)
        {
            var codePoint = new CodePoint(value);
            codePoint.ToString().Should().Be(representation);
        }

        [Theory]
        [InlineData(0x41, UnicodeCategory.UppercaseLetter)]
        [InlineData(0x200D, UnicodeCategory.Format)]
        public void CodePointHasUnicodeCategory(int value, UnicodeCategory category)
        {
            var codePoint = new CodePoint(value);
            codePoint.Category.Should().Be(category);
        }

        [Theory]
        [InlineData(0x41, new byte[] { 0x41 })]
        [InlineData(0x2603, new byte[] { 0xE2, 0x98, 0x83 })]
        public void CodePointHasUtf8Encoding(int value, byte[] expected)
        {
            var codePoint = new CodePoint(value);
            codePoint.Bytes.ToArray().Should().BeEquivalentTo(expected);
        }
    }
}
