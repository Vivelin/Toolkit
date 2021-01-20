using System;
using System.Collections.Generic;
using System.Globalization;

namespace Vivelin.Text
{
    /// <summary>
    /// Provides a set of static methods for inspecting a string.
    /// </summary>
    public static class StringInspector
    {
        /// <summary>
        /// Returns a collection of the grapheme clusters in the string.
        /// </summary>
        /// <param name="value">The string whose graphemes to enumerate.</param>
        /// <returns>
        /// A new <see cref="IEnumerable{T}"/> of <see cref="GraphemeCluster"/>
        /// for each grapheme in <paramref name="value"/>.
        /// </returns>
        public static IEnumerable<GraphemeCluster> EnumerateGraphemes(
            this string value)
        {
            var enumerator = StringInfo.GetTextElementEnumerator(value);
            while (enumerator.MoveNext())
            {
                var textElement = enumerator.GetTextElement();
                yield return new GraphemeCluster(textElement);
            }
        }

        /// <summary>
        /// Returns a collection of the grapheme clusters in the string.
        /// </summary>
        /// <param name="value">The string whose graphemes to enumerate.</param>
        /// <returns>
        /// A new <see cref="IReadOnlyList{T}"/> of <see
        /// cref="GraphemeCluster"/> for each grapheme in <paramref
        /// name="value"/>.
        /// </returns>
        public static IReadOnlyList<GraphemeCluster> GetGraphemes(
            this string value)
        {
            var list = new List<GraphemeCluster>();
            foreach (var item in value.EnumerateGraphemes())
            {
                list.Add(item);
            }
            return list.AsReadOnly();
        }
    }
}