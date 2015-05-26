using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using libAssetControl.Data;
using libAssetControl.Network;
using libAssetControl.Network.Messages;

namespace AssetControl.Daemon
{
	public class DaemonClient : Client
	{
		private AssetStore store;

		public DaemonClient(TcpClient client, AssetStore store)
			: base(client)
		{
			this.store = store;
		}

		protected override void Initialize()
		{
			Register<AuthClientMessage>(authenticate);
		}

		private void authenticate(Client client, object message)
		{
			if (!(message is AuthClientMessage)) return;
			AuthClientMessage authMessage = (AuthClientMessage)message;

		}
	}
}
