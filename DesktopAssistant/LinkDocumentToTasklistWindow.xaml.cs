using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
		string chosenFileDirectory;

		public LinkDocumentToTasklistWindow()
		{
			InitializeComponent();
		}

		private void button_LinkDoc_Click(object sender, RoutedEventArgs e)
		{
			// 1 Выбор файла. Хранится строка его директории

			Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.DefaultExt = ".docx";
			dialog.Filter = @"Word File (.docx ,.doc)|*.docx;*.doc|Text File (.txt)|*.txt";

			bool? isFileChoosed = dialog.ShowDialog();

			if (isFileChoosed == true)
			{
				chosenFileDirectory = dialog.FileName;
				label_LinkedDocName.Content = chosenFileDirectory;
			}

			// 2 Сопоставление директории файла с кнопкой по номеру таскбара, привязка через Dictionary
			Dictionary<string, int> filesByTaskbars = new Dictionary<string, int>(5);

			// 3 Сохранение словаря с сопоставленными директориями и номерами тасков в Properties
			
			// другие типы
			//Filter = @"All Files|*.txt;*.docx;*.doc;*.pdf*.xls;*.xlsx;*.pptx;*.ppt|Text File (.txt)|*.txt|Word File (.docx ,.doc)|*.docx;*.doc|PDF (.pdf)|*.pdf|Spreadsheet (.xls ,.xlsx)|  *.xls ;*.xlsx|Presentation (.pptx ,.ppt)|*.pptx;*.ppt";
			//Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
		}

		private void button_OpenDoc_Click(object sender, RoutedEventArgs e)
		{
			// Открытие сохраненного документа, с проверкой на номер тасклиста

			if (File.Exists(chosenFileDirectory))
			{
				// не работает
				//FileStream x = File.Open(chosenFileDirectory, FileMode.Open, FileAccess.ReadWrite);
			}
			else MessageBox.Show($"Файл не существует в указанной директории",
						"Директория неверна", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
