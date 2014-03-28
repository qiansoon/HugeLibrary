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
    }
}
