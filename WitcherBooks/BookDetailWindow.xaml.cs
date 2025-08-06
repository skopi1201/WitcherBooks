using Domain.Klase;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;


namespace WitcherBooks
{
    /// <summary>
    /// Interaction logic for BookDetailWindow.xaml
    /// </summary>
    
    public partial class BookDetailWindow : Window
    {
        private MediaPlayer _bgMusicPlayer;
        public BookDetailWindow(Book book)
        {
            InitializeComponent();
            InitializeBackgroundMusic();
            TitleBlock.Text = book.Title;
            DateBlock.Text = $"Added on: {book.Date}";
            ImagePreview.Source = new BitmapImage(new Uri(book.Imgpath));

            // Load RTF content
            string filePath = book.Rtfpath;
            if (File.Exists(filePath))
            {
                using FileStream fs = new FileStream(filePath, FileMode.Open);
                TextRange range = new TextRange(ContentBox.Document.ContentStart, ContentBox.Document.ContentEnd);
                range.Load(fs, DataFormats.Rtf);
            }
        }

        private void backBT_Click(object sender, RoutedEventArgs e)
        {
            _bgMusicPlayer.IsMuted = true;
            tableWindow tw = new tableWindow(Domain.enums.UserType.Consumer);
            tw.Show();
            
            this.Close();
        }
        private void InitializeBackgroundMusic()
        {
            _bgMusicPlayer = new MediaPlayer();
            _bgMusicPlayer.Open(new Uri("music/bg1.mp3", UriKind.Relative)); 
            _bgMusicPlayer.Volume = 0.01; // Full 
            _bgMusicPlayer.MediaEnded += (s, e) => _bgMusicPlayer.Position = TimeSpan.Zero; 
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
    }
}

