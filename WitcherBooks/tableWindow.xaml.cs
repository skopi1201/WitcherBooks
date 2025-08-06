using Domain.enums;
using Domain.Klase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for tableWindow.xaml
    /// </summary>
    public partial class tableWindow : Window
    {
        public ObservableCollection<Book> books { get; set; }
        public DataIO serializer = new DataIO();
        private UserType currentUserType;
        private MediaPlayer _bgMusicPlayer;
        public tableWindow(UserType type)
        {
            InitializeComponent();
            InitializeBackgroundMusic();

            currentUserType = type;
            checkUserType(currentUserType);
            books = serializer.DeSerializeObject<ObservableCollection<Book>>("Books.xml");


            if (books == null)
            {
                books = new ObservableCollection<Book>();
            }

           
                //updates every object path to the path relative to to exe file
                foreach (Book book in books)
                {
                    string pathtoexe = AppContext.BaseDirectory;
                    string fullPath = book.Imgpath;
                    string imagesPath = fullPath.Substring(fullPath.IndexOf(@"images"));
                    string relpath = pathtoexe + imagesPath;
                    book.Imgpath = new BitmapImage(new Uri(relpath)).ToString();
                }
           
            DataContext = this;
        }

        public void checkUserType(UserType type)
        {
            if (type == UserType.Administrator)
            {
                addBT.Visibility = Visibility.Visible;
                delBT.Visibility = Visibility.Visible;
                addBT.IsEnabled = true;
                delBT.IsEnabled = true;
            }
            else if (type == UserType.Consumer)
            {
                addBT.Visibility = Visibility.Hidden;
                delBT.Visibility = Visibility.Hidden;
                addBT.IsEnabled = false;
                delBT.IsEnabled = false;
            }

        }
        private void logOutBT_Click(object sender, RoutedEventArgs e)
        {
            serializer.SerializeObject<ObservableCollection<Book>>(books, "Books.xml");
            _bgMusicPlayer.IsMuted = true;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();


        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _bgMusicPlayer.IsMuted = true;
            serializer.SerializeObject<ObservableCollection<Book>>(books, "Books.xml");

        }
        private void addBT_Click(object sender, RoutedEventArgs e)
        {
            serializer.SerializeObject<ObservableCollection<Book>>(books, "Books.xml");
            _bgMusicPlayer.IsMuted = true;
            addWindow addWindow = new addWindow();

            addWindow.Show();
            this.Close();

        }
        private void delBT_Click(object sender, RoutedEventArgs e)
        {


            var result = MessageBox.Show("Are you sure you want to delete the selected item/s?","Confirm Deletion", MessageBoxButton.YesNo,MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var toRemove = books.Where(b => b.isDeleting).ToList();
                foreach (var book in toRemove)
                {
                    books.Remove(book);
                }
                serializer.SerializeObject<ObservableCollection<Book>>(books, "Books.xml");
            }
            else
            {
                return;
            }


          
        }
        private void SelectAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null) return;

            bool newVal = checkBox.IsChecked == true;


            foreach (var book in books)
            {
                book.isDeleting = newVal;
            }
        }
        private void hyperlink_Click(object sender, RoutedEventArgs e)
        {
            _bgMusicPlayer.IsMuted = true;
            serializer.SerializeObject<ObservableCollection<Book>>(books, "Books.xml");
            if (sender is Hyperlink link && link.Tag is Book book)
            {
                if (currentUserType == UserType.Administrator)
                {
                    // Open the editable window
                    BookEditWindow editWindow = new BookEditWindow(book);
                    editWindow.Show();
                    this.Close();
                }
                else
                {
                    // Open read-only detail view
                    BookDetailWindow detailWindow = new BookDetailWindow(book);
                    detailWindow.Show();
                    this.Close();
                }
            }
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
    }
}

