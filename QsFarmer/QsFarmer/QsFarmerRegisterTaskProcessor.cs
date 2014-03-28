using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmer
{
	public class QsFarmerRegisterTaskProcessorFactory : QsITaskProcessorFactory
	{
		public QsITaskProcessor CreateTaskProcessor()
		{
			return new QsFarmerRegisterTaskProcessor();
		}
	}
	
	public class QsFarmerRegisterTaskProcessor : QsITaskProcessor
	{
		private int port;
		private int ip;
		private QsPackageHeader header;
		private QsPackageData data;
		
		public void SetIPAndPort(int ip, int port)
		{
			this.ip = ip;
			this.port = port;
		}
	
		public int UnPack(QsPackageHeader header, QsPackageData data)
		{
			return 0;
		}
	
		public void Run()
		{
			QsNetIOSender("127.0.0.1", 5252, header, data);
		}
		
		public void Pack()
		{
			byte[] buffer = new byte[8];
			BitConverter.GetBytes(ip).CopyTo(buffer, 0);
			BitConverter.GetBytes(port).CopyTo(buffer, 4);
			
			data = new QsPackageData(buffer);
			header = new QsPackageHeader(0x1, data);
		}
	}
}