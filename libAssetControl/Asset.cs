using System;
using System.Collections.Generic;

namespace libAssetControl
{
	public class Asset
	{
		public byte[] Data { get; set; }

		[Obsolete]
		public string Directory { get { return Path; } set { Path = value; } }

		public string Extension { get; set; }

		public string FileName { get; set; }

		public Guid Guid { get; set; }

		public AssetHistory History { get; set; }

		public string Path { get; set; }

		public IList<string> Tags { get; set; }

		public void UpdateAsset(byte[] newData)
		{
		}
	}
}
