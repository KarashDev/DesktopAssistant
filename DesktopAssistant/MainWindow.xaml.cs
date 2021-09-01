using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DesktopAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DateTimeWindow dateTimeWindow;

        public MainWindow()
        {
            InitializeComponent();
            dateTimeWindow = new DateTimeWindow();
            
            textBox_Clipboard_1.Text = Properties.Settings.Default.Clipboard_1.ToString();
            textBox_Clipboard_2.Text = Properties.Settings.Default.Clipboard_2.ToString();
            textBox_Clipboard_3.Text = Properties.Settings.Default.Clipboard_3.ToString();
            textBox_Clipboard_4.Text = Properties.Settings.Default.Clipboard_4.ToString();
            textBox_Clipboard_5.Text = Properties.Settings.Default.Clipboard_5.ToString();

            this.Closed += (s, e) => Application.Current.Shutdown();
            this.Closed += (s, e) => Properties.Settings.Default.Save();
        }

        private void checkBox_ShowDateTimeWindow_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox_ShowDateTimeWindow.IsChecked == true)
            {
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
        private void textBox_Clipboard_1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.Clipboard_1 = textBox_Clipboard_1.Text;
            //Properties.Settings.Default.Save();
        }
        private void textBox_Clipboard_2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.Clipboard_2 = textBox_Clipboard_2.Text;
        }
        private void textBox_Clipboard_3_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.Clipboard_3 = textBox_Clipboard_3.Text;
        }
        private void textBox_Clipboard_4_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.Clipboard_4 = textBox_Clipboard_4.Text;
        }
        private void textBox_Clipboard_5_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.Clipboard_5 = textBox_Clipboard_5.Text;
        }

        // Кнопка сохранения введеного текста в буфер обмена
        void SaveTextToClipBoard(string text)
        {
            Clipboard.Clear();
            Clipboard.SetText(text);
        }

        private void button_Clipboard_1_Click(object sender, RoutedEventArgs e)
        {
            SaveTextToClipBoard(textBox_Clipboard_1.Text);
        }

        private void button_Clipboard_2_Click(object sender, RoutedEventArgs e)
        {
            SaveTextToClipBoard(textBox_Clipboard_2.Text);
        }

        private void button_Clipboard_3_Click(object sender, RoutedEventArgs e)
        {
            SaveTextToClipBoard(textBox_Clipboard_3.Text);
        }

        private void button_Clipboard_4_Click(object sender, RoutedEventArgs e)
        {
            SaveTextToClipBoard(textBox_Clipboard_4.Text);
        }

        private void button_Clipboard_5_Click(object sender, RoutedEventArgs e)
        {
            SaveTextToClipBoard(textBox_Clipboard_5.Text);
        }

        // При активации весь текст в строках шифруется
        private void checkBox_Encrypt_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox_Encrypt.IsChecked == true)
            {
                //хэширование
            }
        }

        private void checkBox_Encrypt_Unchecked(object sender, RoutedEventArgs e)
        {
            if (checkBox_Encrypt.IsChecked == false)
            {
                //новое окно - запрос пароля

                //расхэширование
            }
        }
    }
}
