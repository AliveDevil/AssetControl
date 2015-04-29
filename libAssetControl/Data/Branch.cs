using System;
using System.Collections.Generic;
using System.Linq;

namespace libAssetControl.Data
{
	public sealed class Branch : ProjectItem
	{
		public IEnumerable<Commit> Commits { get; set; }

		public Guid Id { get; set; }

		public string Name { get; set; }

		public Branch(Project project, Guid id, string name)
			: base(project)
		{
			Id = id;
			Name = name;
			Commits = Project.Commits.Where(commit => commit.Branch == this);
		}
	}
}
