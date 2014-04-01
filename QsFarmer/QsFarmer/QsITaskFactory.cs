using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QsFarmer
{
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

    public interface QsITaskFactory
    {
        public QsITaskProcessor CreateTaskProcessor();
    }

	public class QsTaskFactory
	{
		private static QsITaskFactory[] processorFactoryArray = new QsITaskFactory[(int)QsTaskType.QsETaskBoundary]{null, new QsFarmerRegisterTaskProcessorFactory(), null, null, null, null, null,};
		//public static List<QsITaskProcessorFactory> processorFactoryList = new List<QsITaskProcessorFactory>(new QsITaskProcessorFactory[]{QsFarmerRegisterTaskProcessor});
		public static QsITaskFactory CreateFactory(string taskType)
		{
			Type t = Type.GetType(taskType);
			return Activator.CreateInstance(t) as QsITaskFactory;
		}
		
		public static QsITaskFactory CreateFactory(QsTaskType taskType)
		{
			if (taskType >= QsTaskType.QsETaskBoundary)
				return null;
			return processorFactoryArray[(int)taskType].CreateTaskProcessor() as QsITaskFactory;
		}
	}
}
