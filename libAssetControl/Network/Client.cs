using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using libAssetControl.Network.Messages;

namespace libAssetControl.Network
{
	public delegate void MessageHandler(Client client, object message);

	public abstract class Client : IDisposable
	{
		public delegate void ReadAction();

		private BinaryFormatter formatter;
		private Dictionary<Type, MessageHandler> messages;
		private ReadAction reading;
		private IAsyncResult readResult;
		private TcpClient tcpClient;
		private NetworkStream tcpStream;

		public bool Authed { get; private set; }

		public bool Connected { get; private set; }

		protected Client(IPAddress target, int port)
		{
			tcpClient = new TcpClient();
			tcpClient.BeginConnect(target, port, TcpClientConnected, null);
		}

		protected Client(TcpClient client)
		{
			tcpClient = client;
			EarlyInitialize();
		}

		public void Disconnect()
		{
			Write(DisconnectMessage.Message);
		}

		public void Dispose()
		{
			tcpStream.Flush();
			readResult.AsyncWaitHandle.WaitOne();
			tcpStream.Dispose();
			tcpClient.Close();
		}

		public void Register<T>(MessageHandler handler)
		{
			messages[typeof(T)] += handler;
		}

		public void Write(object message)
		{
			formatter.Serialize(tcpStream, message);
		}

		protected virtual void Initialize()
		{
		}

		private void EarlyInitialize()
		{
			formatter = new BinaryFormatter();

			messages = new Dictionary<Type, MessageHandler>();

			Register<HeloMessage>(HandleHeloMessage);
			Register<DisconnectMessage>(HandleDisconnectMessage);
			Register<AuthMessage>(HandleAuthMessage);

			Connected = true;
			tcpStream = tcpClient.GetStream();

			reading = Read;
			Initialize();
			readResult = reading.BeginInvoke(ReadEnded, null);
		}

		private void HandleAuthMessage(Client client, object message)
		{
			if (!(message is AuthMessage)) return;
			AuthMessage authMessage = (AuthMessage)message;
			Authed = authMessage.IsAuthed;
		}

		private void HandleDisconnectMessage(Client c, object message)
		{
			Disconnect();
		}

		private void HandleHeloMessage(Client c, object message)
		{
			Write(HeloMessage.Message);
		}

		private void Read()
		{
			while (tcpClient.Connected)
			{
				if (tcpStream.DataAvailable)
				{
					object message = formatter.Deserialize(tcpStream);
					Type t = message.GetType();
					if (!ResolveMessage(t, message))
					{
						Trace.TraceError("Unknown messagetype: {0}", t.Name);
					}
				}
			}
		}

		private void ReadEnded(IAsyncResult result)
		{
			reading.EndInvoke(result);
		}

		private bool ResolveMessage(Type t, object message)
		{
			MessageHandler handler;
			if (messages.TryGetValue(t, out handler))
			{
				handler(this, message);
				return true;
			}
			return false;
		}

		private void TcpClientConnected(IAsyncResult result)
		{
			tcpClient.EndConnect(result);
			EarlyInitialize();
		}
	}
}
