using System.Collections.Generic;
namespace libAssetControl
{
	public sealed class AssetStore
	{
		private static AssetStore store;

		public static AssetStore Store
		{
			get { return store ?? (store = new AssetStore()); }
		}

		private List<Asset> assets;

		private AssetStore()
		{
			assets = new List<Asset>();
		}
	}
}
