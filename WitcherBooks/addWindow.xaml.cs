using Domain.enums;
using Domain.Klase;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace WitcherBooks
{
    /// <summary>
    /// Interaction logic for addWindow.xaml
    /// </summary>
    public partial class addWindow : Window
    {
        public event Action<Book> BookCreated;
        public ObservableCollection<Book> books { get; set; }
        public DataIO serializer = new DataIO();
        private MediaPlayer _bgMusicPlayer;
        public addWindow()
        {
            InitializeComponent();
            InitializeBackgroundMusic();
            books = serializer.DeSerializeObject<ObservableCollection<Book>>("Books.xml");
            if (books == null)
            {
                books = new ObservableCollection<Book>();
            }
        }
        private void addBT_Click(object sender, RoutedEventArgs e)
        {


            bool hasError = false;

            
            titleError.Visibility = Visibility.Collapsed;
            descError.Visibility = Visibility.Collapsed;
            imageError.Visibility = Visibility.Collapsed;

           
            if (string.IsNullOrWhiteSpace(titleTB.Text))
            {
                titleError.Visibility = Visibility.Visible;
                hasError = true;
            }

           
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            if (string.IsNullOrWhiteSpace(textRange.Text.Trim()))
            {
                descError.Visibility = Visibility.Visible;
                hasError = true;
            }

            
            if (imagePreview.Source == null)
            {
                imageError.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (hasError)
                return;


            _bgMusicPlayer.IsMuted = true;
            Random random = new Random();
            int randomNumber;
            do
            {
                randomNumber = random.Next(100, 1000);
            } while (books.Any(b => b.Id == randomNumber));

            DateTime now = DateTime.Now;




            string filePath = "rtfdata/" + randomNumber.ToString() + ".rtf";
            using FileStream fs = new FileStream(filePath, FileMode.Create);
            textRange.Save(fs, DataFormats.Rtf);

            Book book = new Book(randomNumber, titleTB.Text, imagePreview.Source.ToString(), filePath, now.ToString());
            books.Add(book);
            serializer.SerializeObject<ObservableCollection<Book>>(books, "Books.xml");
            tableWindow tw = new tableWindow(UserType.Administrator);
            tw.Show();
            this.Close();
        }
        private void addImgBT_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openDialog.FilterIndex = 1;
            string imagesFolder = Path.Combine(AppContext.BaseDirectory, "images");
            openDialog.InitialDirectory = imagesFolder;

            bool? result = openDialog.ShowDialog();

            if (result == true)
            {
                string selectedFile = openDialog.FileName;

               
                if (selectedFile.StartsWith(imagesFolder, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        imagePreview.Source = new BitmapImage(new Uri(selectedFile));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to load image:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select an image from the allowed images folder only.", "Invalid File", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }




        }
        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleBold.Execute(null, richTextBox);
        }
        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleItalic.Execute(null, richTextBox);
        }
        private void FontSizeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontSizeBox.SelectedItem is ComboBoxItem selectedItem)
            {
                double size = Convert.ToDouble(selectedItem.Content);
                richTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, size);
            }
        }
        private void Color_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.Selection.ApplyPropertyValue(
                TextElement.ForegroundProperty,
                new SolidColorBrush(Colors.Black));
        }
        private void Underline_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleUnderline.Execute(null, richTextBox);
        }
        private void FontFamilyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string font = selectedItem.Content.ToString();
                richTextBox.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, new FontFamily(font));
            }
        }
        private void backBT_Click(object sender, RoutedEventArgs e)
        {
            _bgMusicPlayer.IsMuted = true;
            tableWindow tw = new tableWindow(UserType.Administrator);
            tw.Show();
            this.Close();
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
        private void titleTB_TextChanged(object sender, TextChangedEventArgs e)
        {
           
                titleError.Visibility = Visibility.Collapsed;
            

        }
    }
}
