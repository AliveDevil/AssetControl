using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using libAssetControl.Network.Messages;

namespace libAssetControl.Network
{
	public delegate void MessageHandler(Client client, object message);

	public abstract class Client : IDisposable
	{
		private BinaryFormatter formatter;
		private Dictionary<Type, MessageHandler> messages;
		private Action reading;
		private IAsyncResult readResult;
		private DeflateStream readStream;
		private TcpClient tcpClient;
		private NetworkStream tcpStream;
		private DeflateStream writeStream;

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
			writeStream.Flush();
			readStream.Flush();
			writeStream.Dispose();
			readStream.Dispose();
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
			formatter.Serialize(writeStream, message);
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

			Connected = true;
			tcpStream = tcpClient.GetStream();

			readStream = new DeflateStream(tcpStream, CompressionMode.Decompress, true);
			writeStream = new DeflateStream(tcpStream, CompressionMode.Compress, true);
			reading = Read;
			Initialize();
			readResult = reading.BeginInvoke(ReadEnded, null);
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
					object message = formatter.Deserialize(readStream);
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
