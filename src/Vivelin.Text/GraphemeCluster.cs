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

        public IReadOnlyList<object> CodePoints { get; }

        private static IReadOnlyList<object> GetCodePoints(string value)
        {
            var codePoints = new List<object>();
            foreach (var rune in value.EnumerateRunes())
            {
                codePoints.Add(rune);
            }
            return codePoints.AsReadOnly();
        }
    }
}
