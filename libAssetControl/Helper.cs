using Newtonsoft.Json;

namespace libAssetControl
{
	internal static class Helper
	{
		internal static void Null(this JsonWriter writer)
		{
			writer.WriteNull();
		}

		internal static void Null(this JsonWriter writer, string name)
		{
			writer.WritePropertyName(name);
			writer.WriteNull();
		}

		internal static void Property<T>(this JsonWriter writer, string name, T value)
		{
			writer.WritePropertyName(name);
			writer.WriteValue(value);
		}

		internal static void Property(this JsonWriter writer, string name)
		{
			writer.WritePropertyName(name);
		}

		internal static void Value<T>(this JsonWriter writer, T value)
		{
			writer.WriteValue(value);
		}
	}
}
