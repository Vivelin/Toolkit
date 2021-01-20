using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivelin.Text
{
    /// <summary>
    /// Represents a sequence of code points
    /// </summary>
    public readonly struct GraphemeCluster
    {
        public GraphemeCluster(string value)
        {
            var si = new StringInfo(value);
            if (si.LengthInTextElements == 0)
                throw new ArgumentException("The specified string contains no grapheme clusters.");
            if (si.LengthInTextElements > 1)
                throw new ArgumentException("The specified string contains more than one grapheme cluster.");

            Representation = value;
            CodePoints = GetCodePoints(value);
        }

        public string Representation { get; }

        public IReadOnlyList<CodePoint> CodePoints { get; }

        private static IReadOnlyList<CodePoint> GetCodePoints(string value)
        {
            var codePoints = new List<CodePoint>();
            foreach (var rune in value.EnumerateRunes())
            {
                var codePoint = new CodePoint(rune);
                codePoints.Add(codePoint);
            }
            return codePoints.AsReadOnly();
        }
    }
}
