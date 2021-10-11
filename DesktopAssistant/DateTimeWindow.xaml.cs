using DesktopAssistant.API_weather_response;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DesktopAssistant
{
	/// <summary>
	/// Interaction logic for DateTimeWindow.xaml
	/// </summary>
	public partial class DateTimeWindow : Window
	{
		// Флаг для проверки, свернуто ли основное окно (от этого зависит, будет ли оно при нажатии
		// на это окошко разворачиваться)
		public bool isMainFormHidden = false;

		public DateTimeWindow()
		{
			CreateDateTimeWindow();

			ShowInTaskbar = false;

			// Этот код убирает окно из таскбара alt-tab
			Window helperWindow = new Window(); // Create helper window
			helperWindow.Top = -100; // Location of new window is outside of visible part of screen
			helperWindow.Left = -100;
			helperWindow.Width = 1; // size of window is enough small to avoid its appearance at the beginning
			helperWindow.Height = 1;
			helperWindow.WindowStyle = WindowStyle.ToolWindow; // Set window style as ToolWindow to avoid its icon in AltTab 
			helperWindow.ShowInTaskbar = false;
			helperWindow.Show(); // We need to show window before set is as owner to our main window
			this.Owner = helperWindow; // Okey, this will result to disappear icon for main window.
			helperWindow.Hide(); // Hide helper window just in case
		}

		public async Task CreateDateTimeWindow()
		{
			InitializeComponent();

			var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
			this.Left = desktopWorkingArea.Right - this.Width - 5;
			this.Top = desktopWorkingArea.Top + 5;

			var secondsTimer = new System.Windows.Threading.DispatcherTimer
			{
				Interval = new TimeSpan(0, 0, 1)
			};
			secondsTimer.Start();
			secondsTimer.Tick += (o, t) =>
			{
				//label_Time_Hrs_Mins.Content = DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
				label_Time_Hrs_Mins.Content = DateTime.Now.ToString("HH:mm", CultureInfo.InvariantCulture);
				label_Time_Secs.Content = DateTime.Now.ToString("ss", CultureInfo.InvariantCulture);
			};

			label_Date.Content = DateTime.Now.ToString("dd.MM.yy", CultureInfo.InvariantCulture);
			label_DayOfWeek.Content = DateTime.Now.DayOfWeek.ToString();

			// === День недели на РУССКОМ ===
			//DateTime dateTime = DateTime.Now;
			//label_DayOfWeek.Content = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(dateTime.DayOfWeek);

			try
			{
				await WeatherDataHandler.GetApiJsonData();

				var oneHourTimer = new System.Windows.Threading.DispatcherTimer
				{
					Interval = new TimeSpan(1, 0, 0)
				};
				oneHourTimer.Tick += (o, t) =>
				{
					WeatherDataHandler.GetApiJsonData();
				};
				oneHourTimer.Start();

				// Если запрос API сработал без exeption, однако не вернул данные - отдельная проверка
				if (WeatherDataHandler.temp_max != 0 && WeatherDataHandler.temp_min != 0)
				{
					label_maxTempToday.Content = $"Max temp : {WeatherDataHandler.temp_max} °C";
					label_minTempToday.Content = $"Min temp : {WeatherDataHandler.temp_min} °C";
				}
				else
				{
					label_maxTempToday.Content = "*no weather data*";
					label_minTempToday.Content = $"";
				}
			}
			catch (Exception)
			{
				label_maxTempToday.Content = "*error loading data*";
				label_minTempToday.Content = $"";
			}
		}

		// При нажатии кнопки на окошко, если главное окно было свернуто, оно разворачивается +
		// снова отображается в таскбаре alt-tab
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (isMainFormHidden)
			{
				var mainWindow = Application.Current.MainWindow;
				mainWindow.WindowState = WindowState.Normal;
				mainWindow.ShowInTaskbar = true;

				Mouse.OverrideCursor = Cursors.Arrow;

				isMainFormHidden = false;
			}
		}
	}
}
