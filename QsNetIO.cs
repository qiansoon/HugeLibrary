public class QsNetIOStateObject
{
	public Socket workSocket = null;
	public const int bufferSize = 1024;
	public int count = 0;
	public byte[] buffer = new byte[bufferSize];
}

public class QsPackageHeader
{
	public const int headerBufferSize = 5;
	public byte[] headerBuffer = new byte[headerBufferSize];
	public QsPackageHeader(){}
	public QsPackageHeader(int cmdType, QsPackageData data)
	{
		headerBuffer[0] = (byte)cmdType;
		fixed(int *p = &headerBuffer[1])
		{
			*p = data.Length;
		}
	}
	public byte GetCmdType()
	{
		return headerBuffer[0];
	}
	public int GetDataLength()
	{
		return BitConverter.ToInt32(headerBuffer, 1);
	}
}

public class QsPackageData
{
	public int dataLength;
	public byte[] dataBuffer;	
	public QsPackageData(){}
	public QsPackageData(int length)
	{
		dataLength = length;
		dataBuffer = new byte[length];
	}
	public QsPackageData(byte[] data)
	{
		dataLength = data.Length;
		dataBuffer = data;
	}
}

public class QsNetIOListener
{
	private Socket listenSocket = null;
	private IPAddress listenIP = IPAddress.Parse("127.0.0.1");
	private int listenPort;
	private IPEndPoint localEndPoint;
	private ManualResetEvent listenDone = new ManualResetEvent(false);
	
	public QsNetIOListener(int port)
	{
		listenPort = port;
	}
	
	public void Listen()
	{
		localEndPoint = new IPEndPoint(listenIP, listenPort);
		listenSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
		
		listenSocket.Bind(localEndPoint);
		listenSocket.Listen(100);
		while (true)
		{
			listenDone.Reset();
			listenSocket.BeginAccept(new AsyncCallback(AcceptCallback), listenSocket);//Begin to listen
			listenDone.WaitOne();//Wait until connection is processed
		}
	}
	
	private void AcceptCallback(IAsyncResult ar)
	{
		//Process connection to remote point
		Socket remoteSocket = listenSocket.EndAccept(ar);
		listenDone.Set();
		//Begin to receive package
		QsNetIOReceiver receiver = new QsNetIOReceiver(remoteSocket);
		receiver.Receive();
	}
}

public class QsNetIOReceiver
{
	private Socket receiveSocket = null;
	private QsPackageHeader header = new QsPackageHeader();
	private QsPackageData data = null; 
	
	public QsNetIOReceiver(socket s)
	{
		receiveSocket = s;
	}
	
	public void Receive()
	{
		QsNetIOStateObject state = new QsNetIOStateObject();
		receiveSocket.BeginReceive(state.buffer, state.count, QsPackageHeader.headerBufferSize - state.count, 0, new AsyncCallback(ReceiveHeaderCallback), state);
	}
	
	public void ReceiveHeaderCallback(IAsyncResult ar)
	{
		QsNetIOStateObject headerState = (QsNetIOStateObject)ar.AsyncState;
		int bytesRead = receiveSocket.EndReceive(ar);
		if (bytesRead > 0)
		{
			state.count += bytesRead;
			if (state.count < QsPackageHeader.headerBufferSize)
				receiveSocket.BeginReceive(header.headerBuffer, headerState.count, QsPackageHeader.headerBufferSize - headerState.count, 0, new AsyncCallback(ReceiveHeaderCallback), headerState);
			else
			{
				QsNetIOStateObject dataState = new QsNetIOStateObject();
				data = new QsPackageData(header.GetLength());
				receiveSocket.BeginReceive(data.buffer, dataState.count, data.dataLength - dataState.count, 0, new AsyncCallback(ReceiveDataCallback), dataState);
			}
		}
	}
	
	private void ReceiveDataCallback(IAsyncResult ar)
	{
		QsNetIOStateObject dataState = (QsNetIOStateObject)ar.AsyncState;
		int bytesRead = receiveSocket.EndReceive(ar);
		if (bytesRead > 0)
		{
			dataState.count += bytesRead;
			if (dataState.count < data.dataLength)
				receiveSocket.BeginReceive(data.buffer, dataState.count, data.dataLength - dataState.count, 0, new AsyncCallback(ReceiveDataCallback), dataState);
			else
			{
				//Finish package receiving. Then process package.
				TaskProcess processTask = new TaskProcess(header, data);
				processTask.Process();
			}	
		}
	}
}

public class QsNetIOSender
{
	private Socket sendSocket = null;
	private QsPackageHeader header = null;
	private QsPackageData data = null;
	private ManualResetEvent connectDone = new ManualResetEvent(false);
	
	private void Connect(string ip, int port)
	{
		IPAddress ipAddress = IPAddress.Parse(ip);
		IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
		sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		sendSocket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), sendSocket);
		connectDone.WaitOne();
	}
	
	private void ConnectCallback(IAsyncResult ar)
	{
		sendSocket.EndConnect(ar);
		connectDone.Set();
	}
	
	public QsNetIOSender(string ip, int port)
	{
		Connect(ip, port);
	}
	
	public QsNetIOSender(string ip, int port, QsPackageHeader h, QsPackageData d)
	{
		Connect(ip, port);
		header = h;
		data = d;
	}
	
	public QsNetIOSender(Socket s)
	{
		sendSocket = s;
	}
	
	public QsNetIOSender(Socket s, QsPackageHeader h, QsPackageData d)
	{
		sendSocket = s;
		header = h;
		data = d;
	}
	
	public void SetPackageHeader(QsPackageHeader h)
	{
		header = h;
	}
	
	public void SetPackageData(QsPackageData d)
	{
		data = d;
	}
	
	public void Send()
	{
		QsNetIOStateObject state = new QsNetIOStateObject();
		sendSocket.BeginSend(header.headerBuffer, state.count, QsPackageHeader.headerBufferSize - state.count, new AsyncCallback(SendHeaderCallback), state);
	}
	
	private void SendHeaderCallback(IAsyncResult ar)
	{
		QsNetIOStateObject headerState = (QsNetIOStateObject)ar.AsyncState;
		int bytesSend = sendSocket.EndSend(ar);
		if (bytesSend > 0)
		{
			headerState.count += bytesSend;
			if (headerState.count < QsPackageHeader.headerBufferSize)
				sendSocket.BeginSend(header.headerBuffer, headerState.count, QsPackageHeader.headerBufferSize - headerState.count, new AsyncCallback(SendHeaderCallback), headerState);
			else
			{
				QsNetIOStaetObject dataState = new QsNetIOStateObject();
				snedSocket.BeginSend(data.dataBuffer, dataState.count, data.dataLength - dataState.count, new AsyncCallback(SendDataCallback), dataState);
			}
		}
	}
	
	private void SendDataCallback(IAsyncResult ar)
	{
		QsNetIOStateObject dataState = (QsNetIOStateObjcet)ar.AsyncState;
		int bytesSend = sendSocket.EndSend(ar);
		if (bytesSend > 0)
		{
			dataState.count += bytesSend;
			if (dataState.count < data.dataLength)
				sendSocket(data.dataBuffer, dataState.count, data.dataLength - dataState.count, new AsyncCallback(SendDataCallback), dataState);
			else
				Close();
		}
		else
		{
			Close();
		}
	}
	
	private void Close()
	{
		sendSocket.Shutdown(SocketShutdown.Both);
		sendSocket.Close();
	}
}