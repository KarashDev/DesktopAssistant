using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopAssistant
{
	public abstract class Notification
	{
		DateTime dateTime;
	}
	
	public class TaskListNotification : Notification
	{
		public DateTime dateTime;
	}
}
