using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libAssetControl.Data
{
	public sealed class Branch : ProjectItem
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public IEnumerable<Commit> Commits { get; set; }

		public Branch(Project project, Guid id, string name)
			: base(project)
		{
			Commits = Project.Commits.Where(commit => commit.Branch == this);
		}
	}
}
