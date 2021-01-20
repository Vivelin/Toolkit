using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivelin.Text
{
    public readonly struct CodePoint
    {
        private readonly Rune _rune;

        public CodePoint(int value)
            : this(new Rune(value))
        {
        }

        public CodePoint(in Rune rune)
        {
            _rune = rune;

            Value = rune.Value;
            Category = Rune.GetUnicodeCategory(rune);

            var bytes = new byte[rune.Utf8SequenceLength];
            rune.EncodeToUtf8(bytes);
            Bytes = bytes;
        }

        public int Value { get; }

        public ReadOnlyMemory<byte> Bytes { get; }

        public UnicodeCategory Category { get; }

        public override string ToString()
        {
            return $"U+{Value:X}";
        }
    }
}
