﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libAssetControl.Network;

namespace AssetControl.Daemon
{
	class Program
	{
		static void Main(string[] args)
		{
			Host host = new Host(c => null, 13337);
		}
	}
}
