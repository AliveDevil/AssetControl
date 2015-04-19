using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace libAssetControl.Network
{
	public class Host : IDisposable
	{
		private TcpListener listener;
		private HashSet<Client> clients;
		private Func<TcpClient, Client> clientFactory;
		private ManualResetEventSlim waitHandle;

		public Host(Func<TcpClient, Client> factory, int port)
		{
			waitHandle = new ManualResetEventSlim();
			clientFactory = factory;
			clients = new HashSet<Client>();
			listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
			listener.Start();
			listener.BeginAcceptTcpClient(AcceptTcpClient, null);
		}

		private void AcceptTcpClient(IAsyncResult result)
		{
			TcpClient client = listener.EndAcceptTcpClient(result);
			listener.BeginAcceptTcpClient(AcceptTcpClient, null);
			clients.Add(clientFactory(client));
		}

		public void Wait()
		{
			waitHandle.Wait();
		}

		public void Dispose()
		{
			listener.Stop();
			foreach (var client in clients)
			{
				client.Disconnect();
			}
			waitHandle.Set();
		}
	}
}
