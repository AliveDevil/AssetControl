using Newtonsoft.Json;
namespace libAssetControl.Data.Serializer
{
	internal interface ISerializer
	{
		void Save(AssetStore store, JsonWriter writer);
		void Load(AssetStore store, JsonReader reader);
	}
}
