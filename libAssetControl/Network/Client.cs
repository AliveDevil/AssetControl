using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace libAssetControl.Network
{
	public class Client
	{
		public bool Connected { get; private set; }

		private TcpClient tcpClient;
		private NetworkStream tcpStream;
		private SslStream sslStream;
		private DeflateStream readingStream;
		private DeflateStream writingStream;
		private StreamReader reader;
		private StreamWriter writer;
		private Action reading;
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
			sslStream = new SslStream(tcpStream, true);

			readingStream = new DeflateStream(tcpStream, CompressionMode.Decompress);
			writingStream = new DeflateStream(tcpStream, CompressionMode.Compress);
			reader = new StreamReader(readingStream);
			writer = new StreamWriter(writingStream);
			reading = Read;
			reading.BeginInvoke(ReadEnded, null);
		}

		private void ReadEnded(IAsyncResult result)
		{
			reading.EndInvoke(result);
		}

		private void Read()
		{

		}
	}
}
