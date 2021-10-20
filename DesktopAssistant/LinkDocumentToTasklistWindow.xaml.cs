using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesktopAssistant
{
	/// <summary>
	/// Interaction logic for LinkDocumentToTasklistWindow.xaml
	/// </summary>
	public partial class LinkDocumentToTasklistWindow : Window
	{
		// Сопоставление директории файла с кнопкой по номеру таскбара
		// Сериализованный в строку filesByTaskbars
		string filesByTaskbars_serialized;

		// Сериализованный в строку filesByTaskbars
		Dictionary<int, string> filesByTaskbars_deserialized;

		string chosenFileDirectory;

		int taskListNumber;

		public LinkDocumentToTasklistWindow(int taskListNumberFromMainform)
		{
			InitializeComponent();

			filesByTaskbars_serialized = Properties.Settings.Default.FilesByTaskbars_InString;

			// TODO перестал выводиться номер тасклиста
			// TODO не работает сохранение нового привязанного документа в Properties

			if (!String.IsNullOrEmpty(filesByTaskbars_serialized))
			{
				filesByTaskbars_deserialized = JsonConvert.DeserializeObject<Dictionary<int, string>>(filesByTaskbars_serialized);

				taskListNumber = taskListNumberFromMainform;

				for (int i = 1; i <= 5; i++)
				{
					if (taskListNumber == i)
					{
						if (filesByTaskbars_deserialized.ContainsKey(i))
							chosenFileDirectory = filesByTaskbars_deserialized[i];
						else chosenFileDirectory = "*Нет документа*";
					}
				}

				//switch (taskListNumber)
				//{
				//	case 1:
				//		chosenFileDirectory = filesByTaskbars_deserialized[1];
				//		break;
				//	case 2:
				//		chosenFileDirectory = filesByTaskbars_deserialized[2];
				//		break;
				//	case 3:
				//		chosenFileDirectory = filesByTaskbars_deserialized[3];
				//		break;
				//	case 4:
				//		chosenFileDirectory = filesByTaskbars_deserialized[4];
				//		break;
				//	case 5:
				//		chosenFileDirectory = filesByTaskbars_deserialized[5];
				//		break;
				//	default:
				//		break;
				//};

				label_LinkedDocName.Content = chosenFileDirectory;
			}
			else filesByTaskbars_deserialized = new Dictionary<int, string>();

		}

		private void button_LinkDoc_Click(object sender, RoutedEventArgs e)
		{
			// Привязка и сохранение текстового файла к задаче (к таскбару по его номеру)
			Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.DefaultExt = ".docx";
			dialog.Filter = @"Word File (.docx ,.doc)|*.docx;*.doc|Text File (.txt)|*.txt";

			bool? isFileChoosed = dialog.ShowDialog();

			if (isFileChoosed == true)
			{
				chosenFileDirectory = dialog.FileName;
				label_LinkedDocName.Content = chosenFileDirectory;

				// Если уже есть привязанный документ - удаляем его перед привязкой нового
				if (filesByTaskbars_deserialized.ContainsKey(Convert.ToInt32(label_taskListNumber.Content)))
				{
					filesByTaskbars_deserialized.Remove(Convert.ToInt32(label_taskListNumber.Content));
				}

				filesByTaskbars_deserialized.Add(Convert.ToInt32(label_taskListNumber.Content), Convert.ToString(chosenFileDirectory));
			}

		}

		private void button_OpenDoc_Click(object sender, RoutedEventArgs e)
		{
			if (File.Exists(chosenFileDirectory))
			{
				FileStream x = File.Open(chosenFileDirectory, FileMode.Open, FileAccess.ReadWrite);
			}
			else MessageBox.Show($"Файл не существует в указанной директории",
						"Директория неверна", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void TasklistDocumentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Сериализация словаря в строку для хранения в Properties в виде строки
			Properties.Settings.Default.FilesByTaskbars_InString = JsonConvert.SerializeObject(filesByTaskbars_deserialized);
		}

		// Окно нельзя закрывать, поэтому установлено сокрытие на кастомную кнопку
		private void button_HideWindow_Click(object sender, RoutedEventArgs e)
		{
			this.Hide();
		}
	}
}
