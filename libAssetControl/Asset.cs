using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libAssetControl
{
	public class Asset
	{
		public string Directory { get; set; }
		public string FileName { get; set; }
		public string Extension { get; set; }
		public byte[] Data { get; set; }
		public byte[] Meta { get; set; }
	}
}
