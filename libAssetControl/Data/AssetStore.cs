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

		public void Save(Stream stream)
		{
			using (TextWriter textWriter = new StreamWriter(stream))
			using (JsonWriter writer = new JsonTextWriter(textWriter))
				SerializerFactory.LatestSerializer().Save(this, writer);
		}
	}
}
