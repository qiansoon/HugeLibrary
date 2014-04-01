using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QsFarmer
{
<<<<<<< HEAD
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
=======
	public enum QsTaskType
	{
		QsETaskFarmerRegister = 0x1,
		QsETaskLanderResponseRegister,
		QsETaskLanderAssignCompareTask,
		QsETaskFarmerReportResult,
		QsETaskLanderAllocFarmland,
		QsETaskLanderAddNewFeature,
		QsETaskBoundary
	}

	public class QsTaskFactory
	{
		private static QsITaskProcessorFactory processorFactoryArray = new QsITaskProcessorFactory[QsTaskType.QsETaskBoundary]{null, QsFarmerRegisterTaskProcessor};
		//public static List<QsITaskProcessorFactory> processorFactoryList = new List<QsITaskProcessorFactory>(new QsITaskProcessorFactory[]{QsFarmerRegisterTaskProcessor});
		public static QsITaskProcessorFactory CreateFactory(string taskType)
		{
			Type t = Type.GetType(taskType);
			return Activator.CreateInstance(t) as QsITaskProcessorFactory;
		}
		
		public static QsITaskProcessorFactory CreateFactory(QsTaskType taskType)
		{
			if (taskType >= QsTaskType.QsETaskBoundary)
				return null;
			return processorFactoryArray[taskType].CreateTaskProcessor() as QsITaskProcessorFactory;
		}
	}

    public interface QsITaskProcessorFactory
    {
		public QsITaskProcessor CreateTaskProcessor();
>>>>>>> 76898a4384d29573238729f5ee0ecc12f97e6246
    }
}
