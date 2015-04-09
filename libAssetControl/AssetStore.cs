using System;
using System.Collections.Generic;

namespace libAssetControl
{
	public sealed class AssetStore : IList<Asset>, IEnumerator<Asset>
	{
		private List<Asset> assets;
		private Asset currentAsset;
		private string directory;
		private int index;

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

		public Asset Current
		{
			get { return currentAsset; }
		}

		public string Directory
		{
			get { return directory; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		object System.Collections.IEnumerator.Current
		{
			get { return currentAsset; }
		}

		public AssetStore(string directory)
		{
			assets = new List<Asset>();
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
			return this;
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

		public bool MoveNext()
		{
			if (++index >= assets.Count)
				return false;
			currentAsset = assets[index];
			return true;
		}

		public bool Remove(Asset item)
		{
			return assets.Remove(item);
		}

		public void RemoveAt(int index)
		{
			assets.RemoveAt(index);
		}

		public void Reset()
		{
			index = -1;
			currentAsset = null;
		}

		public void Save()
		{
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this;
		}
	}
}
