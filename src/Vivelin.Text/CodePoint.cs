using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Unicode;

namespace Vivelin.Text
{
    /// <summary>
    /// Represents a single UTF-8 code point.
    /// </summary>
    [DebuggerDisplay("{Name,nq} {ToString()} ({UnicodeValue,nq})")]
    public readonly struct CodePoint
    {
        private readonly Rune _rune;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodePoint"/> struct
        /// with the specified value.
        /// </summary>
        /// <param name="value">The numerical value of the code point.</param>
        public CodePoint(int value)
            : this(new Rune(value))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodePoint"/> struct
        /// with the specified rune.
        /// </summary>
        /// <param name="rune">The rune that represents the code point.</param>
        public CodePoint(in Rune rune)
        {
            _rune = rune;

            Value = rune.Value;
            UnicodeValue = $"U+{rune.Value:X4}";
            Name = UnicodeInfo.GetName(rune.Value);
            Category = Rune.GetUnicodeCategory(rune);
            Bytes = GetUtf8Encoding(rune);
        }

        /// <summary>
        /// Gets the numerical value of the code point.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Gets the numerical value of the code point as a typical string.
        /// </summary>
        public string UnicodeValue { get; }

        /// <summary>
        /// Gets the name of the code point.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the UTF-8 byte encoding of the code point.
        /// </summary>
        public ReadOnlyMemory<byte> Bytes { get; }

        /// <summary>
        /// Gets the Unicode category the code point is part of.
        /// </summary>
        public UnicodeCategory Category { get; }

        private static byte[] GetUtf8Encoding(Rune rune)
        {
            var bytes = new byte[rune.Utf8SequenceLength];
            rune.EncodeToUtf8(bytes);
            return bytes;
        }

        /// <summary>
        /// Returns a string representation of the code point.
        /// </summary>
        /// <returns>A string that represents the code point.</returns>
        public override string ToString()
        {
            return _rune.ToString();
        }
    }
}