using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QsFarmer
{
    public interface QsITaskProcessor
    {
        public void Run(QsPackageHeader header, QsPackageData data);
    }
}
