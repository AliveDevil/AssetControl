using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace libAssetControl.Network
{
	public class Host
	{
		private TcpListener listener;

		public Host(int port)
		{
			listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
			listener.Start();
			listener.BeginAcceptTcpClient(AcceptTcpClient, null);
		}

		private void AcceptTcpClient(IAsyncResult result)
		{
			TcpClient client = listener.EndAcceptTcpClient(result);
			listener.BeginAcceptTcpClient(AcceptTcpClient, null);
		}
	}
}
