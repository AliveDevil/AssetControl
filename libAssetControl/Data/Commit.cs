using System;
using System.Collections.Generic;
using System.Linq;

namespace libAssetControl.Data
{
	public sealed class Commit : ProjectItem
	{
		public Guid Id { get; set; }
		public Branch Branch { get; set; }
		public Commit Parent { get; set; }
		public IEnumerable<Asset> Changes { get; set; }

		public Commit(Project project, Branch branch, Commit parent, Guid id)
			: base(project)
		{
			Id = id;
			Parent = parent;
			Branch = branch;
			Changes = Project.Assets.Where(asset => asset.Commit == this);
		}
	}
}
