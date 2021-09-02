using DesktopAssistant.API_weather_response;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DesktopAssistant
{
    /// <summary>
    /// Interaction logic for DateTimeWindow.xaml
    /// </summary>
    public partial class DateTimeWindow : Window
    {
        public DateTimeWindow()
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
                label_Time.Content = DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
            };


            label_Date.Content = DateTime.Now.ToString("dd.MM.yy", CultureInfo.InvariantCulture);
            label_DayOfWeek.Content = DateTime.Now.DayOfWeek.ToString();

            // === День недели на РУССКОМ ===
            //DateTime dateTime = DateTime.Now;
            //label_DayOfWeek.Content = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.GetDayName(dateTime.DayOfWeek);

            try
            {
                WeatherDataHandler.GetApiJsonData();

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
                    label_maxTempToday.Content = $"Max temp - {WeatherDataHandler.temp_max} °C";
                    label_minTempToday.Content = $"Min temp - {WeatherDataHandler.temp_min} °C";
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
    }
}
