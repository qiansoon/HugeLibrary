using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QsFarmer
{
    public class QsFarmerRegisterTaskProcessorFactory : QsITaskFactory
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
		
		public void SetIPAndPort(int farmerIP, int farmerPort)
		{
			this.ip = farmerIP;
			this.port = farmerPort;
		}
	
		public int UnPack(QsPackageHeader header, QsPackageData data)
		{
			return 0;
		}
	
		public void Run()
		{
			QsNetIOSender sender = new QsNetIOSender("127.0.0.1", 5252, header, data);
			sender.Send();
		}
		
		public void Pack()
		{
			byte[] buffer = new byte[8];
			BitConverter.GetBytes(ip).CopyTo(buffer, 0);
			BitConverter.GetBytes(port).CopyTo(buffer, 4);
			
			data = new QsPackageData(buffer);
            header = new QsPackageHeader((int)QsFarmer.QsITaskFactory.QsTaskType.QsETaskFarmerRegister, data);
		}
	}
}