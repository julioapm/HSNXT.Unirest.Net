using System;

namespace HSNXT.Unirest.Net.Entities
{
    /// <summary>
    /// Represents a value which may or may not have a value.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    public struct UOptional<T> : IEquatable<UOptional<T>>, IEquatable<T>
    {
        /// <summary>
        /// Gets whether this <see cref="UOptional{T}"/> has a value.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the value of this <see cref="UOptional{T}"/>.
        /// </summary>
        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException("Value is not set.");
                return _val;
            }
        }
        private readonly T _val;

        /// <summary>
        /// Creates a new <see cref="UOptional{T}"/> with specified value.
        /// </summary>
        /// <param name="value">Value of this option.</param>
        public UOptional(T value)
        {
            _val = value;
            HasValue = true;
        }
        
        /// <summary>
        /// Returns a string representation of this optional value.
        /// </summary>
        /// <returns>String representation of this optional value.</returns>
        public override string ToString()
        {
            return $"Unirest.Optional<{typeof(T)}> ({(HasValue ? Value.ToString() : "<no value>")})";
        }

        /// <summary>
        /// Checks whether this <see cref="UOptional{T}"/> (or its value) are equal to another object.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>Whether the object is equal to this <see cref="UOptional{T}"/> or its value.</returns>
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case T t:
                    return Equals(t);
                case UOptional<T> opt:
                    return Equals(opt);
            }

            return false;
        }

        /// <inheritdoc />
        /// <summary>
        /// Checks whether this <see cref="UOptional{T}" /> is equal to another <see cref="UOptional{T}" />.
        /// </summary>
        /// <param name="e"><see cref="UOptional{T}" /> to compare to.</param>
        /// <returns>Whether the <see cref="UOptional{T}" /> is equal to this <see cref="UOptional{T}" />.</returns>
        public bool Equals(UOptional<T> e)
        {
            if (!HasValue && !e.HasValue)
                return true;

            return HasValue == e.HasValue && Value.Equals(e.Value);
        }

        /// <inheritdoc />
        /// <summary>
        /// Checks whether the value of this <see cref="UOptional{T}" /> is equal to specified object.
        /// </summary>
        /// <param name="e">Object to compare to.</param>
        /// <returns>Whether the object is equal to the value of this <see cref="UOptional{T}" />.</returns>
        public bool Equals(T e)
        {
            return HasValue && ReferenceEquals(Value, e);
        }

        /// <summary>
        /// Gets the hash code for this <see cref="UOptional{T}"/>.
        /// </summary>
        /// <returns>The hash code for this <see cref="UOptional{T}"/>.</returns>
        public override int GetHashCode()
        {
            return HasValue ? Value.GetHashCode() : 0;
        }

        /// <summary>
        /// Creates a new <see cref="UOptional{T}"/> with specified value and valid state.
        /// </summary>
        /// <param name="value">Value to populate the optional with.</param>
        /// <returns>Created optional.</returns>
        public static UOptional<T> FromValue(T value)
            => new UOptional<T>(value);

        /// <summary>
        /// Creates a new empty <see cref="UOptional{T}"/> with no value and invalid state.
        /// </summary>
        /// <returns>Created optional.</returns>
        public static UOptional<T> FromNoValue()
            => default;

        public static implicit operator UOptional<T>(T val) 
            => new UOptional<T>(val);

        public static explicit operator T(UOptional<T> opt) 
            => opt.Value;

        public static bool operator ==(UOptional<T> opt1, UOptional<T> opt2) 
            => opt1.Equals(opt2);

        public static bool operator !=(UOptional<T> opt1, UOptional<T> opt2) 
            => !opt1.Equals(opt2);

        public static bool operator ==(UOptional<T> opt, T t) 
            => opt.Equals(t);

        public static bool operator !=(UOptional<T> opt, T t) 
            => !opt.Equals(t);

        public UOptional<TTarget> IfPresent<TTarget>(Func<T, TTarget> mapper)
        {
            return HasValue ? new UOptional<TTarget>(mapper(Value)) : default;
        }
    }
}
