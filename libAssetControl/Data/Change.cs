using System;
using System.Collections.Generic;
using System.Text;

namespace libAssetControl.Data
{
	public sealed class Change : ProjectItem
	{
		public Commit Commit { get; set; }

		public Change(Project project)
			: base(project)
		{

		}
	}
}
