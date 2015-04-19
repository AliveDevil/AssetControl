using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace libAssetControl.Data
{
	public sealed class Project : StoreItem
	{
		public string Name { get; set; }
		public ICollection<User> Users { get; set; }
		public ICollection<Branch> Branches { get; set; }
		public ICollection<Commit> Commits { get; set; }
		public ICollection<Change> Changes { get; set; }

		public Project(AssetStore store, string name)
			: base(store)
		{
			Name = name;
			Users = new Collection<User>();
			Branches = new Collection<Branch>();
			Commits = new Collection<Commit>();
			Changes = new Collection<Change>();
		}
	}
}
