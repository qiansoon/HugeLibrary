using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public class QsTaskProcess
    {
        protected QsPackageHeader header;
        protected QsPackageData data;

        public QsTaskProcess()
        { 
        }

        public QsTaskProcess(QsPackageHeader h, QsPackageData d)
        {
            header = h;
            data = d;
        }

        public virtual void Process() { }
    }

    public class QsTaskProcessFactory
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
            switch (cmdType)
            {
                case 0x1:
                    return new QsDisplayClientContent(header, data);
            }
            return null;
        }
    }

    public class QsDisplayClientContent : QsTaskProcess
    {
        private string content;

        public QsDisplayClientContent(QsPackageHeader h, QsPackageData d)
            : base(h, d)
        {
            content = System.Text.Encoding.Default.GetString(d.dataBuffer);
        }

        public override void Process()
	    {
		    System.Console.WriteLine(content);
	    }
    }

    public class QsSendInfoToServer : QsTaskProcess
    {
        private string content;

        public QsSendInfoToServer(string s)
        {
            data = new QsPackageData(System.Text.Encoding.Default.GetBytes(s));
            header = new QsPackageHeader(0x1, data);
        }

        public override void Process()
        {
            QsNetIOSender sender = new QsNetIOSender("127.0.0.1", 5252, header, data);
            sender.Send();
        }
    }
}
