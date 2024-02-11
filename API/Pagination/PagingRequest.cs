using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Pagination
{
    public class PagingRequest
    {

        public int? Page { get; set; } = 1;

        private PagesizeEnum _pageSize { get; set; } = PagesizeEnum.SEPULUH;
        public int PageSize
        {
            get => (int)_pageSize;
            set
            {
                if (Enum.IsDefined(typeof(PagesizeEnum), value)) _pageSize = (PagesizeEnum)value;
                else _pageSize = PagesizeEnum.SEPULUH;
            }
        }

        public List<SearchCriteria>? Search { get; set; } = [];
        public List<SortCriteria>? Sort { get; set; } = [];
    }

    public record SearchCriteria
    {
        public string PropertyName { get; set; }
        public string? PropertyValue { get; set; }
        public string Operator { get; set; }
        public string? PropertyValue1 { get; set; }
        public string? PropertyValue2 { get; set; }
    }
    public record SortCriteria
    {
        public const string ORDER_BY_DESCENDING = "desc";

        public bool IsAscending { get; set; } = true;
        public string PropertyNameOrder { get; set; }
    }

    public enum PagesizeEnum
    {
        SEPULUH = 10,
        DUA_PULUH = 20,
        LIMA_PULUH = 50,
        SERATUS = 100
    }

    public class CustomEnumConverter : JsonConverterFactory
    {
        // Add anything additional here such as typeToConvert.IsEnumWithDescription() to check for description attributes.
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
            (JsonConverter)Activator.CreateInstance(typeof(CustomConverter<>).MakeGenericType(typeToConvert))!;
    }

    class CustomConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString()!.GetEnumValue<T>()!;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) =>
            writer.WriteNumberValue(Convert.ToInt32(value));
    }

    public static class EnumExtensions
    {
        public static string GetDescription<TEnum>(this TEnum value) where TEnum : struct, Enum
        {
            return value.ToString();
        }
        public static TEnum GetEnumValue<TEnum>(this string value) where TEnum : struct, Enum
        {
            return Enum.Parse<TEnum>(value.AsSpan(), true);
        }

        public static bool IsEnumByString<TEnum>(this string value) where TEnum : struct, Enum
        {
            return Enum.TryParse<TEnum>(value.AsSpan(), true, out _);
        }

        public static bool IsIntegerType(this Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (type == typeof(long)
                || type == typeof(ulong)
                || type == typeof(int)
                || type == typeof(uint)
                || type == typeof(short)
                || type == typeof(ushort)
                || type == typeof(byte)
                || type == typeof(sbyte)
                || type == typeof(System.Numerics.BigInteger))
                return true;
            return false;
        }
    }
}
