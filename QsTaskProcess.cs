public class QsTaskProcess
{
	private QsPackageHeader header;
	private QsPackageData data;
	
	public QsTaskProcess(QsPackageHeader h, QsPackageData d)
	{
		header = h;
		data = d;
	}
	
	public virtual void Process();
}

public class QsTaskProcessFactory()
{
	private QsPackageHeader header;
	private QsPackageData data;
	
	public QsTaskProcessFactory(QsPackageHeader h, QsPackageData d)
	{
		header = h;
		data = d;
	}

	public QsTaskProcess GetTaskProcessor(int cmdType)
	{
		switch(cmdType)
		{
		case 0x1:
			return new QsDisplayClientContent(h, d);
		}
	}
}

public class QsDisplayClientContent : QsTaskProcess
{
	private string content;
	
	public QsDisplay(QsPackageHeader h, QsPackageData d) : base(h, d)
	{
		content = System.Text.Encoding.Default.GetString(d.dataBuffer);
	}
	
	public override void Process()
	{
		System.Console.WriteLine(string);
	}
}

public class QsSendInfoToServer : QsTaskProcess
{
	private string content;
	
	public QsSendInfoToServer(stirng s)
	{
		data = new QsPackage(System.Text.Encoding.Default.GetBytes(s));
		header = new QsPackageHeader(0x1, data);
	}
	
	public override void Process()
	{
		QsNetIOSender sender = new QsNetIOSender("127.0.0.1", 5252, header, data);
		sender.Send();
	}
}