using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace libAssetControl
{
	public sealed class AssetStore : IList<Asset>, INotifyCollectionChanged
	{
		public event NotifyCollectionChangedEventHandler CollectionChanged;

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

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (CollectionChanged != null)
			{
				CollectionChanged(this, e);
			}
		}
	}
}
