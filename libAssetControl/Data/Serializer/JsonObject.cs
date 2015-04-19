using Newtonsoft.Json;

namespace libAssetControl.Data.Serializer
{
	internal class JsonObject : JsonDisposable
	{
		public JsonObject(JsonWriter writer, string name)
			: base(writer, name)
		{
			Writer.WriteStartObject();
		}
		public JsonObject(JsonWriter writer)
			: base(writer)
		{
			Writer.WriteStartObject();
		}

		public override void Dispose()
		{
			Writer.WriteEndObject();
		}
	}
}
