﻿using System;
using System.ComponentModel;
using chancies.Server.Common.Converters;
using Newtonsoft.Json;

namespace chancies.Server.Persistence.Models
{
    [JsonConverter(typeof(IdGuidConverter<SectionId>))]
    [TypeConverter(typeof(UnderlyingTypeConverter<SectionId, Guid>))]
    public readonly struct SectionId
        : IEquatable<SectionId>
    {
        private readonly Guid _value;

        public SectionId(Guid value)
        {
            _value = value;
        }

        public static implicit operator Guid(SectionId id)
        {
            return id._value;
        }

        public static implicit operator SectionId(Guid value)
        {
            return new SectionId(value);
        }

        public static bool operator ==(SectionId left, SectionId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SectionId left, SectionId right)
        {
            return !left.Equals(right);
        }

        public bool Equals(SectionId other)
        {
            return Equals(_value, other._value);
        }

        public override bool Equals(object obj)
        {
            return obj is SectionId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
