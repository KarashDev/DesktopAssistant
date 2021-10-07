using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Drawing;
using System.Globalization;

namespace DesktopAssistant
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private DateTimeWindow dateTimeWindow;

		bool isTextEncrypted = false;

		// Таймер для срабатывания отслеживания количества элементов на рабочем столе
		DispatcherTimer checkDesktopNumberTimer = new System.Windows.Threading.DispatcherTimer
		{
			Interval = new TimeSpan(0, 30, 0)
		};

		// Таймер для ежесекундной проверки совпадения времени уведомления с текущим временем
		DispatcherTimer checkTaskListNotificationsTimer = new System.Windows.Threading.DispatcherTimer
		{
			Interval = new TimeSpan(0, 0, 1)
		};

		public MainWindow()
		{
			InitializeComponent();

			// Скрываем блок с настройками уведомления для списка задач
			HideNotificationSettings();

			// Заполнение combobox
			for (int i = 1; i <= 40; i++)
			{
				combobox_DesktopNumber.Items.Add(i);
			}
			combobox_DesktopNumber.SelectedItem = 20;

			// Заполнение полей значениями, ранее сохраненными в файл настроек
			textBox_Clipboard_1.Text = Properties.Settings.Default.Clipboard_1.ToString();
			textBox_Clipboard_2.Text = Properties.Settings.Default.Clipboard_2.ToString();
			textBox_Clipboard_3.Text = Properties.Settings.Default.Clipboard_3.ToString();
			textBox_Clipboard_4.Text = Properties.Settings.Default.Clipboard_4.ToString();
			textBox_Clipboard_5.Text = Properties.Settings.Default.Clipboard_5.ToString();

			textBox_Tasklist_1.Text = Properties.Settings.Default.Tasklist_1.ToString();
			textBox_Tasklist_2.Text = Properties.Settings.Default.Tasklist_2.ToString();
			textBox_Tasklist_3.Text = Properties.Settings.Default.Tasklist_3.ToString();
			textBox_Tasklist_4.Text = Properties.Settings.Default.Tasklist_4.ToString();
			textBox_Tasklist_5.Text = Properties.Settings.Default.Tasklist_5.ToString();

			// Проверка, включен или выключен текстбокс (выполнена/не выполнена задача) с файла настроек,
			// чтобы восстановить окно в том же виде, в каком оно было до перезапуска программы
			if (Properties.Settings.Default.Tasklist_1_isEnabled)
				textBox_Tasklist_1.IsEnabled = true;
			else
			{
				textBox_Tasklist_1.IsEnabled = false;
				textBox_Tasklist_1.Foreground = Brushes.LightPink;
			}

			if (Properties.Settings.Default.Tasklist_2_isEnabled)
				textBox_Tasklist_2.IsEnabled = true;
			else
			{
				textBox_Tasklist_2.IsEnabled = false;
				textBox_Tasklist_2.Foreground = Brushes.LightPink;
			}

			if (Properties.Settings.Default.Tasklist_3_isEnabled)
				textBox_Tasklist_3.IsEnabled = true;
			else
			{
				textBox_Tasklist_3.IsEnabled = false;
				textBox_Tasklist_3.Foreground = Brushes.LightPink;
			}

			if (Properties.Settings.Default.Tasklist_4_isEnabled)
				textBox_Tasklist_4.IsEnabled = true;
			else
			{
				textBox_Tasklist_4.IsEnabled = false;
				textBox_Tasklist_4.Foreground = Brushes.LightPink;
			}

			if (Properties.Settings.Default.Tasklist_5_isEnabled)
				textBox_Tasklist_5.IsEnabled = true;
			else
			{
				textBox_Tasklist_5.IsEnabled = false;
				textBox_Tasklist_5.Foreground = Brushes.LightPink;
			}

			this.Closed += (s, e) => Application.Current.Shutdown();

			if (!isTextEncrypted)
			{
				this.Closed += (s, e) => Properties.Settings.Default.Save();
			}

			// Выгрузка списка установленных уведомлений из Properties 
			// + если список уведомлений не пустой - сразу запускаем тикать проверяющий таймер
			if (Properties.Settings.Default.ListOfNotifications != null)
			{
				if (Properties.Settings.Default.ListOfNotifications.Any())
				{
					img_activeNotifications.Visibility = Visibility.Visible;
					checkTaskListNotificationsTimer.Start();
				}
			}


			// Таймер, сопоставляющий текущее время с сохраненными данными об уведомлениях
			checkTaskListNotificationsTimer.Tick += (o, t) =>
			{
				var queryMatch = Properties.Settings.Default.ListOfNotifications.FirstOrDefault(n =>
				n.dateTime.ToString("HH") == DateTime.Now.ToString("HH") && n.dateTime.ToString("mm") == DateTime.Now.ToString("mm"));

				if (queryMatch != null)
				{
					NotificationWindow notificationWindow = new NotificationWindow();
					notificationWindow.Show();

					// Когда уведомление отработало - удаляем его из списка уведомлений
					Properties.Settings.Default.ListOfNotifications.Remove(queryMatch);

					if (!Properties.Settings.Default.ListOfNotifications.Any())
					{
						img_activeNotifications.Visibility = Visibility.Hidden;
					}
				}
			};
		}


		// Методы скрытия/показа блока с настройками уведомления для списка задач. Сразу же
		// сохраняет в отдельный лейбл номер кнопки, которая вызвала этот метод
		public void ShowNotificationSettings(int buttonSenderNumber)
		{
			this.Width = 800;
			grid_taksList_NotifyElements.Visibility = Visibility.Visible;

			label_WhichButtonOpenedNotifEditor.Content = buttonSenderNumber;
		}

		public void HideNotificationSettings()
		{
			this.Width = 600;
			grid_taksList_NotifyElements.Visibility = Visibility.Hidden;
		}

		// Если чекбокс активирован - показывается окно с временем и датой
		private void checkBox_ShowDateTimeWindow_Checked(object sender, RoutedEventArgs e)
		{
			if (checkBox_ShowDateTimeWindow.IsChecked == true)
			{
				// Окно с показом погоды создается только 1 раз при первом нажатии чекбокса
				if (dateTimeWindow == null)
				{
					dateTimeWindow = new DateTimeWindow();
				}

				dateTimeWindow.Show();
			}
		}

		private void checkBox_ShowDateTimeWindow_UnChecked(object sender, RoutedEventArgs e)
		{
			if (checkBox_ShowDateTimeWindow.IsChecked == false)
			{
				dateTimeWindow.Hide();
			}
		}

		// Сохранение введенного текста в файл настроек, чтобы он сохранился даже при перезапуске программы
		private void textBox_Clipboard_TextChanged(object sender, TextChangedEventArgs e)
		{
			Control control = (Control)sender;

			switch (control.Name)
			{
				case "textBox_Clipboard_1":
					if (!isTextEncrypted)
						Properties.Settings.Default.Clipboard_1 = textBox_Clipboard_1.Text;
					break;
				case "textBox_Clipboard_2":
					if (!isTextEncrypted)
						Properties.Settings.Default.Clipboard_2 = textBox_Clipboard_2.Text;
					break;
				case "textBox_Clipboard_3":
					if (!isTextEncrypted)
						Properties.Settings.Default.Clipboard_3 = textBox_Clipboard_3.Text;
					break;
				case "textBox_Clipboard_4":
					if (!isTextEncrypted)
						Properties.Settings.Default.Clipboard_4 = textBox_Clipboard_4.Text;
					break;
				case "textBox_Clipboard_5":
					if (!isTextEncrypted)
						Properties.Settings.Default.Clipboard_5 = textBox_Clipboard_5.Text;
					break;

				default:
					break;
			}
		}

		// Кнопка сохранения введеного текста в буфер обмена
		private void SaveTextToClipBoard(string text)
		{
			Clipboard.Clear();
			Clipboard.SetText(text);
		}

		private void button_Clipboard_Click(object sender, RoutedEventArgs e)
		{
			Control control = (Control)sender;

			switch (control.Name)
			{
				case "button_Clipboard_1":
					SaveTextToClipBoard(textBox_Clipboard_1.Text);
					break;
				case "button_Clipboard_2":
					SaveTextToClipBoard(textBox_Clipboard_2.Text);
					break;
				case "button_Clipboard_3":
					SaveTextToClipBoard(textBox_Clipboard_3.Text);
					break;
				case "button_Clipboard_4":
					SaveTextToClipBoard(textBox_Clipboard_4.Text);
					break;
				case "button_Clipboard_5":
					SaveTextToClipBoard(textBox_Clipboard_5.Text);
					break;

				default:
					break;
			}
		}

		// При активации весь текст в строках шифруется
		private void checkBox_Encrypt_Checked(object sender, RoutedEventArgs e)
		{
			if (checkBox_Encrypt.IsChecked == true && isTextEncrypted == false)
			{
				//шифрование 
				isTextEncrypted = true;

				var clipboardString = new string[] { textBox_Clipboard_1.Text, textBox_Clipboard_2.Text, textBox_Clipboard_3.Text ,
				textBox_Clipboard_4.Text,textBox_Clipboard_5.Text};

				string[] encryptedStrings = Cryptographer.Crypt(clipboardString);
				textBox_Clipboard_1.Text = encryptedStrings[0];
				textBox_Clipboard_2.Text = encryptedStrings[1];
				textBox_Clipboard_3.Text = encryptedStrings[2];
				textBox_Clipboard_4.Text = encryptedStrings[3];
				textBox_Clipboard_5.Text = encryptedStrings[4];

				var textBoxes = new[] { textBox_Clipboard_1, textBox_Clipboard_2, textBox_Clipboard_3 ,
				textBox_Clipboard_4,textBox_Clipboard_5};

				foreach (var textBox in textBoxes)
				{
					textBox.IsEnabled = false;
				}
			}
		}

		private void checkBox_Encrypt_Unchecked(object sender, RoutedEventArgs e)
		{
			if (checkBox_Encrypt.IsChecked == false)
			{
				//новое окно - запрос пароля
				PasswordCheckWindow passwordCheckWindow = new PasswordCheckWindow();
				passwordCheckWindow.Show();

				// Когда в окне проверки пароля нажали кнопку "submit"
				passwordCheckWindow.button_submitPassword_Clicked += (s, e) =>
				{
					// Дешифруем строки только если правильно введен пароль в окне проверки пароля
					if (passwordCheckWindow.textbox_Password.Text == "13")
					{
						passwordCheckWindow.Close();

						// Дешифровка
						isTextEncrypted = false;

						var clipboardString = new string[] { textBox_Clipboard_1.Text, textBox_Clipboard_2.Text, textBox_Clipboard_3.Text ,
				textBox_Clipboard_4.Text,textBox_Clipboard_5.Text};
						string[] decryptedStrings = Cryptographer.Decrypt(clipboardString);
						textBox_Clipboard_1.Text = decryptedStrings[0];
						textBox_Clipboard_2.Text = decryptedStrings[1];
						textBox_Clipboard_3.Text = decryptedStrings[2];
						textBox_Clipboard_4.Text = decryptedStrings[3];
						textBox_Clipboard_5.Text = decryptedStrings[4];


						var textBoxes = new[] { textBox_Clipboard_1, textBox_Clipboard_2, textBox_Clipboard_3 ,
				textBox_Clipboard_4,textBox_Clipboard_5};

						foreach (var textBox in textBoxes)
						{
							textBox.IsEnabled = true;
						}

						// Если текст висит незашифрованным дольше 10 секунд - он автоматически шифруется обратно
						var twentySecondsTimer = new System.Windows.Threading.DispatcherTimer
						{
							Interval = new TimeSpan(0, 0, 10)
						};
						twentySecondsTimer.Tick += (o, t) =>
						{
							checkBox_Encrypt.IsChecked = true;
							twentySecondsTimer.Stop();
						};
						twentySecondsTimer.Start();
					}
					else
					{
						passwordCheckWindow.Close();
						MessageBox.Show("Отказано, неверный пароль");
						checkBox_Encrypt.IsChecked = true;
					}
				};

			}
		}


		private void textBox_Tasklist_TextChanged(object sender, TextChangedEventArgs e)
		{
			Control control = (Control)sender;

			switch (control.Name)
			{
				case "textBox_Tasklist_1":
					Properties.Settings.Default.Tasklist_1 = textBox_Tasklist_1.Text;
					break;
				case "textBox_Tasklist_2":
					Properties.Settings.Default.Tasklist_2 = textBox_Tasklist_2.Text;
					break;
				case "textBox_Tasklist_3":
					Properties.Settings.Default.Tasklist_3 = textBox_Tasklist_3.Text;
					break;
				case "textBox_Tasklist_4":
					Properties.Settings.Default.Tasklist_4 = textBox_Tasklist_4.Text;
					break;
				case "textBox_Tasklist_5":
					Properties.Settings.Default.Tasklist_5 = textBox_Tasklist_5.Text;
					break;

				default:
					break;
			};
		}

		private void button_SaveTasksInHistory_Click(object sender, RoutedEventArgs e)
		{
			void LogTextToFile(TextBox textBox)
			{
				string fileName = "DesktopAssistant_TaskHistory.txt";

				string textFromTextbox = String.IsNullOrEmpty(textBox.Text) ? "*пусто*" : textBox.Text;

				string textToWrite = $"\n\" {DateTime.Now.ToString()} - ПОЛЕ {textBox.Name} - {textFromTextbox}\n\"";
				using (StreamWriter writer = new StreamWriter(fileName, true))
				{
					writer.Write(textToWrite);
				}
			}

			if (String.IsNullOrEmpty(textBox_Tasklist_1.Text) && String.IsNullOrEmpty(textBox_Tasklist_2.Text) &&
				String.IsNullOrEmpty(textBox_Tasklist_3.Text) && String.IsNullOrEmpty(textBox_Tasklist_4.Text) &&
				String.IsNullOrEmpty(textBox_Tasklist_5.Text))
			{
				MessageBox.Show("Поля пусты, нечего сохранять");
			}
			else
			{
				LogTextToFile(textBox_Tasklist_1);
				LogTextToFile(textBox_Tasklist_2);
				LogTextToFile(textBox_Tasklist_3);
				LogTextToFile(textBox_Tasklist_4);
				LogTextToFile(textBox_Tasklist_5);
			}

		}

		void EnableTextbox(TextBox textBox)
		{
			textBox.IsEnabled = true;
			textBox.Foreground = Brushes.Black;
		}

		void DisableTextbox(TextBox textBox)
		{
			textBox.IsEnabled = false;
			textBox.Foreground = Brushes.LightPink;
		}

		private void button_Tasklist_Done_Click(object sender, RoutedEventArgs e)
		{
			Control control = (Control)sender;
			switch (control.Name)
			{
				case "button_Tasklist_1_Done":
					if (textBox_Tasklist_1.IsEnabled)
					{
						DisableTextbox(textBox_Tasklist_1);
						Properties.Settings.Default.Tasklist_1_isEnabled = false;
						break;
					}
					else
					{
						EnableTextbox(textBox_Tasklist_1);
						Properties.Settings.Default.Tasklist_1_isEnabled = true;
						break;
					}
				case "button_Tasklist_2_Done":
					if (textBox_Tasklist_2.IsEnabled)
					{
						DisableTextbox(textBox_Tasklist_2);
						Properties.Settings.Default.Tasklist_2_isEnabled = false;
						break;
					}
					else
					{
						EnableTextbox(textBox_Tasklist_2);
						Properties.Settings.Default.Tasklist_2_isEnabled = true;
						break;
					}
				case "button_Tasklist_3_Done":
					if (textBox_Tasklist_3.IsEnabled)
					{
						DisableTextbox(textBox_Tasklist_3);
						Properties.Settings.Default.Tasklist_3_isEnabled = false;
						break;
					}
					else
					{
						EnableTextbox(textBox_Tasklist_3);
						Properties.Settings.Default.Tasklist_3_isEnabled = true;
						break;
					}
				case "button_Tasklist_4_Done":
					if (textBox_Tasklist_4.IsEnabled)
					{
						DisableTextbox(textBox_Tasklist_4);
						Properties.Settings.Default.Tasklist_4_isEnabled = false;
						break;
					}
					else
					{
						EnableTextbox(textBox_Tasklist_4);
						Properties.Settings.Default.Tasklist_4_isEnabled = true;
						break;
					}
				case "button_Tasklist_5_Done":
					if (textBox_Tasklist_5.IsEnabled)
					{
						DisableTextbox(textBox_Tasklist_5);
						Properties.Settings.Default.Tasklist_5_isEnabled = false;
						break;
					}
					else
					{
						EnableTextbox(textBox_Tasklist_5);
						Properties.Settings.Default.Tasklist_5_isEnabled = true;
						break;
					}

				default:
					break;
			}

		}

		private void button_Tasklist_Clear_Click(object sender, RoutedEventArgs e)
		{
			Control control = (Control)sender;

			switch (control.Name)
			{
				case "button_Tasklist_1_Clear":
					textBox_Tasklist_1.Clear();
					if (!textBox_Tasklist_1.IsEnabled)
					{
						EnableTextbox(textBox_Tasklist_1);
						Properties.Settings.Default.Tasklist_1_isEnabled = true;
					}
					break;
				case "button_Tasklist_2_Clear":
					textBox_Tasklist_2.Clear();
					if (!textBox_Tasklist_2.IsEnabled)
					{
						EnableTextbox(textBox_Tasklist_2);
						Properties.Settings.Default.Tasklist_2_isEnabled = true;
					}
					break;
				case "button_Tasklist_3_Clear":
					textBox_Tasklist_3.Clear();
					if (!textBox_Tasklist_3.IsEnabled)
					{
						EnableTextbox(textBox_Tasklist_3);
						Properties.Settings.Default.Tasklist_3_isEnabled = true;
					}
					break;
				case "button_Tasklist_4_Clear":
					textBox_Tasklist_4.Clear();
					if (!textBox_Tasklist_4.IsEnabled)
					{
						EnableTextbox(textBox_Tasklist_4);
						Properties.Settings.Default.Tasklist_4_isEnabled = true;
					}
					break;
				case "button_Tasklist_5_Clear":
					textBox_Tasklist_5.Clear();
					if (!textBox_Tasklist_5.IsEnabled)
					{
						EnableTextbox(textBox_Tasklist_5);
						Properties.Settings.Default.Tasklist_5_isEnabled = true;
					}
					break;

				default:
					break;
			}

		}

		// Чекбокс мониторинга количества элементов на рабочем столе
		private void checkBox_DekstopElNumberTracking_Checked(object sender, RoutedEventArgs e)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

			if (checkBox_DekstopElNumberTracking.IsChecked == true)
			{
				checkDesktopNumberTimer.Start();
				checkDesktopNumberTimer.Tick += (o, t) =>
				{
					int fileCountOnDesktop = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).Length;
					label_Desktop_NumberOfFiles.Content = fileCountOnDesktop;


					if (fileCountOnDesktop >= (int)combobox_DesktopNumber.SelectedItem)
					{
						MessageBox.Show($"На рабочем столе слишком много объектов",
						"Рабочий стол забит", MessageBoxButton.OK, MessageBoxImage.Warning);
					}
				};
			}
		}

		private void checkBox_DekstopElNumberTracking_Unchecked(object sender, RoutedEventArgs e)
		{
			checkDesktopNumberTimer.Stop();
		}

		private void button_Tasklist_Notify_Click(object sender, RoutedEventArgs e)
		{
			Control control = (Control)sender;

			this.Width = 1000;

			switch (control.Name)
			{
				case "button_Tasklist_1_Notify":
					ShowNotificationSettings(1);
					break;
				case "button_Tasklist_2_Notify":
					ShowNotificationSettings(2);
					break;
				case "button_Tasklist_3_Notify":
					ShowNotificationSettings(3);
					break;
				case "button_Tasklist_4_Notify":
					ShowNotificationSettings(4);
					break;
				case "button_Tasklist_5_Notify":
					ShowNotificationSettings(5);
					break;
				default:
					break;
			};
		}

		// Активации уведомления связанного с задачей
		private void button_Tasklist_Notif_Act_Click(object sender, RoutedEventArgs e)
		{
			var selectedDate = datePicker_Notify.SelectedDate;
			var enteredHours = textBox_Notify_Hrs.Text;
			var enteredMinutes = textBox_Notify_Mins.Text;
			var pickedDate = (DateTime)selectedDate;

			var date = pickedDate.ToString("dd.MM.yy");
			var time = $"{enteredHours}:{enteredMinutes}:00";

			var dateTimeToSave = DateTime.ParseExact($"{date} {time}", "dd.MM.yy HH:mm:ss", CultureInfo.InvariantCulture);

			// Проверка с какой именно кнопки произошел вызов окна с настройками уведомления
			int whichButtonOpenedNotifEditor = 0;

			switch (label_WhichButtonOpenedNotifEditor.Content)
			{
				case 1:
					whichButtonOpenedNotifEditor = 1;
					break;
				case 2:
					whichButtonOpenedNotifEditor = 2;
					break;
				case 3:
					whichButtonOpenedNotifEditor = 3;
					break;
				case 4:
					whichButtonOpenedNotifEditor = 4;
					break;
				case 5:
					whichButtonOpenedNotifEditor = 5;
					break;
				default:
					break;
			};


			try
			{
				// Создание уведомления и добавление его в список в файле настроек
				var newNotification = new TaskListNotification(dateTimeToSave, whichButtonOpenedNotifEditor);
				//var newNotification = new TaskListNotification() { DateTime = dateTimeToSave,  TaksListTextboxNumber = whichButtonOpenedNotifEditor };

				if (Properties.Settings.Default.ListOfNotifications == null)
				{
					Properties.Settings.Default.ListOfNotifications = new List<TaskListNotification>();
					Properties.Settings.Default.ListOfNotifications.Add(newNotification);
					img_activeNotifications.Visibility = Visibility.Visible;

					checkTaskListNotificationsTimer.Start();
				}
				else
				{
					Properties.Settings.Default.ListOfNotifications.Add(newNotification);
					img_activeNotifications.Visibility = Visibility.Visible;
				}
			}
			catch (Exception)
			{
				MessageBox.Show($"Ошибка сохранения уведомления",
						"ОШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			HideNotificationSettings();
		}

		private void button_Tasklist_Notif_Cancel_Click(object sender, RoutedEventArgs e)
		{
			HideNotificationSettings();
		}
	}
}
