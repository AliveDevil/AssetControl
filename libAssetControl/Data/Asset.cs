using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libAssetControl.Data
{
	public sealed class Asset : ProjectItem
	{
		public Commit Commit { get; set; }

		public Asset(Project project)
			: base(project)
		{

		}
	}
}
