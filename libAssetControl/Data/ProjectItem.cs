using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libAssetControl.Data
{
	public abstract class ProjectItem
	{
		protected Project Project { get; private set; }

		public ProjectItem(Project project)
		{
			Project = project;
		}
	}
}
