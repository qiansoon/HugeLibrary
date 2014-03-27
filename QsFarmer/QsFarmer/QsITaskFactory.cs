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
		public static 
		public static QsITaskProcessorFactory CreateFactory(string taskType)
		{
			Type t = Type.GetType(taskType);
			return Activator.CreateInstance(t) as QsITaskProcessorFactory;
		}
	}

    public interface QsITaskProcessorFactory
    {
		QsITaskProcessor CreateTaskProcessor();
    }
}
