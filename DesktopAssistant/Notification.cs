using System;

namespace DesktopAssistant
{
	// ПО НЕИЗВЕСТНОЙ ПРИЧИНЕ ПРИ ИСПОЛЬЗОВАНИИ ЛЮБЫХ НАСЛЕДОВАНИЙ НЕ СОХРАНЯЕТСЯ ФАЙЛ В PROPERTIES
	//// Реализация через интерфейс (защита данных хуже, но код более читальный)
	//public interface Notification
	//{
	//	DateTime DateTime { get; set; }
	//}

	//public sealed class TaskListNotification : Notification
	//{
	//	private DateTime dateTime;

	//	private int taksListTextboxNumber;

	//	public DateTime DateTime
	//	{
	//		get => dateTime;
	//		set
	//		{
	//			if (dateTime == default(DateTime)) { dateTime = value; }
	//			else throw new Exception("Попытка заменить первоначальное значение даты в уведомлении");
	//		}
	//	}

	//	public int TaksListTextboxNumber
	//	{
	//		get => TaksListTextboxNumber;
	//		set
	//		{
	//			if (TaksListTextboxNumber == default(int)) { taksListTextboxNumber = value; }
	//		}
	//	}
	//}


	//// Реализация через абстрактный класс (защита данных лучше, но код менее читабельный)
	//public abstract class Notification
	//{
	//	public readonly DateTime dateTime;

	//	public Notification(DateTime dateTime)
	//	{
	//		this.dateTime = dateTime;
	//	}
	//}

	//public sealed class TaskListNotification : Notification
	//{
	//	public readonly int TaksListTextboxNumber;

	//	public TaskListNotification(DateTime dateTime, int taskListTxtBoxNum) : base(dateTime)
	//	{
	//		TaksListTextboxNumber = taskListTxtBoxNum;
	//	}
	//}

	public class TaskListNotification
	{
		public DateTime dateTime;
		public int TaksListTextboxNumber;
	}
}
