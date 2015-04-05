using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using libAssetControl.Network.Messages;

namespace libAssetControl.Network
{
	public class Client : IDisposable
	{
		public bool Connected { get; private set; }

		private TcpClient tcpClient;
		private NetworkStream tcpStream;
		private DeflateStream readingStream;
		private DeflateStream writingStream;
		private Action reading;
		private IAsyncResult readResult;
		private BinaryFormatter formatter;

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

		private void TcpClientConnected(IAsyncResult result)
		{
			tcpClient.EndConnect(result);
			Initialize();
		}

		private void Initialize()
		{
			Connected = true;
			tcpStream = tcpClient.GetStream();

			readingStream = new DeflateStream(tcpStream, CompressionMode.Decompress, true);
			writingStream = new DeflateStream(tcpStream, CompressionMode.Compress, true);
			reading = Read;
			readResult = reading.BeginInvoke(ReadEnded, null);
		}

		private void ReadEnded(IAsyncResult result)
		{
			reading.EndInvoke(result);
		}

		private void Read()
		{
			while (tcpClient.Connected)
			{
				if (tcpStream.DataAvailable)
				{
					object message = formatter.Deserialize(readingStream);
					Type t = message.GetType();
				}
			}
		}

		public void Disconnect()
		{
			Write(DisconnectMessage.Message);
		}

		public void Write(object message)
		{
			formatter.Serialize(writingStream, message);
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
	}
}
