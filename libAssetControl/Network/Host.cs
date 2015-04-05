using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace libAssetControl.Network
{
	public class Host : IDisposable
	{
		private TcpListener listener;
		private HashSet<Client> clients;

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
			clients.Add(new Client(client));
		}

		public void Dispose()
		{
			listener.Stop();
			foreach (var client in clients)
			{
				client.Disconnect();
			}
		}
	}
}
