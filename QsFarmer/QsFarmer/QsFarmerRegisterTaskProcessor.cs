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
			
		}
		
		public void Pack()
		{
			
		}
	}
}