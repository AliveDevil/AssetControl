using System;
using Newtonsoft.Json;

namespace libAssetControl.Data.Serializer
{
	internal class JsonArray : JsonDisposable
	{
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
