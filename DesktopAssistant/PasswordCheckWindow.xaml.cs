using System;
using System.Windows;

namespace DesktopAssistant
{
    /// <summary>
    /// Interaction logic for PasswordCheckWindow.xaml
    /// </summary>
    public partial class PasswordCheckWindow : Window
    {
        public bool isPasswordCorrect = false;
        
        // Главная форма должна в определеный момент "видеть", что на этой форме была нажата кнопка;
        // Для этого в этой форме создается событие, а в главной прописывается реакция на это событие
        public event EventHandler button_submitPassword_Clicked;

        public PasswordCheckWindow()    
        {
            InitializeComponent();
        }

        private void button_submitPassword_Click(object sender, RoutedEventArgs e)
        {
            button_submitPassword_Clicked.Invoke(sender, e);
        }
    }
}
