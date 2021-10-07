using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopAssistant
{
	//// Реализация через интерфейс (защита данных хуже, но код более читальный)
	//public interface Notification
	//{
	//	DateTime DateTime { get; set; }
	//}

	//public sealed class TaskListNotification : Notification
	//{
	//	private DateTime dateTime;

	//	public DateTime DateTime
	//	{
	//		get { return dateTime; }
	//		set
	//		{
	//			if (dateTime == null) { dateTime = value; }
	//			else throw new Exception("Попытка заменить первоначальное значение даты в уведомлении");
	//		}
	//	}

	//	public int TaksListTextboxNumber
	//	{
	//		get => TaksListTextboxNumber;
	//		set
	//		{
	//			//if (TaksListTextboxNumber == default)
	//				TaksListTextboxNumber = value;
	//		}
	//	}
	//}


	// Реализация через абстрактный класс (защита данных лучше, но код менее читабельный)
	public abstract class Notification
	{
		public readonly DateTime dateTime;

		public Notification(DateTime dateTime)
		{
			this.dateTime = dateTime;
		}
	}

	public sealed class TaskListNotification : Notification
	{
		public readonly int TaksListTextboxNumber;

		public TaskListNotification(DateTime dateTime, int taskListTxtBoxNum) : base(dateTime)
		{
			TaksListTextboxNumber = taskListTxtBoxNum;
		}
	}
}
