using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using libAssetControl.Network.Messages;

namespace libAssetControl.Network
{
	public delegate void MessageReceivedEventHandler(Client client, object message);

	public class Client : IDisposable
	{
		private delegate void MessageHandler(object message);

		public event MessageReceivedEventHandler MessageReceived;

		private BinaryFormatter formatter;

		private Dictionary<Type, MessageHandler> messages;
		private Action reading;

		private DeflateStream readingStream;

		private IAsyncResult readResult;

		private TcpClient tcpClient;

		private NetworkStream tcpStream;

		private DeflateStream writingStream;

		public bool Connected { get; private set; }

		public Client(IPAddress target, int port)
		{
			tcpClient = new TcpClient();
			tcpClient.BeginConnect(target, port, TcpClientConnected, null);
		}

		internal Client(TcpClient client)
		{
			tcpClient = client;
			Initialize();
		}

		public void Disconnect()
		{
			Write(DisconnectMessage.Message);
		}

		public void Dispose()
		{
			writingStream.Flush();
			readingStream.Flush();
			writingStream.Dispose();
			readingStream.Dispose();
			readResult.AsyncWaitHandle.WaitOne();
			tcpStream.Dispose();
			tcpClient.Close();
		}

		public void Write(object message)
		{
			formatter.Serialize(writingStream, message);
		}

		private void HandleDisconnectMessage(object message)
		{
			Disconnect();
		}

		private void HandleHeloMessage(object message)
		{
			Write(HeloMessage.Message);
		}

		private void Initialize()
		{
			messages = new Dictionary<Type, MessageHandler>()
			{
				{typeof(HeloMessage), HandleHeloMessage},
				{typeof(DisconnectMessage), HandleDisconnectMessage}
			};

			Connected = true;
			tcpStream = tcpClient.GetStream();

			readingStream = new DeflateStream(tcpStream, CompressionMode.Decompress, true);
			writingStream = new DeflateStream(tcpStream, CompressionMode.Compress, true);
			reading = Read;
			readResult = reading.BeginInvoke(ReadEnded, null);
		}

		private void OnMessageReceived(object message)
		{
			if (MessageReceived != null)
			{
				MessageReceived(this, message);
			}
		}

		private void Read()
		{
			while (tcpClient.Connected)
			{
				if (tcpStream.DataAvailable)
				{
					object message = formatter.Deserialize(readingStream);
					Type t = message.GetType();
					if (!ResolveMessage(t, message))
					{
						OnMessageReceived(message);
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
				handler(message);
				return true;
			}
			return false;
		}

		private void TcpClientConnected(IAsyncResult result)
		{
			tcpClient.EndConnect(result);
			Initialize();
		}
	}
}
