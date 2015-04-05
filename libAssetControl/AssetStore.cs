using System.Collections.Generic;

namespace libAssetControl
{
	public sealed class AssetStore
	{
		private static AssetStore store;

		private List<Asset> assets;

		public static AssetStore Store
		{
			get { return store ?? (store = new AssetStore()); /* Keep one instance active. Simple singleton. */ }
		}

		private AssetStore()
		{
			assets = new List<Asset>();
		}
	}
}
