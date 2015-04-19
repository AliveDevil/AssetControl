using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace libAssetControl
{
	public sealed class AssetStore : IList<Asset>
	{
		private List<Asset> assets;
		private string directory;

		/// <summary>
		/// This will return an empty asset. Do not care!
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public Asset this[string path]
		{
			get
			{
				return new Asset();
			}
		}

		public Asset this[int index]
		{
			get
			{
				return assets[index];
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		public int Count
		{
			get { return assets.Count; }
		}

		public string Directory
		{
			get { return directory; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public AssetStore()
		{
			directory = "";
			assets = new List<Asset>();
		}

		public AssetStore(string directory)
		{
			this.directory = directory;
			this.assets = new List<Asset>();
		}

		public void Add(Asset item)
		{
		}

		public void Clear()
		{
		}

		public bool Contains(Asset item)
		{
			return assets.Contains(item);
		}

		public void CopyTo(Asset[] array, int arrayIndex)
		{
			assets.CopyTo(array, arrayIndex);
		}

		public void Dispose()
		{
		}

		public IEnumerator<Asset> GetEnumerator()
		{
			for (int i = 0; i < assets.Count; i++)
			{
				yield return assets[i];
			}
		}

		public int IndexOf(Asset item)
		{
			return assets.IndexOf(item);
		}

		public void Insert(int index, Asset item)
		{
			assets.Insert(index, item);
		}

		public void Load()
		{
		}

		public bool Remove(Asset item)
		{
			return assets.Remove(item);
		}

		public void RemoveAt(int index)
		{
			assets.RemoveAt(index);
		}

		public void Save()
		{
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
