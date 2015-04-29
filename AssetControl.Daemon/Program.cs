using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using libAssetControl.Data;
using libAssetControl.Network;

namespace AssetControl.Daemon
{
	class Program
	{
		static void Main(string[] args)
		{
			string file = ConfigurationManager.AppSettings["StoreFile"];
			FileInfo storeFile = new FileInfo(file);
			AssetStore store = new AssetStore();
			if (!storeFile.Exists)
				using (Stream fileStream = storeFile.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
					store.Save(fileStream);
			using (Stream fileStream = storeFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
				store.Load(fileStream);
			Host host = new Host(c => new DaemonClient(c, store), 13337);
			host.Wait();
		}
	}
}
