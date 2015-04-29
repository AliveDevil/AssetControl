using System;
using System.Collections.Generic;
using System.Text;

namespace libAssetControl.Data
{
	public sealed class Change : ProjectItem
	{
		public Commit Commit { get; set; }

		public Change(Project project, Commit commit)
			: base(project)
		{
			Commit = commit;
		}
	}
}
