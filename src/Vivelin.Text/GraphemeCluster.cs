using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Vivelin.Text
{
    /// <summary>
    /// Represents a sequence of code points encoding a single glyph.
    /// </summary>
    public readonly struct GraphemeCluster
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphemeCluster"/>
        /// struct with the specified value.
        /// </summary>
        /// <param name="value">The grapheme to represent.</param>
        internal GraphemeCluster(string value)
        {
            Representation = value;
            CodePoints = GetCodePoints(value);
        }

        /// <summary>
        /// Gets a string representation of the grapheme cluster.
        /// </summary>
        public string Representation { get; }

        /// <summary>
        /// Gets the code points that make up the grapheme.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public IReadOnlyList<CodePoint> CodePoints { get; }

        /// <summary>
        /// Creates a new <see cref="GraphemeCluster"/> from the specified
        /// string.
        /// </summary>
        /// <param name="value">The string containing a single grapheme.</param>
        /// <returns>A new <see cref="GraphemeCluster"/> struct.</returns>
        /// <exception cref="ArgumentException">
        /// The string either contains no graphemes or too many.
        /// </exception>
        public static GraphemeCluster Create(string value)
        {
            var si = new StringInfo(value);
            if (si.LengthInTextElements == 0)
                throw new ArgumentException("The specified string contains no grapheme clusters.");
            if (si.LengthInTextElements > 1)
                throw new ArgumentException("The specified string contains more than one grapheme cluster.");

            return new GraphemeCluster(value);
        }

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

        /// <summary>
        /// Returns a string representation of the grapheme cluster.
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString()
        {
            return Representation;
        }
    }
}