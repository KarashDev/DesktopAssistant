using System;
using System.Collections.Generic;
using System.Text;

namespace DesktopAssistant
{
	/// <summary>
	/// Логическая ошибка работы с Api; возникает, если с Api пришел ответ, но неправильный
	/// </summary>
	class QueueExeption : Exception
	{
		/// <summary>
		/// Логическая ошибка работы с Api; возникает, если с Api пришел ответ, но неправильный
		/// </summary>
		public QueueExeption() 
		{ 
		}
	}
}
