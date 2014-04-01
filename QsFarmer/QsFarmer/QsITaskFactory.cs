using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QsFarmer
{
    public class QsTaskFactory
    {
        private static HashSet<QsITaskFactory> taskFactorySet = new HashSet<QsITaskFactory>
        public static string GetTaskFactory(int type)
        {
            return taskFactorySet.ElementAt(type);
        }
        public static QsITaskFactory CreateFactory(string factory)
        {
            Type t = Type.GetType("QsFarmer." + factory.Trim());
            return Activator.CreateInstance(t) as QsITaskFactory;
        }
    }

    public interface QsITaskFactory
    {
        public QsITaskProcessor CreateTaskProcessor();
    }
}
