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

namespace DesktopAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DateTimeWindow dateTimeWindow;
        bool isTextEncrypted = false;

        public MainWindow()
        {
            InitializeComponent();

            textBox_Clipboard_1.Text = Properties.Settings.Default.Clipboard_1.ToString();
            textBox_Clipboard_2.Text = Properties.Settings.Default.Clipboard_2.ToString();
            textBox_Clipboard_3.Text = Properties.Settings.Default.Clipboard_3.ToString();
            textBox_Clipboard_4.Text = Properties.Settings.Default.Clipboard_4.ToString();
            textBox_Clipboard_5.Text = Properties.Settings.Default.Clipboard_5.ToString();

            this.Closed += (s, e) => Application.Current.Shutdown();

            if (!isTextEncrypted)
            {
                this.Closed += (s, e) => Properties.Settings.Default.Save();
            }
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
        private void textBox_Clipboard_1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isTextEncrypted)
            {
                Properties.Settings.Default.Clipboard_1 = textBox_Clipboard_1.Text;
            }
        }
        private void textBox_Clipboard_2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isTextEncrypted)
            {
                Properties.Settings.Default.Clipboard_2 = textBox_Clipboard_2.Text;
            }
        }
        private void textBox_Clipboard_3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isTextEncrypted)
            {
                Properties.Settings.Default.Clipboard_3 = textBox_Clipboard_3.Text;
            }
        }
        private void textBox_Clipboard_4_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isTextEncrypted)
            {
                Properties.Settings.Default.Clipboard_4 = textBox_Clipboard_4.Text;
            }
        }
        private void textBox_Clipboard_5_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isTextEncrypted)
            {
                Properties.Settings.Default.Clipboard_5 = textBox_Clipboard_5.Text;
            }
        }


        // Кнопка сохранения введеного текста в буфер обмена
        private void SaveTextToClipBoard(string text)
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


        public event EventHandler tasklistTextChanged;

        private void textBox_Tasklist_1_TextChanged(object sender, TextChangedEventArgs e)
        {
            tasklistTextChanged += (s, e) =>
            {
                File.WriteAllTextAsync("DesktopAssistant_TaskHistory.txt", $" {DateTime.Now.ToString()} {textBox_Tasklist_1.Text}\n");
            };

            if (checkBox_SaveInHistory.IsChecked == true)
            {
                tasklistTextChanged.Invoke(sender, e);
            }

        }

        private void button_Tasklist_1_Done_Click(object sender, RoutedEventArgs e)
        {
            textBox_Tasklist_1.Foreground = Brushes.LightGreen;
            textBox_Tasklist_1.IsEnabled = false;

        }
    }
}
