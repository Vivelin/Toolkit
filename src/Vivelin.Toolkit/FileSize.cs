using System;

namespace Vivelin.Toolkit
{
    /// <summary>
    /// Represents a file size
    /// </summary>
    public readonly struct FileSize : IEquatable<FileSize>,
        IComparable<FileSize>
    {
        private const double Base2Factor = 1024;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSize"/> struct.
        /// </summary>
        /// <param name="sizeInBytes">The number of bytes.</param>
        public FileSize(long sizeInBytes)
        {
            Bytes = sizeInBytes;
        }

        /// <summary>
        /// Gets the file size in bytes.
        /// </summary>
        public long Bytes { get; }

        /// <summary>
        /// Gets the file size in kilobytes.
        /// </summary>
        public double Kilobytes => Bytes / Base2Factor;

        /// <summary>
        /// Gets the file size in megabytes.
        /// </summary>
        public double Megabytes => Kilobytes / Base2Factor;

        /// <summary>
        /// Gets the file size in gigabytes.
        /// </summary>
        public double Gigabytes => Megabytes / Base2Factor;

        /// <summary>
        /// Gets the file size in terabytes.
        /// </summary>
        public double Terabytes => Gigabytes / Base2Factor;

        /// <summary>
        /// Returns a file size for the specifies amount of kilobytes.
        /// </summary>
        /// <param name="value">The number of kilobytes (base 2).</param>
        /// <returns>
        /// A new <see cref="FileSize"/> for <paramref name="value"/>.
        /// </returns>
        public static FileSize KB(double value)
            => new FileSize((long)(value * Base2Factor));

        /// <summary>
        /// Returns a file size for the specifies amount of megabytes.
        /// </summary>
        /// <param name="value">The number of megabytes (base 2).</param>
        /// <returns>
        /// A new <see cref="FileSize"/> for <paramref name="value"/>.
        /// </returns>
        public static FileSize MB(double value)
            => KB(value * Base2Factor);

        /// <summary>
        /// Returns a file size for the specifies amount of gigabytes.
        /// </summary>
        /// <param name="value">The number of gigabytes (base 2).</param>
        /// <returns>
        /// A new <see cref="FileSize"/> for <paramref name="value"/>.
        /// </returns>
        public static FileSize GB(double value)
            => MB(value * Base2Factor);

        /// <summary>
        /// Returns a file size for the specifies amount of terabytes.
        /// </summary>
        /// <param name="value">The number of terabytes (base 2).</param>
        /// <returns>
        /// A new <see cref="FileSize"/> for <paramref name="value"/>.
        /// </returns>
        public static FileSize TB(double value)
            => GB(value * Base2Factor);

        public static bool operator ==(FileSize left, FileSize right)
            => left.Equals(right);

        public static bool operator !=(FileSize left, FileSize right)
            => !(left == right);

        public static bool operator >(FileSize left, FileSize right)
            => left.CompareTo(right) > 0;

        public static bool operator <(FileSize left, FileSize right)
            => left.CompareTo(right) < 0;

        public static bool operator >=(FileSize left, FileSize right)
            => left.CompareTo(right) >= 0;

        public static bool operator <=(FileSize left, FileSize right)
            => left.CompareTo(right) <= 0;

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">
        /// The object to compare with the current instance.
        /// </param>
        /// <returns>
        /// true if <paramref name="obj">obj</paramref> and this instance are
        /// the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is FileSize other)
                return Equals(other);
            return false;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of
        /// the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref
        /// name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(FileSize other)
            => Bytes == other.Bytes;

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
            => Bytes.GetHashCode();

        /// <summary>
        /// Compares the current instance with another file size and returns an
        /// integer that indicates whether the current instance precedes,
        /// follows, or occurs in the same position in the sort order as the
        /// other file size.
        /// </summary>
        /// <param name="other">
        /// A file size to compare with this instance.
        /// </param>
        /// <returns>
        /// A value that indicates the relative order of the objects being
        /// compared. The return value has these meanings:
        /// <list type="table">
        /// <listheader>Value</listheader>
        /// <listheader>Meaning</listheader>
        /// </list>
        /// <item>
        /// <term>Less than zero</term>
        /// <term>
        /// This instance precedes <paramref name="other"/> in the sort order.
        /// </term>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <term>
        /// This instance occurs in the same position in the sort order as
        /// <paramref name="other"/>.
        /// </term>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <term>
        /// This instance follows <paramref name="other"/> in the sort order.
        /// </term>
        /// </item>
        /// </returns>
        public int CompareTo(FileSize other)
            => Bytes.CompareTo(other.Bytes);
    }
}