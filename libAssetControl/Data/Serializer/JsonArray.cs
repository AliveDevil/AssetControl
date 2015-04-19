using Newtonsoft.Json;

namespace libAssetControl.Data.Serializer
{
	internal class JsonArray : JsonDisposable
	{
		public JsonArray(JsonWriter writer, string name)
			: base(writer, name)
		{
			Writer.WriteStartArray();
		}

		public JsonArray(JsonWriter writer)
			: base(writer)
		{
			Writer.WriteStartArray();
		}

		public override void Dispose()
		{
			Writer.WriteEndArray();
		}
	}
}
