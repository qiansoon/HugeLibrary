using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QsFarmer
{
    public interface QsITaskProcessor
    {
		public int UnPack(QsPackageHeader header, QsPackageData data);
        public void Run();
    }
}
