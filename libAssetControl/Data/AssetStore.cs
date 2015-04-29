using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using libAssetControl.Data.Serializer;
using Newtonsoft.Json;

namespace libAssetControl.Data
{
	public class AssetStore
	{
		public ICollection<Project> Projects { get; private set; }

		public ICollection<User> Users { get; private set; }

		public AssetStore()
		{
			Users = new Collection<User>();
			Projects = new Collection<Project>();
		}

		public void Load(Stream stream)
		{
			using (TextReader textReader = new StreamReader(stream))
			using (JsonReader reader = new JsonTextReader(textReader))
			{
				// skip JsonToken.StartObject
				reader.Read();
				// first property has to be version!
				if (!reader.Read() || (string)reader.Value != "version")
					return; // throw an exception!
				SerializerFactory.ImporterForVersion(reader.ReadAsString()).Load(this, reader);
			}
		}

		public void Save(Stream stream)
		{
			using (TextWriter textWriter = new StreamWriter(stream))
			using (JsonWriter writer = new JsonTextWriter(textWriter))
				SerializerFactory.LatestSerializer().Save(this, writer);
		}
	}
}
