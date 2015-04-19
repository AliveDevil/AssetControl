using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using libAssetControl.Network;

namespace AssetControl.Daemon
{
	public class DaemonClient : Client
	{
		public DaemonClient(TcpClient client)
			: base(client)
		{

		}
	}
}
