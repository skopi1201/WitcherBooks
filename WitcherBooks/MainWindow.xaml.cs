using Domain.enums;
using Domain.Klase;
using Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WitcherBooks
{
    public partial class MainWindow : Window
    {
        private MediaPlayer _bgMusicPlayer;
        public MainWindow()
        {
            InitializeComponent();

            InitializeBackgroundMusic();
        }

        public void loggingIn(UserType type)
        {
            tableWindow tw  = new tableWindow(type);
            tw.Show();
           
            this.Close();
        }
    
        private void exitBT_Click(object sender, RoutedEventArgs e)
        { 

                this.Close();
           
        }

        private void logInBT_Click(object sender, RoutedEventArgs e)
        {
            string user = Tb_Username.Text.Trim();
            string pass = Tb_Password.Text.Trim();


           
                bool hasError = false;

                usernameError.Visibility = Visibility.Collapsed;
                passwordError.Visibility = Visibility.Collapsed;

                if (string.IsNullOrWhiteSpace(Tb_Username.Text))
                {
                    usernameError.Visibility = Visibility.Visible;
                    hasError = true;
                }

                if (string.IsNullOrWhiteSpace(Tb_Password.Text))
                {
                    passwordError.Visibility = Visibility.Visible;
                    hasError = true;
                }

                if (hasError)
                    return;

                // Proceed with login logic...
            


            LogInService login = new LogInService();
            bool isLogged;
            UserType type;

            (isLogged, type) = login.LogIN(user, pass);

            if (isLogged)
            {
                loggingIn(type);
            }
            else
            {
                MessageBox.Show("Incorrect username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
            Tb_Password.Text = "";
            Tb_Username.Text = "";
        }

        private void InitializeBackgroundMusic()
        {
            _bgMusicPlayer = new MediaPlayer();
            _bgMusicPlayer.Open(new Uri("music/bg1.mp3", UriKind.Relative)); // Update the path accordingly
            _bgMusicPlayer.Volume = 0.01; // Full volume
            _bgMusicPlayer.MediaEnded += (s, e) => _bgMusicPlayer.Position = TimeSpan.Zero; // Loop
            _bgMusicPlayer.Play();
        }
        private void MuteButton_Checked(object sender, RoutedEventArgs e)
        {
            _bgMusicPlayer.IsMuted = true;
        }

        private void MuteButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _bgMusicPlayer.IsMuted = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _bgMusicPlayer.IsMuted = true;
        }

        private void Tb_Username_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Tb_Username.Text))
                usernameError.Visibility = Visibility.Collapsed;
        }

        private void Tb_Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Tb_Password.Text))
                passwordError.Visibility = Visibility.Collapsed;
        }

    }
}