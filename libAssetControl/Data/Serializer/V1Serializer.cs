using Newtonsoft.Json;

namespace libAssetControl.Data.Serializer
{
	internal class V1Serializer : ISerializer
	{
		public void Load(AssetStore store, JsonReader reader)
		{
		}

		public void Save(AssetStore store, JsonWriter writer)
		{
			using (new JsonObject(writer))
			{
				WriteProperty("version", 1, writer);
				using (new JsonObject(writer, "Users"))
				using (new JsonArray(writer))
				{
					foreach (var user in store.Users)
					{
						using (new JsonObject(writer))
						{
							WriteProperty("name", user.Name, writer);
							WriteProperty("password", user.Password, writer);
						}
					}
				}
				using (new JsonObject(writer, "Projects"))
				using (new JsonArray(writer))
				{
					foreach (var project in store.Projects)
					{

					}
				}
			}
		}

		private void WriteProperty(string name, JsonWriter writer)
		{
			writer.WritePropertyName(name);
		}

		private void WriteProperty<T>(string name, T value, JsonWriter writer)
		{
			WriteProperty(name, writer);
			WriteValue(value, writer);
		}

		private void WriteValue<T>(T value, JsonWriter writer)
		{
			writer.WriteValue(value);
		}
	}
}
