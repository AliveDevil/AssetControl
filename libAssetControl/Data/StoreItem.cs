using System;
using System.Collections.Generic;
using System.Text;

namespace libAssetControl.Data
{
	public abstract class StoreItem
	{
		protected AssetStore Store { get; private set; }

		public StoreItem(AssetStore store)
		{
			Store = store;
		}
	}
}
